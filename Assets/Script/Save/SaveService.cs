using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// JEDYNE miejsce, które wie, gdzie leżą pliki zapisu dla danego trybu gry.
/// Board = plansza (Zapis*.json) – to, co trafia do bazy jako "duch".
/// Run   = stan rozgrywki (Save2*.json) – pieniądze, ławka, sklep, runda.
/// </summary>
public static class SavePaths
{
    public static string Root => Application.dataPath + "/Save";

    // Kolejność ma znaczenie i odpowiada staremu kodowi (Ranked nadpisywał wszystko).
    public static string Suffix
    {
        get
        {
            if (StoryManager.Active) return "S";
            if (RankedManager.Ranked) return "R";
            if (Tutorial.tutorial) return "4";
            if (PlayerManager.SI) return "5";
            if (Multi.multi) return "3";
            return "";
        }
    }

    public static string Board => BoardFor(Suffix);
    public static string Run => RunFor(Suffix);

    public static string BoardFor(string suffix) => $"{Root}/Zapis{suffix}.json";
    public static string RunFor(string suffix) => $"{Root}/Save2{suffix}.json";

    // Stan rozgrywki zapisujemy tylko w trybach, które da się wznowić.
    public static bool HasRunFile => StoryManager.Active || (!Multi.multi && !Tutorial.tutorial && !PlayerManager.SI);
}

/// <summary>
/// Cała logika zapisu i wczytywania. SaveManager zostaje tylko jako "kontener" na klasy danych
/// i cienkie wrappery, żeby stare wywołania dalej się kompilowały.
/// </summary>
public static class SaveService
{
    public const int EnemyOffset = 3;

    /// <summary>true dopiero po zakończeniu wczytywania sceny – do tego czasu nie startujemy walki.</summary>
    public static bool Ready;

    /// <summary>Następny wolny Uid jednostki gracza (sekcja 5).</summary>
    public static int NextUid = 1;

    static FightManager FM => EventSystem.eventSystem.GetComponent<FightManager>();
    static ShopManager Shop => EventSystem.eventSystem.GetComponent<ShopManager>();
    static CharacterManager Chars => EventSystem.eventSystem.GetComponent<CharacterManager>();
    static EnemyManager Enemies => EventSystem.eventSystem.GetComponent<EnemyManager>();

    // =====================================================================
    //  ZAPIS
    // =====================================================================

    public static void SaveAll()
    {
        if (!Ready) return; // nie nadpisuj zapisu, zanim go wczytamy

        Directory.CreateDirectory(SavePaths.Root);
        File.WriteAllText(SavePaths.Board, JsonUtility.ToJson(CaptureBoard(), true));

        if (SavePaths.HasRunFile)
            File.WriteAllText(SavePaths.Run, JsonUtility.ToJson(CaptureRun(), true));
    }

    public static SaveManager.SaveData CaptureBoard()
    {
        var data = new SaveManager.SaveData
        {
            playerName = PlayerManager.Name,
            number = PlayerManager.PlayerFaceId,
            bohater = HeroState.Id,
            bohaterLevel = HeroState.Level,
            lineLevels = new List<int>(),
        };

        for (int i = 0; i < 3; i++)
        {
            Linia linia = FM.linie[i];
            data.lineLevels.Add(linia.poziom);
            foreach (Pole pole in linia.pola)
            {
                if (pole != null && pole.unit != null)
                    data.units.Add(Snapshot(pole.unit.GetComponent<Unit>(), i, pole.nr));
            }
        }

        // Stare klienty czytają tylko levelUp – dajemy najwyższy poziom rzędu.
        data.levelUp = data.lineLevels.Max();
        return data;
    }

