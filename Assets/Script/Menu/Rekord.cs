using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Data.SqlClient; // Obsługa SQL Server
using System.Threading.Tasks; // Do obsługi asynchroniczności
using System; // Dodaj tę linię, aby użyć klasy Convert
using System.Linq;

/*
SELECT Fraction, Count(*) AS 'count', ROUND(AVG(CAST(Win AS float)), 1) AS AvgWin  
FROM Stats
WHERE Round - win != 0 OR Lose = 3
GROUP BY Fraction
ORDER BY AvgWin   DESC

*/
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

    public bool own;

    public async void SetStats(int Id)
    {
        this.Id = Id;
        numberText.text = own ? (number).ToString() : (number + Ranking.pageSize).ToString();

        try
        {
            using (SqlConnection con = new SqlConnection(DB.conStr))
            {
                await con.OpenAsync();

                // 1. Pobierz nazwę gracza
                string queryName = "SELECT Name FROM Players WHERE Id = @Id";
                SqlCommand cmdName = new SqlCommand(queryName, con);
                cmdName.Parameters.AddWithValue("@Id", Id);
                object resultName = await cmdName.ExecuteScalarAsync();
                name.text = (resultName != null && resultName != DBNull.Value) ? resultName.ToString() : "Brak";

                // 2. Pobierz Face
                string queryFace = "SELECT Face FROM Players WHERE Id = @Id";
                SqlCommand cmdFace = new SqlCommand(queryFace, con);
                cmdFace.Parameters.AddWithValue("@Id", Id);
                object resultFace = await cmdFace.ExecuteScalarAsync();

                int facee = (resultFace != null && resultFace != DBNull.Value) ? Convert.ToInt32(resultFace) : 0;
                face.sprite = playerManager.spriteFace[Mathf.Clamp(facee, 0, playerManager.spriteFace.Length - 1)];

                // 3. Pobierz średnią wygranych
                string queryStats = @"
                    SELECT ROUND(AVG(CAST(Win AS float)), 1) AS AvgWin
                    FROM Stats
                    WHERE PlayerId = @Id AND (Round - Win != 0 OR Lose = 3);
                ";
                SqlCommand cmdStats = new SqlCommand(queryStats, con);
                cmdStats.Parameters.AddWithValue("@Id", Id);
                SqlDataReader reader = await cmdStats.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    avr.text = (reader["AvgWin"] != DBNull.Value) ? Convert.ToDouble(reader["AvgWin"]).ToString() : "0";
                }
                else
                {
                    avr.text = "0";
                }
                reader.Close();

                // 4. Pobierz LP
                string queryLP = "SELECT LP FROM Players WHERE Id = @Id";
                SqlCommand cmdLP = new SqlCommand(queryLP, con);
                cmdLP.Parameters.AddWithValue("@Id", Id);
                object resultLP = await cmdLP.ExecuteScalarAsync();
                double sum = (resultLP != null && resultLP != DBNull.Value) ? Convert.ToDouble(resultLP) : 0;

                // Wylicz rank
                if (sum < 1500)
                {
                    RankImage.sprite = ranksSprite[(int)sum / 300];
                    RankNumber.text = (((int)sum % 300) / 100 + 1).ToString();
                    LP.text = ((int)sum % 100) + " LP";
                }
                else
                {
                    RankImage.sprite = ranksSprite[5];
                    RankNumber.text = "";
                    LP.text = (sum - 1500) + " LP";
                }

                // 5. Pobierz najczęstszą frakcję
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

                if (await fractionReader.ReadAsync())
                {
                    string fraction = (fractionReader["Fraction"] != DBNull.Value) ? fractionReader["Fraction"].ToString() : "00000";
                    UstawFrakcje(fraction);
                }
                else
                {
                    UstawFrakcje("00000"); // brak frakcji
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