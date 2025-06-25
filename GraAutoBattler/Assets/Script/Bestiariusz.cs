using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;
using System.Data.SqlClient;


public class Bestiariusz : MonoBehaviour
{
    public CharacterManager characterManager;
    public SynergyManager synergyManager;
    public List<GameObject> Show;
    public List<Synergy> synergie;
    public int Page;
    public List<Image> Face;
    public List<DescriptionManager> Opisy;
    public List<TextMeshProUGUI> Lore;

    public Fraction.fractionType Frakcja;
    public int type;

    public GameObject OpisSzczegolu;
    public TextMeshProUGUI OpisCaly;

    public GameObject Synergie;
    public GameObject Achievements;
    public GameObject Zasady;
    public List<GameObject> Osiagniecia;
    public Vector2[] transformY;


    public void ShowSzczegoly(int i)
    {
        int index = i + Page * 4;
        OpisCaly.text = Show[index].GetComponent<Unit>().Lore;
        OpisSzczegolu.SetActive(true);
    }

    public void Showaj()
    {
        OpisSzczegolu.SetActive(false);
    }

    public void Next()
    {
        if (type == -1 && Page * 8 < Show.Count - 8)
        {
            Page++;
            FillInterfaceWithoutEvolutions();
        }
        else
        if (Page * 4 < Show.Count - 4)
        {
            Page++;
            Complete();
        }
    }

    public void Prev()
    {
        if (type == -1 && Page != 0)
        {
            Page--;
            FillInterfaceWithoutEvolutions();
        }
        else
        if (Page != 0)
        {
            Page--;
            Complete();
        }
    }

    public void Start()
    {
        Page = 0;
        Show = characterManager.characters;
        Show = Filter(Show);
        Complete();
    }

    public List<Synergy> SortSynergy(List<Synergy> prevSynergy, string napis)
    {
        return prevSynergy
            .Where(s => 
                napis.Contains($"S{s.Id}S")  // Sprawdza, czy napis zawiera np. "S4S" dla Id=4
            )
            .ToList();
    }

    public void ShowZasadyVoid()
    {
        Synergie.SetActive(false);
        Achievements.SetActive(false);
        Zasady.SetActive(true);
    }