    public static SaveManager.SaveData2 CaptureRun()
    {
        var data = new SaveManager.SaveData2
        {
            playerId = PlayerManager.Id,
            money = MoneyManager.money,
            incom = MoneyManager.income,
            poddymka = RankedManager.Poddymka,
            fractions = FractionsToString(),
            win = StatsManager.win,
            round = StatsManager.Round,
            lose = StatsManager.life,
            nextFight = CurrentEnemyComp(),
            allRoll = ShopManager.allRoll,
            levelDiscount = ShopManager.nizka,
            heroSaved = true,
            heroRerolls = HeroState.RerollsLeft,
            heroLastShopRound = HeroState.LastShopRound,
            heroOffer = new List<int>(HeroState.Offer),
            storyMission = StoryManager.Active ? StoryManager.Mission.missionId : "",
            storyDialogueRound = StoryManager.DialogueShownRound,
            time = (float)PlayerManager.stopwatch.Elapsed.TotalSeconds,
            popular = PlayerManager.Popular.Select(kv => new SaveManager.SerializableKeyValuePair { Key = kv.Key, Value = kv.Value }).ToList(),
            strong = PlayerManager.Strong.Select(kv => new SaveManager.SerializableKeyValuePair { Key = kv.Key, Value = kv.Value }).ToList(),
            shop = new List<int>(),
        };

        foreach (ShopObject slot in Shop.character)
        {
            Unit u = slot.unit != null && slot.unit != slot.nullObject ? slot.unit.GetComponent<Unit>() : null;
            data.shop.Add(u != null ? u.Id : -1);
        }

        foreach (Pole pole in Shop.lawka)
        {
            if (pole.unit != null)
                data.units.Add(Snapshot(pole.unit.GetComponent<Unit>(), -1, pole.nr));
        }
        return data;
    }

    static string CurrentEnemyComp()
    {
        if (Multi.multi) return " ";
        var comps = Enemies.Comps;
        int r = StatsManager.Round;
        return (r >= 0 && r < comps.Count) ? comps[r] : "";
    }

    // =====================================================================
    //  JEDNOSTKA <-> DANE (jedna funkcja w każdą stronę zamiast 4 kopii)
    // =====================================================================

    public static SaveManager.SaveUnit Snapshot(Unit unit, int line, int pole)
    {
        if (unit.Uid == 0) unit.Uid = NextUid++; // każda jednostka gracza dostaje stały numer

        var d = new SaveManager.SaveUnit
        {
            Uid = unit.Uid,
            Line = line,
            Pole = pole,
            Id = unit.Id,
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
            UpgradeLevel = unit.UpgradeLevel,
            Spell = unit.GetComponent<Spell>() != null,
        };
        Wizard wizard = unit.GetComponent<Wizard>();
        if (wizard != null && wizard.spell != null)
            d.SpellId = wizard.spell.Id;
        return d;
    }

    public static void Apply(SaveManager.SaveUnit d, Unit unit)
    {
        unit.Id = d.Id;
        unit.Cost = d.Cost;
        unit.RealCost = d.RealCost;
        unit.Initiative = d.Initiative;
        unit.MaxHealth = d.MaxHealth;
        unit.Health = d.Health;
        unit.Attack = d.Attack;
        unit.Defense = d.Defense;
        unit.Range = d.Range;
        unit.AP = d.AP;
        unit.MagicResist = d.MagicResist;
        unit.UpgradeLevel = d.UpgradeLevel;
        unit.Uid = d.Uid;
        if (d.Uid >= NextUid) NextUid = d.Uid + 1;

        Wizard wizard = unit.GetComponent<Wizard>();
        if (wizard != null)
            wizard.AddSpell(d.SpellId);
    }

    // =====================================================================
    //  WCZYTYWANIE
    // =====================================================================

    /// <summary>Tekst (JSON albo nowy format) -> dane. Null, jeśli nie da się odczytać.</summary>
    public static SaveManager.SaveData ParseBoard(string text)
    {
        return SaveCodec.Parse(text); // stary JSON albo nowy format "~2..."
    }

    /// <summary>Plansza gracza z pliku (zamiast LoadActive()).</summary>
    public static SaveManager.SaveData LoadPlayerBoard()
    {
        ClearSide(false);
        SaveManager.SaveData data = File.Exists(SavePaths.Board) ? ParseBoard(File.ReadAllText(SavePaths.Board)) : null;
        ApplyLineLevels(data, false);
        if (data != null)
            SpawnUnits(data.units, false);
        FM.setList();
        return data;
    }

    /// <summary>Plansza przeciwnika z tekstu składu (zamiast LoadActive(true)).</summary>
    public static void LoadEnemyBoard(string comp)
    {
        ClearSide(true);
        SaveManager.SaveData data = ParseBoard(comp) ?? ParseBoard(EnemyManager.Zapasowy);

        if (data != null && !Tutorial.tutorial)
        {
            int r = StatsManager.Round;
            int lp = (r >= 0 && r < Enemies.LP.Count) ? Enemies.LP[r] : 1000;
            Enemies.SetPlayer(data.playerName, data.number, lp, data.bohater);
        }

        HeroState.EnemyId = (data != null && data.bohaterLevel > 0) ? data.bohater : HeroIds.Brak;
        HeroState.EnemyLevel = data != null ? data.bohaterLevel : 0;

        ApplyLineLevels(data, true);
        if (data != null)
            SpawnUnits(data.units, true);
        FM.setList();
    }

