using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [Serializable]
    public class SaveData
    {
        public string playerName;
        public int number;
        public int levelUp;            // STARY format – tylko do odczytu starych zapisów
        public int bohater;
        public int bohaterLevel;       // NOWE (sekcja 4): ile zdolności odblokowano, 0 = brak bohatera
        public List<int> lineLevels;   // NOWY format – poziom każdego z 3 rzędów
        public List<SaveUnit> units = new List<SaveUnit>();
    }

    [Serializable]
    public class SerializableKeyValuePair
    {
        public int Key;
        public int Value;
    }

    [Serializable]
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
        public int levelDiscount;      // NOWE: ShopManager.nizka (Mroczna chata)
        public bool heroSaved;         // NOWE (sekcja 4): false w starych zapisach
        public int heroRerolls;
        public int heroLastShopRound;
        public List<int> heroOffer;
        public string storyMission;    // NOWE (sekcja 5): id misji, pusty poza trybem fabularnym
        public int storyDialogueRound;
        public float time;
        public List<SerializableKeyValuePair> popular;
        public List<SerializableKeyValuePair> strong;
    }

    [Serializable]
    public class SaveUnit
    {
        public int Line;
        public int Pole;
        public int Id;
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
        public int UpgradeLevel;
        public int SpellId;
        public bool Spell;             // NOWE: true = Id wskazuje na CharacterManager.Spells, nie characters
        public int Uid;                // NOWE (sekcja 5): stały numer jednostki gracza – do trwałej śmierci
    }

    // Jedyny punkt startowy wczytywania sceny. Zastępuje DelayedLoad2() i FightManager.Sztart().
    IEnumerator Start()
    {
        SaveService.Ready = false;
        HeroState.Reset();
        SaveService.NextUid = 1;
        yield return null; // jedna klatka: wszystkie Start() (Linia, Shop, Money, Stats...) już się wykonały

        if (PlayerManager.isSave)
        {
            SaveService.LoadRun();
            SaveData board = SaveService.LoadPlayerBoard();
            if (board != null && board.bohaterLevel > 0) // stare zapisy: bohater bez zdolności = wybierz od nowa
            {
                HeroState.Id = board.bohater;
                HeroState.Level = board.bohaterLevel;
            }
        }
        else
        {
            SaveService.ApplyLineLevels(null, false); // świeża gra: wszystkie rzędy na poziomie 1
            if (StoryManager.Active)
                StoryManager.ApplyMissionStart(); // złoto, życia, bohater z misji
        }

        SaveService.Ready = true;
        if (BohaterManager.Instance != null)
            BohaterManager.Instance.ShowStateAfterLoad(); // pokaż bohatera albo wybór
        SaveService.SaveAll();
        LoadActive(true); // pokaż najbliższego przeciwnika
    }

    // ---- Stare API – zostaje, żeby nie trzeba było zmieniać wszystkich wywołań naraz ----

    public static void Save(string playerName, int number, int levelUp, int bohater) => SaveService.SaveAll();
    public static void Save() => SaveService.SaveAll();

    public void LoadActive() => SaveService.LoadPlayerBoard();

    public void LoadActive(bool isEnemy)
    {
        if (!isEnemy) { SaveService.LoadPlayerBoard(); return; }
        List<string> comps = EventSystem.eventSystem.GetComponent<EnemyManager>().Comps;
        int r = StatsManager.Round;
        SaveService.LoadEnemyBoard(r >= 0 && r < comps.Count ? comps[r] : null);
    }
}