    public void ShowAchivmentsVoid()
    {
        Synergie.SetActive(false);
        Achievements.SetActive(true);
        Zasady.SetActive(false);

        string firstLine = "";
        string savePath1 = Application.dataPath + "/Save/Achivments.txt";

        // Sprawdź, czy plik istnieje
        if (File.Exists(savePath1))
        {
            // Odczytaj wszystkie linie z pliku
            string[] allLines = File.ReadAllLines(savePath1);

            // Jeśli plik nie jest pusty, weź pierwszą linię
            if (allLines.Length > 0)
            {
                firstLine = allLines[0];
                Debug.Log("Pierwsza linia: " + firstLine);
            }
            else
            {
                Debug.LogWarning("Plik jest pusty!");
            }
        }
        else
        {
            Debug.LogError("Plik nie istnieje: " + savePath1);
        }
        int j = 0;
        foreach (GameObject New in Osiagniecia)
        {
            Debug.Log("weszło");
            GameObject newSynergy = Instantiate(New, Achievements.transform); // Parent od razu
            newSynergy.SetActive(true);
            if (!firstLine.Contains($"S{j}S"))
            {
                newSynergy.GetComponent<Image>().color = Color.gray;
            }
            RectTransform rectTransform = newSynergy.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                //rectTransform.anchoredPosition = transformY[0]; // Jeśli transformY[0] to Vector2
                rectTransform.localPosition = new Vector3(transformY[j / 4].x, transformY[j % 4].y, 0);
            }
            j++;
        }
    }

    public void ShowSynergyVoid()
    {
        Synergie.SetActive(true);
        Achievements.SetActive(false);
        Zasady.SetActive(false);

        string firstLine = "";
        string savePath1 = Application.dataPath + "/Save/Synergy.txt";

        // Sprawdź, czy plik istnieje
        if (File.Exists(savePath1))
        {
            // Odczytaj wszystkie linie z pliku
            string[] allLines = File.ReadAllLines(savePath1);
            
            // Jeśli plik nie jest pusty, weź pierwszą linię
            if (allLines.Length > 0)
            {
                firstLine = allLines[0];
                Debug.Log("Pierwsza linia: " + firstLine);
            }
            else
            {
                Debug.LogWarning("Plik jest pusty!");
            }
        }
        else
        {
            Debug.LogError("Plik nie istnieje: " + savePath1);
        }

        List <Synergy> prevSynergy = synergyManager.Synergie;
        prevSynergy = SortSynergy(prevSynergy, firstLine);

        Debug.Log(prevSynergy.Count);
        int ile = 13;
        for(int j = 0; j < prevSynergy.Count; j++)
        { 
            GameObject newSynergy = Instantiate(prevSynergy[j].opis.gameObject, Synergie.transform); // Parent od razu
            newSynergy.SetActive(true);
            RectTransform rectTransform = newSynergy.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                //rectTransform.anchoredPosition = transformY[0]; // Jeśli transformY[0] to Vector2
                rectTransform.localPosition = new Vector3(transformY[j/4].x, transformY[j%4].y, 0);
            }
        }
    }

    public void Complete()
    {
        foreach (Transform child in Synergie.transform)
            {
                Destroy(child.gameObject); // Niszczy każdy obiekt-dziecko
            }
     foreach (Transform child in Achievements.transform)
            {
                Destroy(child.gameObject); // Niszczy każdy obiekt-dziecko
            }
        if(type == 2)
        {
            ShowSynergyVoid();
        }
        else
        {
            if (type == 3)
            {
                ShowAchivmentsVoid();
            }
            else
            {
                Synergie.SetActive(false);
                Achievements.SetActive(false);
                Zasady.SetActive(false);
            }
        }
        if (type == 0)
        {
            // Oryginalna logika dla type == 0 (4 elementy × 2 miejsca = 8 slotów)
            for (int i = 0; i < 4; i++)
            {
                int index = i + Page * 4;

                if (index >= Show.Count || Show[index] == null)
                {
                    Face[i*2].gameObject.SetActive(false);
                    Face[i*2 + 1].gameObject.SetActive(false);
                    Opisy[i*2].gameObject.SetActive(false);
                    Opisy[i*2 + 1].gameObject.SetActive(false);
                    
                    if (i < Lore.Count)
                        Lore[i].text = "";
                    
                    continue;
                }

                // Aktywuj sloty
                Face[i*2].gameObject.SetActive(true);
                Face[i*2 + 1].gameObject.SetActive(true);
                Opisy[i*2].gameObject.SetActive(true);
                Opisy[i*2 + 1].gameObject.SetActive(true);

                var spriteRenderer = Show[index].GetComponent<SpriteRenderer>();
                var herosComponent = Show[index].GetComponent<Heros>();
                var unitComponent = Show[index].GetComponent<Unit>();

                // Postać podstawowa
                Face[i*2].sprite = spriteRenderer?.sprite;
                Opisy[i*2].unit = unitComponent;
                Opisy[i*2].gameObject.SetActive(unitComponent != null);

                // Ewolucja (jeśli istnieje)
                if (herosComponent?.EvolveHeroes != null)
                {
                    var evolveSpriteRenderer = herosComponent.EvolveHeroes.GetComponent<SpriteRenderer>();
                    Face[i*2 + 1].sprite = evolveSpriteRenderer?.sprite;
                    
                    var evolveUnit = herosComponent.EvolveHeroes.GetComponent<Unit>();
                    Opisy[i*2 + 1].unit = evolveUnit;
                    Opisy[i*2 + 1].gameObject.SetActive(evolveUnit != null);
                }
                else
                {
                    Face[i*2 + 1].sprite = null;
                    Opisy[i*2 + 1].gameObject.SetActive(false);
                }

                // Lore tylko dla type == 0
                if (i < Lore.Count)
                {
                    Lore[i].text = unitComponent != null 
                        ? (unitComponent.Lore?.Length > 100 
                            ? unitComponent.Lore.Substring(0, 100) + " (czytaj dalej)" 
                            : unitComponent.Lore) ?? ""
                        : "";
                }
            }
        }
        if(type == 1) // type == 1
        {
            // Nowa logika dla type == 1 (8 elementów w 8 slotach, bez ewolucji i bez LORE)
            for (int i = 0; i < 8; i++)
            {
                int index = i + Page * 8;

                if (index >= Show.Count || Show[index] == null)
                {
                    Face[i].gameObject.SetActive(false);
                    Opisy[i].gameObject.SetActive(false);
                    continue;
                }

                Face[i].gameObject.SetActive(true);
                Opisy[i].gameObject.SetActive(true);

                var spriteRenderer = Show[index].GetComponent<SpriteRenderer>();
                var unitComponent = Show[index].GetComponent<Unit>();

                Face[i].sprite = spriteRenderer?.sprite;
                Opisy[i].unit = unitComponent;
                Opisy[i].gameObject.SetActive(unitComponent != null);
            }
            for(int i = 0; i < 4; i++)
            {
                Lore[i].text = "";
            }
        }
    }

    private void FillInterfaceWithoutEvolutions()
    {
        // 8 slotów bez ewolucji i bez LORE
        for (int i = 0; i < 8; i++)
        {
            int index = i + Page * 8;
            bool slotValid = index < Show.Count && Show[index] != null;

            Face[i].gameObject.SetActive(slotValid);
            Opisy[i].gameObject.SetActive(slotValid);

            if (slotValid)
            {
                var spriteRenderer = Show[index].GetComponent<SpriteRenderer>();
                var unitComponent = Show[index].GetComponent<Unit>();

                Face[i].sprite = spriteRenderer?.sprite;
                Opisy[i].unit = unitComponent;
            }
        }

        // Wyczyść LORE
        for (int i = 0; i < 4; i++)
        {
            Lore[i].text = "";
        }
    }


    public void SetType(int newType)
    {
        type = newType;
        Start();
    }

    public List<GameObject> Filter(List<GameObject> gameObjects)
    {
        if(type == 0)
        return gameObjects
            .Where(go => go.GetComponent<Heros>() != null && !go.GetComponent<Heros>().Evolution
            && go.GetComponent<Heros>().Star != 0 && go.GetComponent<Heros>().fraction == Frakcja)
            .OrderBy(go => go.GetComponent<Heros>().Cost)
            .ToList();
        else
        return gameObjects
            .Where(go => go.GetComponent<Building>() != null && go.GetComponent<Unit>().Star != 0 && 
             go.GetComponent<Unit>().fraction == Frakcja)
            .OrderBy(go => go.GetComponent<Unit>().Cost)
            .ToList();

    }
    public void UstawFrakcja(int frakcjaIndex)
    {
        if (frakcjaIndex == 5)
        {
            type = -1;
            Page = 0;
            Show = characterManager.characters;
            Show = characterManager.characters
            .Where(go => go.GetComponent<Heros>() != null 
                  && go.GetComponent<Heros>().Star == 0)
            .OrderBy(go => go.GetComponent<Heros>().Cost)
            .ToList();

            foreach (Transform child in Synergie.transform)
                {
                    Destroy(child.gameObject); // Niszczy każdy obiekt-dziecko
                }
        foreach (Transform child in Achievements.transform)
                {
                    Destroy(child.gameObject); // Niszczy każdy obiekt-dziecko
                }

            // Obsługa specjalnych typów
            if (type == 2)
            {
                ShowSynergyVoid();
                return;
            }
            else if (type == 3)
            {
                ShowAchivmentsVoid();
                return;
            }
            else
            {
                Synergie.SetActive(false);
                Achievements.SetActive(false);
                Zasady.SetActive(false);
            }

            FillInterfaceWithoutEvolutions();
    
        return;
        }
        // Sprawdź czy indeks jest w zakresie dostępnych frakcji
            if (System.Enum.IsDefined(typeof(Fraction.fractionType), frakcjaIndex))
            {
                Frakcja = (Fraction.fractionType)frakcjaIndex;
                Page = 0;
                Start(); // Odśwież widok
            }
            else
            {
                Debug.LogWarning($"Nieznany indeks frakcji: {frakcjaIndex}");
                // Możesz tutaj ustawić domyślną frakcję lub zignorować
            }
    }

    public static void AddAchivments(int nr)
    {
        string savePath1 = Application.dataPath + "/Save/Achivments.txt";

        if (File.Exists(savePath1))
        {
            string[] allLines = File.ReadAllLines(savePath1);
            
            if (allLines.Length > 0)
            {
                string firstLine = allLines[0];
                Debug.Log("Pierwsza linia: " + firstLine);

                if (!firstLine.Contains($"S{nr}S"))
                {
                    // Dopisz nowe osiągnięcie do pliku
                    File.AppendAllText(savePath1, $"{nr}S");
                    Debug.Log($"Dopisano S{nr}S do pliku");
                    
                    // Przekaż CAŁĄ zawartość pliku do funkcji aktualizującej bazę danych
                    string allAchievements = File.ReadAllText(savePath1);
                    UpdateDatabaseAchievement(allAchievements);
                }
            }
            else
            {
                File.WriteAllText(savePath1, $"S{nr}S");
                Debug.Log($"Utworzono plik z S{nr}S");
                UpdateDatabaseAchievement($"S{nr}S");
            }
        }
        else
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath1));
            File.WriteAllText(savePath1, $"S{nr}S");
            Debug.Log($"Utworzono nowy plik z S{nr}S");
            UpdateDatabaseAchievement($"S{nr}S");
        }
    }

    private static void UpdateDatabaseAchievement(string achievementId)
    {
    try
        {
            using (SqlConnection con = DB.Connect(DB.conStr))
            {
                // Zapytanie SQL do aktualizacji FaceId dla gracza o danym Id
                string query = "UPDATE Players SET Achievements = @new WHERE Id = @PlayerId";
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

  
}