    public static void ClearSide(bool enemy)
    {
        int off = enemy ? EnemyOffset : 0;
        for (int i = 0; i < 3; i++)
            FM.linie[i + off].ResetLinia();
    }

    /// <summary>Nowy zapis: lineLevels. Stary zapis: levelUp N = poziom N w każdym rzędzie.</summary>
    public static void ApplyLineLevels(SaveManager.SaveData data, bool enemy)
    {
        int off = enemy ? EnemyOffset : 0;
        for (int j = 0; j < 3; j++)
        {
            int level = 1;
            if (data != null)
            {
                if (data.lineLevels != null && data.lineLevels.Count == 3)
                    level = data.lineLevels[j];
                else
                    level = Mathf.Max(1, data.levelUp);
            }
            FM.linie[j + off].SetLevel(level);
        }
    }

    public static void SpawnUnits(List<SaveManager.SaveUnit> units, bool enemy)
    {
        if (units == null) return;
        int off = enemy ? EnemyOffset : 0;

        foreach (var d in units)
        {
            if (d.Id < 0 || d.Id >= Chars.characters.Count || d.Line < 0 || d.Line > 2)
            {
                Debug.LogWarning($"Pomijam jednostkę z zapisu: Id={d.Id}, Line={d.Line}");
                continue;
            }

            GameObject prefab = Chars.characters[d.Id];
            Pole pole = FindSafePole(FM.linie[d.Line + off], d.Pole, prefab);

            if (pole == null)
            {
                // Nigdy nie zostawiamy jednostki "w powietrzu".
                if (enemy) { Debug.LogWarning($"Brak miejsca dla wroga Id={d.Id} – pomijam."); continue; }
                GameObject onBench = SpawnOnBench(prefab, -1);
                if (onBench != null) Apply(d, onBench.GetComponent<Unit>());
                else Refund(d);
                continue;
            }

            GameObject go = UnityEngine.Object.Instantiate(prefab);
            Unit unit = go.GetComponent<Unit>();
            Apply(d, unit);
            if (enemy)
            {
                unit.Enemy = true;
                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                sr.flipX = !sr.flipX;
            }
            pole.unit = go;
            pole.Start(); // ustawia DragObject.pole i pozycję
        }
    }

    /// <summary>Pole o zadanym numerze, a jeśli go nie ma / jest zajęte – najbliższe wolne pasujące.</summary>
    static Pole FindSafePole(Linia linia, int wanted, GameObject prefab)
    {
        var wolne = linia.pola.Where(p => p != null && p.unit == null && Linia.Pasuje(prefab, p)).ToList();
        return wolne.FirstOrDefault(p => p.nr == wanted)
            ?? wolne.OrderBy(p => Mathf.Abs(p.nr - wanted)).FirstOrDefault();
    }

    /// <summary>Tworzy jednostkę na ławce. benchIndex = -1 -> pierwsze wolne miejsce. Null, gdy ławka pełna.</summary>
    public static GameObject SpawnOnBench(GameObject prefab, int benchIndex)
    {
        List<Pole> lawka = Shop.lawka;
        Pole target = (benchIndex >= 0 && benchIndex < lawka.Count && lawka[benchIndex].unit == null)
            ? lawka[benchIndex]
            : lawka.FirstOrDefault(p => p.unit == null);
        if (target == null) return null;

        GameObject go = UnityEngine.Object.Instantiate(prefab);
        target.unit = go;
        target.Start();
        return go;
    }

    static void Refund(SaveManager.SaveUnit d)
    {
        int value = Mathf.Max(0, (d.RealCost != 0 ? d.RealCost : d.Cost) - 1);
        MoneyManager.money += value;
        Debug.LogWarning($"Brak miejsca dla jednostki Id={d.Id} – zwrócono {value} złota.");
    }

