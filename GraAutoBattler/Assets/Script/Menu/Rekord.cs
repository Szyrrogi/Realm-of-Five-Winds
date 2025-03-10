using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Data.SqlClient; // Obsługa SQL Server
using System.Threading.Tasks; // Do obsługi asynchroniczności
using System; // Dodaj tę linię, aby użyć klasy Convert
using System.Linq;

public class Rekord : MonoBehaviour
{
    public int Id;
    public int number;

    public TextMeshProUGUI name;
    public TextMeshProUGUI numberText;

    public Image RankImage;
    public Sprite[] ranksSprite;
    public TextMeshProUGUI RankNumber;
    public TextMeshProUGUI LP;

    public TextMeshProUGUI avr;
    public Image face;
    public Image[] wycinek;
    public PlayerManager playerManager;

    public async void SetStats(int Id)
    {
        this.Id = Id;
        numberText.text = number.ToString();

        try
        {
            using (SqlConnection con = new SqlConnection(DB.conStr))
            {
                await con.OpenAsync();

                // 1. Pobierz nazwę gracza z tabeli Players
                string queryName = "SELECT Name FROM Players WHERE Id = @Id";
                SqlCommand cmdName = new SqlCommand(queryName, con);
                cmdName.Parameters.AddWithValue("@Id", Id);
                object resultName = await cmdName.ExecuteScalarAsync();

                if (resultName != null)
                {
                    name.text = resultName.ToString(); // Ustaw nazwę gracza
                }
                else
                {
                    Debug.Log("Gracz o podanym Id nie został znaleziony.");
                    return;
                }

                // 2. Pobierz Face gracza z tabeli Players
                string queryFace = "SELECT Face FROM Players WHERE Id = @Id";
                SqlCommand cmdFace = new SqlCommand(queryFace, con);
                cmdFace.Parameters.AddWithValue("@Id", Id); // Użyj Id jako parametru

                object resultFace = await cmdFace.ExecuteScalarAsync(); // Wykonaj zapytanie

                if (resultFace != null)
                {
                    int facee = Convert.ToInt32(resultFace); // Konwersja wyniku na int

                    face.sprite = playerManager.spriteFace[facee]; // Ustaw sprite

                }
                else
                {
                    Debug.Log("Gracz o PlayerId = " + Id + " nie został znaleziony.");
                    return; // Przerwij metodę, jeśli gracz nie został znaleziony
                }

                // 3. Policz statystyki z tabeli Stats
                string queryStats = @"
                    SELECT 
                        ROUND(AVG(CAST(Win AS float)), 1) AS AvgWin
                    FROM Stats
                    WHERE PlayerId = @Id;
                ";

                SqlCommand cmdStats = new SqlCommand(queryStats, con);
                cmdStats.Parameters.AddWithValue("@Id", Id);
                SqlDataReader reader = await cmdStats.ExecuteReaderAsync();
                double sum = 0;
                if (await reader.ReadAsync())
                {
                    avr.text = Convert.ToDouble(reader["AvgWin"]).ToString();
                }
                else
                {
                    Debug.Log("Brak statystyk dla gracza o podanym Id.");
                }

                reader.Close();
                //
                // 2. Pobierz LP gracza z tabeli Players
                string queryLP = "SELECT LP FROM Players WHERE Id = @Id";
                SqlCommand cmdLP = new SqlCommand(queryLP, con);
                cmdLP.Parameters.AddWithValue("@Id", Id); // Użyj Id jako parametru

                object resultLP = await cmdLP.ExecuteScalarAsync(); // Wykonaj zapytanie

                if (resultLP != null)
                {
                    sum = Convert.ToDouble(resultLP); // Konwersja wyniku na double
                }
                else
                {
                    Debug.Log("Gracz o PlayerId = " + Id + " nie został znaleziony.");
                    return; // Przerwij metodę, jeśli gracz nie został znaleziony
                }

                RankImage.sprite = ranksSprite[(int)sum / 300];
                RankNumber.text = (((int)sum % 300) / 100 + 1).ToString();
                LP.text = ((int)sum % 100) + " LP";

                // 4. Pobierz frakcje gracza z tabeli Stats
                string queryFraction = @"
                    SELECT TOP 1 Fraction, COUNT(*) AS Count
                    FROM Stats
                    WHERE PlayerId = @Id
                    GROUP BY Fraction
                    ORDER BY Count DESC;
                ";

                SqlCommand cmdFraction = new SqlCommand(queryFraction, con);
                cmdFraction.Parameters.AddWithValue("@Id", Id);
                SqlDataReader fractionReader = await cmdFraction.ExecuteReaderAsync();

                while (await fractionReader.ReadAsync())
                {
                    string fraction = fractionReader["Fraction"].ToString();
                    int count = Convert.ToInt32(fractionReader["Count"]);
                    UstawFrakcje(fraction);
                }

                fractionReader.Close();
            }
        }
        catch (SqlException ex)
        {
            Debug.Log("Błąd bazy danych: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            Debug.Log("Wystąpił nieoczekiwany błąd: " + ex.Message);
        }
    }

    void UstawFrakcje(string napis)
    {
    
        int ilosc = napis.Count(c => c == '1');;
        int j = 1;
        for(int i = 0; i < 5; i++)
        {
                if (napis[i] == '1')
                {
                    UstawWycinek(wycinek[i], 0f, j / (float)ilosc);
                    j++;
                }
                else
                {
                    UstawWycinek(wycinek[i], 0f, 0f); // chce puste
                }
        }
        
        //UstawWycinek(wycinek[0], 0f, 1f / (float)ilosc);

    }


    void UstawWycinek(Image wycinek, float startFill, float fillAmount)
    {
        // Ustaw tryb na Filled
        wycinek.type = Image.Type.Filled;

        // Ustaw metodę wypełniania na Radial (wykres kołowy)
        wycinek.fillMethod = Image.FillMethod.Radial360;

        // Ustaw początek wypełniania na górę (360 stopni)
        wycinek.fillOrigin = (int)Image.Origin360.Top;

        // Ustaw wypełnienie na odpowiednią wartość
        wycinek.fillAmount = fillAmount;

        // Obróć wycinek, aby zaczynał się w odpowiednim miejscu
        wycinek.transform.rotation = Quaternion.Euler(0, 0, -startFill * 360f);
    }
}