using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>Stan trybu fabularnego. Statyczny, żeby przetrwał zmianę sceny (menu misji → Main).</summary>
public static class StoryManager
{
    public static bool Active;
    public static MissionData Mission;

    /// <summary>1 = wygrana, 0 = remis, -1 = przegrana. Ustawiane w FightManager po każdej walce.</summary>
    public static int LastResult;

    /// <summary>Dla której rundy pokazano już dialog "przed walką" (zapisywane, żeby nie powtarzać po wczytaniu).</summary>
    public static int DialogueShownRound = -1;

    static FightManager FM => EventSystem.eventSystem.GetComponent<FightManager>();

    // =====================================================================
    //  START MISJI
    // =====================================================================

    /// <summary>Wołane z menu misji tuż przed załadowaniem sceny gry.</summary>
    public static void Begin(MissionData mission, bool continueSave)
    {
        Active = true;
        Mission = mission;
        LastResult = 0;
        DialogueShownRound = -1;

        RankedManager.Ranked = false;
        Multi.multi = false;
        Tutorial.tutorial = false;
        PlayerManager.SI = false;
        PlayerManager.isSave = continueSave && File.Exists(SavePaths.RunFor("S"));

        // Frakcje = te, z których pochodzi pula misji (sklep i tak filtruje po puli).
        Fraction.fractionList = mission.playerPool
            .Where(go => go != null && go.GetComponent<Unit>() != null)
            .Select(go => go.GetComponent<Unit>().fraction)
            .Distinct().ToList();
    }

    /// <summary>Wołane w SaveManager.Start() przy NOWEJ misji (nie przy wczytaniu).</summary>
    public static void ApplyMissionStart()
    {
        MoneyManager.money = Mission.startMoney;
        MoneyManager.income = Mission.startIncome;
        StatsManager.life = Mission.lives;
        StatsManager.Round = 0;
        StatsManager.win = 0;

        if (Mission.fixedHero != null)
        {
            HeroState.Id = Mission.fixedHero.bohaterId;
            HeroState.Level = Mission.fixedHeroLevel;
            for (int a = 1; a <= HeroState.Level; a++)
                HeroAbilities.OnUnlocked(a);
            HeroAbilities.OnShopPhase(0);
        }
    }

    public static MissionWave Wave(int index)
    {
        if (Mission == null || index < 0 || index >= Mission.waves.Count) return null;
        return Mission.waves[index];
    }

    public static bool IsMissionWon => Mission != null && StatsManager.Round >= Mission.waves.Count;

    // =====================================================================
    //  PRZECIWNICY
    // =====================================================================

    /// <summary>Lista składów dla EnemyManager.Comps – jeden na falę, już z wymieszanymi rzędami.</summary>
    public static List<string> BuildComps()
    {
        var list = new List<string>();
        for (int i = 0; i < Mission.waves.Count; i++)
            list.Add(PreparedComp(i));
        return list;
    }

    /// <summary>Skład fali: nazwa i twarz z misji, rzędy wymieszane (jeśli fala tak chce).</summary>
    public static string PreparedComp(int waveIndex)
    {
        MissionWave w = Wave(waveIndex);
        if (w == null || w.comp == null) return EnemyManager.Zapasowy;

        SaveManager.SaveData data = SaveCodec.Parse(w.comp.text);
        if (data == null) { Debug.LogWarning($"Misja {Mission.missionId}: nie da się odczytać składu fali {waveIndex}"); return EnemyManager.Zapasowy; }

        data.playerName = w.enemyName;
        data.number = w.enemyFace;
        if (w.shuffleRows) ShuffleRows(data);
        return SaveCodec.Encode(data);
    }

