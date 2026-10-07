using System.Collections.Generic;

/// <summary>Numery bohaterów = pole bohaterId w plikach BohaterData. 1–3 zostają jak były.</summary>
public static class HeroIds
{
    public const int Brak = 0;
    // Ludzie
    public const int Gogol = 1, Arcymag = 2, BestiaMaster = 3;
    // Nekro
    public const int SzczuronMichael = 4, WampirzyZiomek = 5, Szkieletor = 6;
    // Nomadzi
    public const int Skoczek = 7, Magik = 8, Gambler = 9;
    // Krasnoludy
    public const int Kopacz = 10, MlotMistrz = 11, Tarczownik = 12;
    // Elfy
    public const int Drewniak = 13, Lucznik = 14, Saperito = 15;
}

/// <summary>Stan bohatera w bieżącej rozgrywce. Zapisywany przez SaveService.</summary>
public static class HeroState
{
    /// <summary>AbilityCost[n] = koszt odblokowania zdolności nr n. Pierwsza jest darmowa.</summary>
    public static readonly int[] AbilityCost = { 0, 0, 6, 10 };

    public static int Id;                  // 0 = jeszcze nie wybrano
    public static int Level;               // ile zdolności odblokowano (0..3)
    public static int EnemyId, EnemyLevel; // bohater przeciwnika (z jego składu)
    public static int RerollsLeft = -1;    // -1 = jeszcze nie ustalono
    public static int LastShopRound = -1;  // za którą rundę rozdano już prezenty
    public static List<int> Offer = new List<int>(); // dwie pokazane karty (żeby restart nie dawał darmowego losowania)
    public static bool Picking;            // trwa wybór – blokuje walkę

    public static bool Has(int heroId, int ability, bool enemy)
    {
        return enemy ? (EnemyId == heroId && EnemyLevel >= ability)
                     : (Id == heroId && Level >= ability);
    }

    /// <summary>Koszt następnej zdolności albo -1, gdy nie ma czego kupić.</summary>
    public static int NextCost => (Id != 0 && Level >= 1 && Level < 3) ? AbilityCost[Level + 1] : -1;

    public static void Reset()
    {
        Id = Level = EnemyId = EnemyLevel = 0;
        RerollsLeft = -1;
        LastShopRound = -1;
        Offer = new List<int>();
        Picking = false;
    }
}
