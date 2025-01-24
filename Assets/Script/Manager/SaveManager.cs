using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private static string customPath;

    [System.Serializable]
    public class SaveData
    {
        public string playerName;
        public int number;
        public int levelUp;
        public List<SaveUnit> units = new List<SaveUnit>();
    }

    [System.Serializable]
    public class SaveUnit
    {
        public int Line;
        public int Pole;

        public int Id;
        public string Name;
        public int Cost;

        public int Initiative;
        public int MaxHealth;
        public int Health;
        public int Attack;
        public int Defense;
        public int Range;
        public int AP;
        public int MagicResist;
        public string Description;

        public int UpgradeLevel;
    }

    public static void Save(string playerName, int number, int levelUp)
    {
        // Upewnij się, że folder istnieje
        customPath = Application.dataPath + "/Save/Zapis.json";
        string directory = Path.GetDirectoryName(customPath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Tworzymy dane do zapisu
        SaveData data = new SaveData
        {
            playerName = playerName,
            number = number,
            levelUp = levelUp
        };

        // Pobieramy dane jednostek z planszy
        FightManager fightManager = EventSystem.eventSystem.GetComponent<FightManager>();
        foreach (Linia linia in fightManager.linie)
        {
            if(linia.nr < 3)
            {
                foreach (Pole pole in linia.pola)
                {
                    if (pole.unit != null)
                    {
                        Unit unit = pole.unit.GetComponent<Unit>();
                        SaveUnit unitData = new SaveUnit
                        {
                            Line = linia.nr,
                            Pole = pole.nr,
                            Id = unit.Id,
                            Name = unit.Name,
                            Cost = unit.Cost,
                            Initiative = unit.Initiative,
                            MaxHealth = unit.MaxHealth,
                            Health = unit.Health,
                            Attack = unit.Attack,
                            Defense = unit.Defense,
                            Range = unit.Range,
                            AP = unit.AP,
                            MagicResist = unit.MagicResist,
                            Description = unit.Description,
                            UpgradeLevel = unit.UpgradeLevel
                        };

                        // Dodajemy jednostkę do listy w SaveData
                        data.units.Add(unitData);
                    }
                }
            }
        }
        Debug.Log("TO: " +  Application.dataPath);
        
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(customPath, json);
        // Sprawdzenie, czy pierwszy plik istnieje i utworzenie go, jeśli nie


        // Drugi zapis do folderu Wave z unikalną nazwą
        // string name = StatsManager.Round + "_Owca_" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        // string secondPath = Path.Combine(Application.dataPath, "Wave", name + ".json");
        

        // try
        // {
        //     string fileName = $"{StatsManager.Round}_Bejdżej_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.json";
        //    //string fileName = "Wave.json";
        //     string filePath = Path.Combine(Application.dataPath, "Wave", fileName); 
        //     Directory.CreateDirectory(Path.GetDirectoryName(filePath)); 

        //     File.WriteAllText(filePath, json);
        //     Debug.Log($"Dane zapisane w: {filePath}");
        // }
        // catch (IOException ex)
        // {
        //     Debug.LogError("Błąd zapisu pliku: " + ex.Message); 
        // }
        // catch (Exception ex)
        // {
        //     Debug.LogError("Błąd podczas zapisu danych: " + ex.Message); 
        // }
    }

    public void Clear()
    {
        Clear(false);
    }

    public void Clear(bool isEnemy)
    {
        FightManager fightManager = EventSystem.eventSystem.GetComponent<FightManager>();
        foreach (Linia linia in fightManager.linie)
        {
            if((linia.nr < 3 && !isEnemy) || (linia.nr >= 3 && isEnemy))
            {
                foreach (Pole pole in linia.pola)
                {
                    if (pole.unit!= null)
                    {
                        Destroy(pole.unit);
                    }
                    Destroy(pole.gameObject);
                }
                linia.pola = new List<Pole>();
            }
        }
    }

    public void LoadActive()
    {
        Clear();
        SaveManager.SaveData loadedData = SaveManager.Load();
        if (loadedData != null)
        {
            SaveManager.InstantiateUnits(loadedData);
        }
    }

    public void LoadActive(bool isEnemy)
    {
        Clear(isEnemy);
        SaveManager.SaveData loadedData = SaveManager.Load(isEnemy);
        if (loadedData != null)
        {
            SaveManager.InstantiateUnits(loadedData, isEnemy);
        }
    }

    public static SaveData Load()
    {
        return Load(false);
    }

    public static string findEnemy()
    {

        string folderPath = Application.dataPath + "/Wave";

        if (Directory.Exists(folderPath))
        {
            // Pobranie wszystkich plików .json w folderze
            string[] files = Directory.GetFiles(folderPath, "*.json");

            List<string> matchingFiles = new List<string>();

            foreach (string filePath in files)
            {
                // Pobranie nazwy pliku bez ścieżki i rozszerzenia
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string[] parts = fileName.Split('_');

                if (parts.Length > 0 && int.TryParse(parts[0], out int firstNumber))
                {
                    if (firstNumber == StatsManager.Round)
                    {
                        matchingFiles.Add(filePath);
                    }
                }
            }

            if (matchingFiles.Count > 0)
            {
                // Losowanie jednego z pasujących plików
                string selectedFile = matchingFiles[UnityEngine.Random.Range(0, matchingFiles.Count)];
                Debug.Log($"Wylosowano plik: {selectedFile}");
                return selectedFile;
            }
            else
            {
                Debug.LogWarning($"Brak plików .json zgodnych z numerem rundy ({StatsManager.Round}).");
            }
        }
        else
        {
            Debug.LogError($"Folder nie istnieje: {folderPath}");
        }
        return Application.dataPath + "/Wave/Wave1.json";
    }

    public static SaveData Load(bool isEnemy)
    {
        int modyfikator = 0;
        //customPath = @"D:\OneDrive\Pulpit\SzyrGameStudio\Do Boju!\Assets\Save\Zapis.json";
        customPath = Application.dataPath + "/Save/Zapis.json";

        if(isEnemy)
        {
            customPath = findEnemy();
            //customPath = Application.dataPath + "/Wave/Wave1.json";
            modyfikator = 3;
        }
        if (File.Exists(customPath))
        {
            string json = File.ReadAllText(customPath);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            if(isEnemy)
            {
                EventSystem.eventSystem.GetComponent<EnemyManager>().SetPlayer(data.playerName, data.number);
            }

            if(!isEnemy)
                ShopManager.levelUp = data.levelUp;
            for(int i = 0; i <= data.levelUp-1; i++)
            {
                for(int j = 0; j < 3; j++)
                    EventSystem.eventSystem.GetComponent<FightManager>().linie[j + modyfikator].Upgrade(i == 3 ? 1 : i);
            }

            return data;
        }
        else
        {
            return null;
        }
    }
    public static void InstantiateUnits(SaveData data)
    {
        InstantiateUnits(data, false);
    }

    public static void InstantiateUnits(SaveData data, bool isEnemy)
    {
        int modyfikator = 0;
        if (isEnemy)
        {
            modyfikator = 3;
        }
        if (data == null || data.units == null || data.units.Count == 0)
        {
            return;
        }

        FightManager fightManager = EventSystem.eventSystem.GetComponent<FightManager>();

        foreach (var unitData in data.units)
        {
            // Twórz obiekt jednostki
            GameObject unitObject = Instantiate(EventSystem.eventSystem.GetComponent<CharacterManager>().characters[unitData.Id]);

            // Ustaw pozycję jednostki na planszy
            Pole pole = fightManager.GetPole(unitData.Line + modyfikator, unitData.Pole);
            if (pole != null)
            {
                unitObject.transform.position = pole.transform.position;
                pole.unit = unitObject; // Powiąż jednostkę z polem
                pole.Start();
            }

            // Ustaw statystyki jednostki
            Unit unitComponent = unitObject.GetComponent<Unit>();
            if (unitComponent != null)
            {
                unitComponent.Id = unitData.Id;
                unitComponent.Name = unitData.Name;
                unitComponent.Cost = unitData.Cost;
                unitComponent.Initiative = unitData.Initiative;
                unitComponent.Health = unitData.Health;
                unitComponent.MaxHealth = unitData.MaxHealth;
                unitComponent.Attack = unitData.Attack;
                unitComponent.Defense = unitData.Defense;
                unitComponent.Range = unitData.Range;
                unitComponent.AP = unitData.AP;
                unitComponent.MagicResist = unitData.MagicResist;
                unitComponent.Description = unitData.Description;
                unitComponent.UpgradeLevel = unitData.UpgradeLevel;
                if(isEnemy)
                {
                    unitComponent.Enemy = true;
                    unitObject.GetComponent<SpriteRenderer>().flipX = !unitObject.GetComponent<SpriteRenderer>().flipX;
                }
            }
        }

        Debug.Log("Wszystkie jednostki zostały odtworzone.");
    }
}