    /// <summary>Stan rozgrywki (zamiast Load2()). Zwraca null, jeśli pliku nie ma.</summary>
    public static SaveManager.SaveData2 LoadRun()
    {
        if (!File.Exists(SavePaths.Run)) return null;

        SaveManager.SaveData2 data;
        try { data = JsonUtility.FromJson<SaveManager.SaveData2>(File.ReadAllText(SavePaths.Run)); }
        catch (Exception e) { Debug.LogWarning("Uszkodzony zapis rozgrywki: " + e.Message); return null; }
        if (data == null) return null;

        StatsManager.life = data.lose;
        StatsManager.Round = data.round;
        StatsManager.win = data.win;
        MoneyManager.income = data.incom;
        MoneyManager.money = data.money;
        RankedManager.Poddymka = data.poddymka;
        PlayerManager.Id = data.playerId;
        PlayerManager.Time = data.time;
        ShopManager.allRoll = data.allRoll;     // BYŁ BŁĄD: allRoll trafiał do nizka
        ShopManager.nizka = data.levelDiscount; // stare zapisy: 0
        PlayerManager.Popular = (data.popular ?? new List<SaveManager.SerializableKeyValuePair>()).ToDictionary(x => x.Key, x => x.Value);
        PlayerManager.Strong = (data.strong ?? new List<SaveManager.SerializableKeyValuePair>()).ToDictionary(x => x.Key, x => x.Value);

        if (data.round >= 0 && data.round < Enemies.Comps.Count && !string.IsNullOrEmpty(data.nextFight))
            Enemies.Comps[data.round] = data.nextFight;

        FractionsFromString(data.fractions);

        if (StoryManager.Active)
            StoryManager.DialogueShownRound = data.storyDialogueRound;

        if (data.heroSaved)
        {
            HeroState.RerollsLeft = data.heroRerolls;
            HeroState.LastShopRound = data.heroLastShopRound;
            HeroState.Offer = data.heroOffer ?? new List<int>();
        }

        // Sklep
        for (int i = 0; i < Shop.character.Length; i++)
        {
            int id = (data.shop != null && i < data.shop.Count) ? data.shop[i] : -1;
            ShopObject slot = Shop.character[i];
            slot.unit = (id >= 0 && id < Chars.characters.Count) ? Chars.characters[id] : slot.nullObject;
            slot.SetLook();
        }

        // Ławka
        foreach (Pole p in Shop.lawka)
        {
            if (p.unit != null) { UnityEngine.Object.Destroy(p.unit); p.unit = null; }
        }
        foreach (var d in data.units ?? new List<SaveManager.SaveUnit>())
        {
            List<GameObject> lista = d.Spell ? Chars.Spells : Chars.characters; // zaklęcia mają osobną listę
            if (d.Id < 0 || d.Id >= lista.Count) continue;
            GameObject go = SpawnOnBench(lista[d.Id], d.Pole);
            if (go != null) Apply(d, go.GetComponent<Unit>());
        }
        return data;
    }

    public static void DeleteRun()
    {
        if (File.Exists(SavePaths.Run))
            File.Delete(SavePaths.Run);
    }

    /// <summary>Do menu: czy jest zapis do wznowienia dla tego gracza (suffix "" = zwykły, "R" = ranked).</summary>
    public static bool HasRunFor(string suffix, int playerId)
    {
        string path = SavePaths.RunFor(suffix);
        if (!File.Exists(path)) return false;
        try
        {
            var data = JsonUtility.FromJson<SaveManager.SaveData2>(File.ReadAllText(path));
            return data != null && data.playerId == playerId;
        }
        catch { return false; }
    }

    /// <summary>
    /// false = wysyłaj jeszcze stary JSON (okres przejściowy, gdy część graczy ma starą wersję gry).
    /// Nowa wersja i tak CZYTA oba formaty.
    /// </summary>
    public static bool UploadCompact = true;

    /// <summary>
    /// Tekst planszy do wysłania (baza / Photon). Czyta PLIK, czyli stan sprzed walki –
    /// po walce część jednostek na planszy już nie żyje.
    /// </summary>
    public static string ExportBoard()
    {
        SaveManager.SaveData data = File.Exists(SavePaths.Board) ? ParseBoard(File.ReadAllText(SavePaths.Board)) : null;
        if (data == null) data = CaptureBoard();
        return UploadCompact ? SaveCodec.Encode(data) : JsonUtility.ToJson(data);
    }

    // =====================================================================
    //  FRAKCJE
    // =====================================================================

    static string FractionsToString()
    {
        if (Fraction.fractionList == null) Fraction.fractionList = new List<Fraction.fractionType>();
        string s = "";
        foreach (Fraction.fractionType t in Enum.GetValues(typeof(Fraction.fractionType)))
            s += Fraction.fractionList.Contains(t) ? "1" : "0";
        return s;
    }

    static void FractionsFromString(string s)
    {
        if (string.IsNullOrEmpty(s)) return;
        Fraction.fractionList = new List<Fraction.fractionType>();
        for (int i = 0; i < s.Length; i++)
            if (s[i] == '1') Fraction.fractionList.Add((Fraction.fractionType)i);
    }
}
