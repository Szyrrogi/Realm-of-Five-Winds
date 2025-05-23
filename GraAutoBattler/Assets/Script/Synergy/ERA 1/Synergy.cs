using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Data.SqlClient;

public class Synergy : MonoBehaviour
{
    public List<Unit> units;
    public int Id;
    
    public GameObject opis;
    public bool showOpis;
    public bool Enemy;

    public void Start()
    {
        if(!Enemy)
        {
            string savePath1 = Application.dataPath + "/Save/Synergy.txt";

            // Sprawdź, czy plik istnieje
            if (File.Exists(savePath1))
            {
                // Odczytaj wszystkie linie z pliku
                string[] allLines = File.ReadAllLines(savePath1);
                
                // Jeśli plik nie jest pusty, weź pierwszą linię
                if (allLines.Length > 0)
                {
                    string firstLine = allLines[0];
                    Debug.Log("Pierwsza linia: " + firstLine);

                    // Sprawdź czy firstLine zawiera obecne Id w formacie "SidS"
                    if (!firstLine.Contains($"S{Id}S"))
                    {
                        // Dopisz brakujące Id do pliku
                        File.AppendAllText(savePath1, $"{Id}S");
                        Debug.Log($"Dopisano S{Id}S do pliku");

                        string allSynergy = File.ReadAllText(savePath1);
                        UpdateDatabaseSynergy(allSynergy);
                    }
                }
                else
                {
                    // Jeśli plik jest pusty, zapisz pierwsze Id
                    File.WriteAllText(savePath1, $"S{Id}S");
                    Debug.Log($"Utworzono plik z S{Id}S");
                    string allSynergy = File.ReadAllText(savePath1);
                    UpdateDatabaseSynergy(allSynergy);
                }
            }
            else
            {
                // Jeśli plik nie istnieje, utwórz go z obecnym Id
                Directory.CreateDirectory(Path.GetDirectoryName(savePath1)); // Utwórz folder jeśli nie istnieje
                File.WriteAllText(savePath1, $"S{Id}S");
                Debug.Log($"Utworzono nowy plik z S{Id}S");
                string allSynergy = File.ReadAllText(savePath1);
                UpdateDatabaseSynergy(allSynergy);
            }
        }
    }

    private static void UpdateDatabaseSynergy(string achievementId)
    {
    try
        {
            using (SqlConnection con = DB.Connect(DB.conStr))
            {
                // Zapytanie SQL do aktualizacji FaceId dla gracza o danym Id
                string query = "UPDATE Players SET Synergies = @new WHERE Id = @PlayerId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@new", achievementId);
                cmd.Parameters.AddWithValue("@PlayerId", PlayerManager.Id);
                // Wykonaj zapytanie
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Debug.Log("FaceId został pomyślnie zaktualizowany w bazie danych.");
                }
                else
                {
                    Debug.LogWarning("Nie znaleziono gracza o podanym Id.");
                }
            }
        }
        catch (SqlException ex)
        {
            Debug.LogError("Błąd bazy danych podczas aktualizacji FaceId: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Wystąpił nieoczekiwany błąd: " + ex.Message);
        }
    }

    public virtual IEnumerator BeforBattle()
    {
        Debug.Log("wykonano " + Id);
        yield return null;
    }

    public virtual void AfterBattle()
    {
        Debug.Log("wykonanoAfter " + Id);
    }

    public virtual bool CheckIsActive(int modify)
    {
        for (int i = 0; i < 3; i++)
        {
            if (EventSystem.eventSystem == null || EventSystem.eventSystem.GetComponent<FightManager>() == null)
                continue;

            Linia line = EventSystem.eventSystem.GetComponent<FightManager>().linie[i + modify];
            if (line == null || line.pola == null)
                continue;

            List<Unit> newUnits = new List<Unit>(units);
            foreach (var pole in line.pola)
            {
                if (pole == null || pole.unit == null)
                    continue;

                Unit poleUnit = pole.unit.GetComponent<Unit>();
                if (poleUnit == null)
                    continue;

                foreach (Unit unit in units)
                {
                    if (unit == null)
                        continue;

                    if (unit.Name == poleUnit.Name)
                    {
                        Unit unitToRemove = newUnits.Find(u => u != null && u.Name == poleUnit.Name);
                        if (unitToRemove != null)
                        {
                            newUnits.Remove(unitToRemove);
                        }
                    }
                }
                if (newUnits.Count == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

     public void OnMouseEnter()
    {
        showOpis = true;
        StartCoroutine(WaitDescription());
    }

    public void OnMouseExit()
    {
        showOpis = false;
        opis.SetActive(false);
    }

    IEnumerator WaitDescription()
    {
        // Czekaj przez określony czas
        yield return new WaitForSeconds(0.1f);

        if (showOpis)
        {
            // Pobierz RectTransform, jeśli opis jest elementem UI
            RectTransform opisRectTransform = opis.GetComponent<RectTransform>();

            // Sprawdź pozycję X (używając anchoredPosition dla UI)
            if (Enemy && opisRectTransform.anchoredPosition.x > 0)
            {
                // Ustaw nową pozycję
                opisRectTransform.anchoredPosition = new Vector2(-opisRectTransform.anchoredPosition.x, opisRectTransform.anchoredPosition.y);
            }

            // Aktywuj opis
            opis.SetActive(true);
        }
    }
}