using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using System.Data.SqlClient;
using System.Linq; 

public class SaveManager : MonoBehaviour
{
    private static string customPath;


    public void Start()
    {
        if (PlayerManager.isSave)
        {  
            StartCoroutine(DelayedLoad2());
        }
    }

    private IEnumerator DelayedLoad2()
    {
        yield return new WaitForSeconds(0.5f);
        LoadActive();
        Load2();
    }


    [System.Serializable]
    public class SaveData
    {
        public string playerName;
        public int number;
        public int levelUp; // STARY format: wspólny poziom dla wszystkich 3 linii. Zostaje dla kompatybilności wstecznej.
        public int bohater;
        // NOWY format: osobny poziom (1..4) dla każdej z 3 linii gracza, w kolejności linie[0], linie[1], linie[2].
        // W starych zapisach ta lista nie istnieje w JSON-ie, więc po wczytaniu będzie null —
        // to jest sygnał, że trzeba użyć starego, wspólnego "levelUp".
        public List<int> lineLevels;
        public List<SaveUnit> units = new List<SaveUnit>();
    }
    [System.Serializable]
    public class SerializableKeyValuePair
    {
        public int Key;
        public int Value;
    }

    [System.Serializable]
    public class SaveData2
    {
        public int playerId;
        public int money;
        public int incom;
        public bool poddymka;
        public int win;
        public string fractions;
        public int lose;
        public int round;
        public List<int> shop;
        public string nextFight;
        public List<SaveUnit> units = new List<SaveUnit>();
        public int allRoll;
        public float time;
        public List<SerializableKeyValuePair> popular;
        public List<SerializableKeyValuePair> strong;
    }

    [System.Serializable]
    public class SaveUnit
    {
        public int Line;
        public int Pole;

        public int Id;
        //public string Name;
        public int Cost;
        public int RealCost;

        public int Initiative;
        public int MaxHealth;
        public int Health;
        public int Attack;
        public int Defense;
        public int Range;
        public int AP;
        public int MagicResist;
        //public string Description;

        public int UpgradeLevel;

        public int SpellId;
    }