    /// <summary>Losowa permutacja rzędów 0–2: jednostki i poziomy rzędów przenoszą się razem.</summary>
    public static void ShuffleRows(SaveManager.SaveData data)
    {
        int[] perm = { 0, 1, 2 };
        for (int i = 2; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            int t = perm[i]; perm[i] = perm[j]; perm[j] = t;
        }

        foreach (var u in data.units)
            if (u.Line >= 0 && u.Line <= 2) u.Line = perm[u.Line];

        if (data.lineLevels != null && data.lineLevels.Count == 3)
        {
            var levels = new List<int> { 0, 0, 0 };
            for (int i = 0; i < 3; i++) levels[perm[i]] = data.lineLevels[i];
            data.lineLevels = levels;
        }
    }

    /// <summary>Wołane w FightManager.sprawdzKtoWygralWParach() w każdej z trzech gałęzi.</summary>
    public static void OnBattleResult(int result)
    {
        if (!Active) return;
        LastResult = result;
        if (result <= 0)
        {
            // Przegrana albo remis: ta sama fala jeszcze raz, z nowym losowaniem rzędów.
            StatsManager.Round--;
            List<string> comps = EventSystem.eventSystem.GetComponent<EnemyManager>().Comps;
            if (StatsManager.Round >= 0 && StatsManager.Round < comps.Count)
                comps[StatsManager.Round] = PreparedComp(StatsManager.Round);
        }
    }

    // =====================================================================
    //  SKLEP
    // =====================================================================

    /// <summary>Zastępuje filtr sklepu: tylko pula misji (+ opcjonalnie bez limitu gwiazdek).</summary>
    public static List<GameObject> FilterShop(List<GameObject> all)
    {
        int maxStar = StatsManager.Round / 3 + 1;
        return all.Where(go =>
        {
            if (!Mission.playerPool.Contains(go)) return false;
            Unit u = go.GetComponent<Unit>();
            Heros h = go.GetComponent<Heros>();
            if (u == null || u.Star == 0 || (h != null && h.Evolution)) return false;
            return Mission.ignoreStarLimit || u.Star <= maxStar;
        }).ToList();
    }

    // =====================================================================
    //  TRWAŁA ŚMIERĆ – kluczowa mechanika trybu
    // =====================================================================

    /// <summary>
    /// Po walce usuwa z zapisu planszy jednostki gracza, których już nie ma na polu bitwy.
    /// Ocalałe wracają ze statystykami sprzed walki, czyli z odnowionym HP.
    /// Wołane w FightManager.Battle() tuż przed LoadActive().
    /// </summary>
    public static void ApplyPermadeath()
    {
        if (!Active || !File.Exists(SavePaths.Board)) return;

        var alive = new HashSet<int>();
        foreach (Linia linia in FM.linie)
        {
            foreach (Pole pole in linia.pola)
            {
                if (pole == null || pole.unit == null) continue;
                Unit u = pole.unit.GetComponent<Unit>();
                if (u != null && !u.Enemy && u.Uid != 0) alive.Add(u.Uid);
            }
        }

        SaveManager.SaveData board = SaveService.ParseBoard(File.ReadAllText(SavePaths.Board));
        if (board == null) return;
        int removed = board.units.RemoveAll(d => d.Uid != 0 && !alive.Contains(d.Uid));
        if (removed > 0)
        {
            File.WriteAllText(SavePaths.Board, JsonUtility.ToJson(board, true));
            Debug.Log($"Fabuła: {removed} jednostek poległo na zawsze.");
        }
    }

    // =====================================================================
    //  POSTĘP
    // =====================================================================

    public static void MarkCompleted()
    {
        PlayerPrefs.SetInt("story_done_" + Mission.missionId, 1);
        PlayerPrefs.Save();
    }

    public static bool IsCompleted(MissionData m) => m != null && PlayerPrefs.GetInt("story_done_" + m.missionId, 0) == 1;

    /// <summary>Id misji z zapisu do kontynuacji albo null.</summary>
    public static string SavedMissionId()
    {
        string path = SavePaths.RunFor("S");
        if (!File.Exists(path)) return null;
        try { return JsonUtility.FromJson<SaveManager.SaveData2>(File.ReadAllText(path)).storyMission; }
        catch { return null; }
    }
}
