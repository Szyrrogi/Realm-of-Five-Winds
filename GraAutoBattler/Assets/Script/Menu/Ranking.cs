using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data.SqlClient; // Obsługa SQL Server
using System; // Konwersja typów (Convert)
using System.Threading.Tasks; // Do obsługi asynchroniczności

public class Ranking : MonoBehaviour
{
    public Rekord[] rekordy;

    public async void Start()
    {
        await SprawdzStatystyki(); // Czekaj na zakończenie metody asynchronicznej
    }

    async Task SprawdzStatystyki()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(DB.conStr))
            {
                await con.OpenAsync(); // Asynchroniczne otwarcie połączenia

                // 1. Sortowanie PlayerId według LP
                string querySort = @"
                    SELECT 
                        Id AS PlayerId, 
                        LP
                    FROM Players
                    ORDER BY LP DESC;
                ";

                SqlCommand cmdSort = new SqlCommand(querySort, con);
                SqlDataReader reader = await cmdSort.ExecuteReaderAsync(); // Asynchroniczne wykonanie zapytania

                // 2. Wyświetlenie PlayerId na pierwszym, drugim i trzecim miejscu
                int position = 0;
                while (await reader.ReadAsync()) // Asynchroniczne odczytanie wyniku
                {
                    if (position == 0)
                    {
                        // Pierwszy gracz
                        int topPlayerId = reader.GetInt32(0); // Pierwszy PlayerId
                        rekordy[1].SetStats(topPlayerId); // Ustaw statystyki dla pierwszego gracza
                    }
                    else if (position == 1)
                    {
                        // Drugi gracz
                        int secondPlayerId = reader.GetInt32(0); // Drugi PlayerId
                        rekordy[2].SetStats(secondPlayerId); // Ustaw statystyki dla drugiego gracza
                    }
                    else if (position == 2)
                    {
                        // Trzeci gracz
                        int thirdPlayerId = reader.GetInt32(0); // Trzeci PlayerId
                        rekordy[3].SetStats(thirdPlayerId); // Ustaw statystyki dla trzeciego gracza
                    }
                    else if (position == 3)
                    {
                        // Czwarty gracz
                        int fourthPlayerId = reader.GetInt32(0); // Czwarty PlayerId
                        rekordy[4].SetStats(fourthPlayerId); // Ustaw statystyki dla czwartego gracza
                    }
                    else if (position == 4)
                    {
                        // Piąty gracz
                        int fifthPlayerId = reader.GetInt32(0); // Piąty PlayerId
                        rekordy[5].SetStats(fifthPlayerId); // Ustaw statystyki dla piątego gracza
                    }
                    else
                    {
                        break; // Nie potrzebujemy więcej pozycji
                    }

                    position++;
                }

                reader.Close();

                // 3. Znalezienie pozycji gracza o PlayerId = 2 (lub aktualnie zalogowanego gracza)
                if (Login.zalogowano)
                {
                    string queryPosition = @"
                        WITH RankedPlayers AS (
                            SELECT 
                                Id AS PlayerId, 
                                LP,
                                ROW_NUMBER() OVER (ORDER BY LP DESC) AS Position
                            FROM Players
                        )
                        SELECT Position
                        FROM RankedPlayers
                        WHERE PlayerId = @PlayerId;
                    ";

                    SqlCommand cmdPosition = new SqlCommand(queryPosition, con);
                    cmdPosition.Parameters.AddWithValue("@PlayerId", PlayerManager.Id);
                    object result = await cmdPosition.ExecuteScalarAsync(); // Wykonaj zapytanie

                    if (result != null)
                    {
                        position = Convert.ToInt32(result); // Konwersja wyniku na int
                        rekordy[0].gameObject.SetActive(true);
                        rekordy[0].number = position;
                        rekordy[0].SetStats(PlayerManager.Id);
                    }
                    else
                    {
                        Debug.Log("Gracz o PlayerId = " + PlayerManager.Id + " nie został znaleziony.");
                    }
                }
                else
                {
                    rekordy[0].gameObject.SetActive(false);
                }
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
}