    public static void Save(string playerName, int number, int levelUp, int bohater)
    {
        string savePath1 = Application.dataPath + "/Save/Zapis.json";
        if(Multi.multi)
        {
            savePath1 = Application.dataPath + "/Save/Zapis3.json";
        }
        if(PlayerManager.SI)
        {
            savePath1 = Application.dataPath + "/Save/Zapis5.json";
        }
        if(Tutorial.tutorial)
        {
            savePath1 = Application.dataPath + "/Save/Zapis4.json";
        }
        string savePath2 = Application.dataPath + "/Save/Save2.json";
        if(RankedManager.Ranked)
        {
            savePath1 = Application.dataPath + "/Save/ZapisR.json";
            savePath2 = Application.dataPath + "/Save/Save2R.json";
        }
        
        string directory = Path.GetDirectoryName(savePath1);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Tworzenie danych do pierwszego zapisu
        FightManager fightManagerDoZapisu = EventSystem.eventSystem.GetComponent<FightManager>();
        List<int> aktualnePoziomyLinii = new List<int>
        {
            fightManagerDoZapisu.linie[0].poziom,
            fightManagerDoZapisu.linie[1].poziom,
            fightManagerDoZapisu.linie[2].poziom
        };

        SaveData data1 = new SaveData
        {
            playerName = playerName,
            number = number,
            bohater = bohater,
            levelUp = levelUp, // zostaje dla starego kodu/kompatybilności (np. widok w bazie danych)
            lineLevels = aktualnePoziomyLinii
        };
        
        // Tworzenie danych do drugiego zapisu (losowe wartości)
        string frakcja = "";

        // Iteracja przez wszystkie wartości enuma
        if(Fraction.fractionList == null)
        {
            Fraction.fractionList = new List<Fraction.fractionType>();
        }
        foreach (Fraction.fractionType type in Enum.GetValues(typeof(Fraction.fractionType)))
        {
            if (Fraction.fractionList.Contains(type))
            {
                frakcja += "1";
            }
            else
            {
                frakcja += "0";
            }
        }

        System.Random rand = new System.Random();
        string zapisWalki = "";

        if (!Multi.multi)
        {
            var enemyManager = EventSystem.eventSystem.GetComponent<EnemyManager>();
            if (StatsManager.Round >= 0 && StatsManager.Round < enemyManager.Comps.Count)
                zapisWalki = EventSystem.eventSystem.GetComponent<EnemyManager>().Comps[StatsManager.Round];
        }
        else
            zapisWalki = " ";
            
        SaveData2 data2 = new SaveData2
        {
            playerId = PlayerManager.Id,
            money = MoneyManager.money,
            incom = MoneyManager.income,
            poddymka = RankedManager.Poddymka,
            fractions = frakcja,
            win = StatsManager.win,
            round = StatsManager.Round,
            lose = StatsManager.life,
            nextFight = zapisWalki,
            allRoll = ShopManager.allRoll,
            time = (float)PlayerManager.stopwatch.Elapsed.TotalSeconds,
            popular = PlayerManager.Popular.Select(kv => new SerializableKeyValuePair { Key = kv.Key, Value = kv.Value }).ToList(),
            strong = PlayerManager.Strong.Select(kv => new SerializableKeyValuePair { Key = kv.Key, Value = kv.Value }).ToList(),
            shop = new List<int>(),
        };
        for(int i = 0; i < 5; i++)
        {
            if(EventSystem.eventSystem.GetComponent<ShopManager>().character[i].unit != null)
                data2.shop.Add(EventSystem.eventSystem.GetComponent<ShopManager>().character[i].unit.GetComponent<Unit>().Id);
            else
                data2.shop.Add(-1);
        }
        
        // Pobieranie jednostek
        FightManager fightManager = EventSystem.eventSystem.GetComponent<FightManager>();
        foreach (Linia linia in fightManager.linie)
        {
            if (linia.nr < 3)
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
                            //Name = unit.Name,
                            Cost = unit.Cost,
                            RealCost = unit.RealCost,
                            Initiative = unit.Initiative,
                            MaxHealth = unit.MaxHealth,
                            Health = unit.Health,
                            Attack = unit.Attack,
                            Defense = unit.Defense,
                            Range = unit.Range,
                            AP = unit.AP,
                            MagicResist = unit.MagicResist,
                            //Description = unit.Description,
                            UpgradeLevel = unit.UpgradeLevel
                        };

                        Wizard wizard = unit.gameObject.GetComponent<Wizard>();
                        if (wizard != null)
                        {
                            unitData.SpellId = wizard.spell.Id;
                        }
                        
                        data1.units.Add(unitData);
                        //data2.units.Add(unitData);
                    }
                }
            }
        }
        
        foreach (Pole pole in EventSystem.eventSystem.GetComponent<ShopManager>().lawka)
        {
            if (pole.unit != null)
            {
                Unit unit = pole.unit.GetComponent<Unit>();
                SaveUnit unitData = new SaveUnit
                {
                    Line = -1,
                    Pole = pole.nr,
                    Id = unit.Id,
                    //Name = unit.Name,
                    Cost = unit.Cost,
                    RealCost = unit.RealCost,
                    Initiative = unit.Initiative,
                    MaxHealth = unit.MaxHealth,
                    Health = unit.Health,
                    Attack = unit.Attack,
                    Defense = unit.Defense,
                    Range = unit.Range,
                    AP = unit.AP,
                    MagicResist = unit.MagicResist,
                    //Description = unit.Description,
                    UpgradeLevel = unit.UpgradeLevel
                };

                Wizard wizard = unit.gameObject.GetComponent<Wizard>();
                if (wizard != null)
                {
                    unitData.SpellId = wizard.spell.Id;
                }
                
                //data1.units.Add(unitData);
                data2.units.Add(unitData);
            }
        }
    

        File.WriteAllText(savePath1, JsonUtility.ToJson(data1, true));
        if(!Multi.multi && !Tutorial.tutorial && !PlayerManager.SI)
            File.WriteAllText(savePath2, JsonUtility.ToJson(data2, true));

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
                // ResetLinia() niszczy starą drogę i jednostki na niej ORAZ resetuje "poziom" na
                // Linii — bez tego SetLevel() przy wczytywaniu zapisu widziałby "już jestem na
                // docelowym poziomie" i nigdy nie odpalałby SpawnRoad(), zostawiając puste pola.
                linia.ResetLinia();
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
    //TEST
        // List<FightManager.AutoBattlerGameViewModel> lista = FightManager.LoadFeaturedGamesFromDatabase();

        // var filteredList = lista
        //     .Where(game => game.Round == StatsManager.Round) 
        //     .Where(game => game.Name != PlayerManager.Name) 
        //     .Where(game => game.PlayerId != PlayerManager.Id) 
        //     .Where(game => game.Version == PlayerManager.Version)
        //     .ToList();

        // var randomGame = filteredList[UnityEngine.Random.Range(0, filteredList.Count)];
        
        
            SaveManager.SaveData loadedData = SaveManager.Load(isEnemy, EventSystem.eventSystem.GetComponent<EnemyManager>().Comps[StatsManager.Round]);

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

    public static SaveData Load(bool isEnemy, string jsonPath)
    {
        string json = jsonPath;

        SaveData data = JsonUtility.FromJson<SaveData>(json);

        if(isEnemy && !Tutorial.tutorial)
        {
            if (data != null)
            {
                int newLP = EventSystem.eventSystem.GetComponent<EnemyManager>().LP[StatsManager.Round];
                EventSystem.eventSystem.GetComponent<EnemyManager>().SetPlayer(data.playerName, data.number, newLP, data.bohater);
            }
        }

        if(!isEnemy)
            ShopManager.levelUp = data.levelUp;

        if (data.lineLevels != null && data.lineLevels.Count == 3)
        {
            // NOWY format zapisu: każda linia ma swój własny poziom.
            for (int j = 0; j < 3; j++)
                EventSystem.eventSystem.GetComponent<FightManager>().linie[j + 3].SetLevel(data.lineLevels[j]);
        }
        else
        {
            // STARY format zapisu (brak lineLevels) — jak dawniej, ulepszamy wszystkie 3 linie tak samo.
            for(int i = 0; i <= data.levelUp-1; i++)
            {
                for(int j = 0; j < 3; j++)
                    EventSystem.eventSystem.GetComponent<FightManager>().linie[j + 3].Upgrade(i == 3 ? 1 : i);
            }
            for (int j = 0; j < 3; j++)
            {
                Linia linia = EventSystem.eventSystem.GetComponent<FightManager>().linie[j + 3];
                linia.poziom = Mathf.Clamp(data.levelUp, 1, linia.roadPrefabs.Length);
            }
        }

        return data;
    }

    public static SaveData Load(bool isEnemy)
    {
        int modyfikator = 0;
        customPath = Application.dataPath + "/Save/Zapis.json";
        if(Multi.multi)
        {
            customPath = Application.dataPath + "/Save/Zapis3.json";
        }
        if(PlayerManager.SI)
        {
            customPath = Application.dataPath + "/Save/Zapis5.json";
        }
        if(Tutorial.tutorial)
        {
            customPath = Application.dataPath + "/Save/Zapis4.json";
        }
        if(Tutorial.tutorial)
        {
            customPath = Application.dataPath + "/Save/Zapis4.json";
        }
        if(RankedManager.Ranked)
        {
            customPath = Application.dataPath + "/Save/ZapisR.json";
        }

        if(isEnemy)
        {
            customPath = findEnemy();
            modyfikator = 3;
        }
        if (File.Exists(customPath))
        {
            string json = File.ReadAllText(customPath);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            if(isEnemy)
            {
                int newLP = EventSystem.eventSystem.GetComponent<EnemyManager>().LP[StatsManager.Round];
                EventSystem.eventSystem.GetComponent<EnemyManager>().SetPlayer(data.playerName, data.number, newLP, data.bohater);
            }

            if(!isEnemy)
                ShopManager.levelUp = data.levelUp;

            if (data.lineLevels != null && data.lineLevels.Count == 3)
            {
                // NOWY format zapisu: każda linia ma swój własny poziom.
                for (int j = 0; j < 3; j++)
                    EventSystem.eventSystem.GetComponent<FightManager>().linie[j + modyfikator].SetLevel(data.lineLevels[j]);
            }
            else
            {
                // STARY format zapisu (brak lineLevels) — jak dawniej, ulepszamy wszystkie 3 linie tak samo.
                for(int i = 0; i <= data.levelUp-1; i++)
                {
                    for(int j = 0; j < 3; j++)
                        EventSystem.eventSystem.GetComponent<FightManager>().linie[j + modyfikator].Upgrade(i == 3 ? 1 : i);
                }
                // Synchronizacja nowego pola "poziom" z odtworzonym stanem, żeby dalsze
                // (osobne) ulepszanie tej linii liczyło koszty od właściwego miejsca.
                for (int j = 0; j < 3; j++)
                {
                    Linia linia = EventSystem.eventSystem.GetComponent<FightManager>().linie[j + modyfikator];
                    linia.poziom = Mathf.Clamp(data.levelUp, 1, linia.roadPrefabs.Length);
                }
            }

            return data;
        }
        else
        {
            return null;
        }
    }

    public static SaveData2 Load2()
    {
        string savePath2 = Application.dataPath + "/Save/Save2.json";
        if(RankedManager.Ranked)
        {
            savePath2 = Application.dataPath + "/Save/Save2R.json";
        }
        if (File.Exists(savePath2))
        {
            string json = File.ReadAllText(savePath2);
            SaveData2 data = JsonUtility.FromJson<SaveData2>(json);
            
            StatsManager.life = data.lose;
            StatsManager.Round = data.round;
            StatsManager.win = data.win;
            MoneyManager.income = data.incom;
            RankedManager.Poddymka = data.poddymka;
            MoneyManager.money = data.money;
            PlayerManager.Id = data.playerId;
            EventSystem.eventSystem.GetComponent<EnemyManager>().Comps[data.round] = data.nextFight;

            ShopManager.nizka = data.allRoll;
            PlayerManager.Time = data.time;
            PlayerManager.Popular = data.popular.ToDictionary(x => x.Key, x => x.Value);
            PlayerManager.Strong = data.strong.ToDictionary(x => x.Key, x => x.Value);

            for (int i = 0; i < 5; i++)
            {
                EventSystem.eventSystem.GetComponent<ShopManager>().character[i].unit = EventSystem.eventSystem.GetComponent<CharacterManager>().characters[data.shop[i]];
                EventSystem.eventSystem.GetComponent<ShopManager>().character[i].SetLook();
            }
            List<GameObject> pomoc = new List<GameObject>();

            string frakcja = data.fractions;
            for (int i = 0; i < frakcja.Length; i++)
            {
                // Pobierz wartość enuma na podstawie indeksu
                Fraction.fractionType type = (Fraction.fractionType)i;

                // Jeśli znak to '1', dodaj frakcję do listy
                if (frakcja[i] == '1')
                {
                    if (!Fraction.fractionList.Contains(type))
                    {
                        Fraction.fractionList.Add(type);
                    }
                }
                else
                {
                    if (Fraction.fractionList.Contains(type))
                    {
                        Fraction.fractionList.Remove(type);
                    }
                }
            }
            
            InstantiateShopUnits(data);
            
            
            return data;
        }
        else
        {
            Debug.LogWarning("Plik Save2.json nie istnieje.");
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
                //unitComponent.Name = unitData.Name;
                unitComponent.Cost = unitData.Cost;
                unitComponent.RealCost = unitData.RealCost;
                unitComponent.Initiative = unitData.Initiative;
                unitComponent.Health = unitData.Health;
                unitComponent.MaxHealth = unitData.MaxHealth;
                unitComponent.Attack = unitData.Attack;
                unitComponent.Defense = unitData.Defense;
                unitComponent.Range = unitData.Range;
                unitComponent.AP = unitData.AP;
                unitComponent.MagicResist = unitData.MagicResist;
                //unitComponent.Description = unitData.Description;
                unitComponent.UpgradeLevel = unitData.UpgradeLevel;
                if(isEnemy)
                {
                    unitComponent.Enemy = true;
                    unitObject.GetComponent<SpriteRenderer>().flipX = !unitObject.GetComponent<SpriteRenderer>().flipX;
                }
                Wizard wizzard = unitObject.GetComponent<Wizard>();
                if(wizzard != null)
                {
                    wizzard.AddSpell(unitData.SpellId);
                }
            }
        }
    }
    public static void InstantiateShopUnits(SaveData2 data)
    {
        if (data == null || data.units == null || data.units.Count == 0)
        {
            return;
        }
        
        ShopManager shopManager = EventSystem.eventSystem.GetComponent<ShopManager>();
        
        foreach (var unitData in data.units)
        {
            GameObject unitObject = Instantiate(EventSystem.eventSystem.GetComponent<CharacterManager>().characters[unitData.Id]);
            
            shopManager.lawka[unitData.Pole].unit = unitObject ; // Dodaj jednostkę do ławeczki
            shopManager.lawka[unitData.Pole].Start(); // Uruchom jednostkę

            Unit unitComponent = unitObject.GetComponent<Unit>();
            if (unitComponent != null)
            {
                unitComponent.Id = unitData.Id;
                //unitComponent.Name = unitData.Name;
                unitComponent.Cost = unitData.Cost;
                unitComponent.RealCost = unitData.RealCost;
                unitComponent.Initiative = unitData.Initiative;
                unitComponent.Health = unitData.Health;
                unitComponent.MaxHealth = unitData.MaxHealth;
                unitComponent.Attack = unitData.Attack;
                unitComponent.Defense = unitData.Defense;
                unitComponent.Range = unitData.Range;
                unitComponent.AP = unitData.AP;
                unitComponent.MagicResist = unitData.MagicResist;
                //unitComponent.Description = unitData.Description;
                unitComponent.UpgradeLevel = unitData.UpgradeLevel;
            }
        }
    }

}