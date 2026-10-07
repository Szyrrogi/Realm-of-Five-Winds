using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using H = HeroIds;

/// <summary>
/// Wszystkie zdolności bohaterów w jednym pliku. Reszta gry woła tylko te metody
/// (ceny, dochód, początek walki, prezenty), więc zmiana balansu = zmiana tutaj.
/// </summary>
public static class HeroAbilities
{
    static bool Has(int hero, int ability, bool enemy) => HeroState.Has(hero, ability, enemy);
    static FightManager FM => EventSystem.eventSystem.GetComponent<FightManager>();
    static CharacterManager Chars => EventSystem.eventSystem.GetComponent<CharacterManager>();
    static readonly Color Purple = new Color(0.5f, 0f, 1f);

    // =====================================================================
    //  GRUPY JEDNOSTEK – jeśli chcesz, żeby zdolność łapała inne jednostki, zmień tutaj
    // =====================================================================

    static bool IsType(Unit u, Unit.CreatureType t) => u.Typy != null && u.Typy.Contains(t);
    static bool IsAdept(Unit u) => u.GetComponent<AdeptMroczny>() || (u.Name != null && u.Name.Length > 0 && u.Name[0] == "Adept");
    static bool IsShieldBearer(Unit u) => u.GetComponent<Tarczownik>() || u.GetComponent<PustynnyTarczownik>();
    static bool IsGolem(Unit u) => u.GetComponent<Golem>() || u.GetComponent<PiaskowyGolem>();
    static bool IsArcher(Unit u) => u.Range > 0;

    // =====================================================================
    //  SKLEP I EKONOMIA
    // =====================================================================

    /// <summary>Cena w sklepie po zniżkach bohatera. price = cena bazowa (RealCost albo Cost).</summary>
    public static int ShopPrice(GameObject prefab, int price)
    {
        Unit u = prefab != null ? prefab.GetComponent<Unit>() : null;
        if (u == null) return price;
        Heros hero = prefab.GetComponent<Heros>();

        if (Has(H.Gogol, 1, false) && (prefab.GetComponent<Bank>() || prefab.GetComponent<Kopalnia>()))
            price -= 1;
        if (Has(H.WampirzyZiomek, 1, false) && hero && IsType(u, Unit.CreatureType.Wampir))
            price -= 1;
        if (Has(H.Magik, 2, false) && (prefab.GetComponent<NewMagGuildd>() || prefab.GetComponent<MagGuild>()))
            price = 0;
        if (Has(H.Kopacz, 1, false) && prefab.GetComponent<WarsztatGolemow>()) // Hala Rekrutów i Warsztat Golemów
            price -= 1;
        if (Has(H.Lucznik, 3, false))
        {
            if (prefab.GetComponent<ElfiDowódca>()) price -= 3;
            if (prefab.GetComponent<Nauczyciel>()) price -= 2;
        }
        if (Has(H.Gambler, 3, false) && hero && hero.Evolution)
            price = (price + 1) / 2;

        return Mathf.Max(0, price);
    }

    public static int BonusIncome() => Has(H.Gogol, 3, false) ? 3 : 0;
    public static int FreeRollsFromTree() => Has(H.Drewniak, 1, false) ? 2 : 1;
    public static int JaskolkaBonusAP(bool enemy) => Has(H.Arcymag, 1, enemy) ? 15 : 0;
    public static int CmentarzBonus(bool enemy) => Has(H.Szkieletor, 3, enemy) ? 20 : 0;
    public static int NawiedzonyDworBonus(bool enemy) => Has(H.WampirzyZiomek, 2, enemy) ? 15 : 0;
    public static int ObozBonus(bool enemy) => Has(H.Skoczek, 2, enemy) ? 5 : 0;
    public static bool ZemstaKeepsStats(bool enemy) => Has(H.Szkieletor, 2, enemy);
    public static int WielkiGolemCopies() => Has(H.Kopacz, 3, false) ? 3 : 1;

    /// <summary>Ile połączeń potrzeba do ewolucji (zamiast pola UpgradeNeed).</summary>
    public static int UpgradeNeed(Heros h)
    {
        if (h.GetComponent<Szczur>() && Has(H.SzczuronMichael, 2, h.Enemy)) return Mathf.Min(h.UpgradeNeed, 2);
        if (h.GetComponent<Ent>() && Has(H.Drewniak, 2, h.Enemy)) return Mathf.Min(h.UpgradeNeed, 2);
        return h.UpgradeNeed;
    }

    // =====================================================================
    //  PREZENTY
    // =====================================================================

    /// <summary>Wołane raz, w chwili odblokowania zdolności (1 = przy wyborze bohatera).</summary>
    public static void OnUnlocked(int ability)
    {
        int id = HeroState.Id;
        if (id == H.Gambler && ability == 1) Give("Loch");
        if (id == H.Saperito && ability == 1) Give("Pułapka");
        if (id == H.Arcymag && ability == 2) Give("Pirokataklizm");
        if (id == H.Gambler && ability == 2) Give("Obieżyświat", true);
        if (id == H.MlotMistrz && ability == 2) { GiveRandomBuilding(); GiveRandomBuilding(); }
    }

    /// <summary>Wołane na początku każdej fazy sklepu (po wyborze bohatera i po każdej walce).</summary>
    public static void OnShopPhase(int round)
    {
        if (HeroState.Id == H.Brak || round <= HeroState.LastShopRound) return; // bez podwójnych prezentów po wczytaniu
        HeroState.LastShopRound = round;

        if (Has(H.SzczuronMichael, 1, false) && round < 3) Give("Szczur");
        if (Has(H.Szkieletor, 1, false) && round == 4) Give("Grabarz");
        if (Has(H.Skoczek, 1, false) && round < 3) EventSystem.eventSystem.GetComponent<ShopManager>().FreeRoll++;
    }

    static void Give(string polishName, bool evolved = false)
    {
        GameObject prefab = FindPrefab(polishName, evolved);
        if (prefab == null) { Debug.LogWarning("Bohater: nie znaleziono prefabu " + polishName); return; }
        if (SaveService.SpawnOnBench(prefab, -1) == null)
            MoneyManager.money += prefab.GetComponent<Unit>().Cost; // ławka pełna – zwrot w złocie
    }

    static void GiveRandomBuilding()
    {
        int maxStar = StatsManager.Round / 3 + 1;
        var pool = Chars.characters.Where(go =>
        {
            Unit u = go.GetComponent<Unit>();
            return go.GetComponent<Building>() && u.Star != 0 && u.Star <= maxStar
                && (Fraction.fractionList == null || Fraction.fractionList.Contains(u.fraction));
        }).ToList();
        if (pool.Count == 0) return;
        GameObject prefab = pool[UnityEngine.Random.Range(0, pool.Count)];
        if (SaveService.SpawnOnBench(prefab, -1) == null)
            MoneyManager.money += prefab.GetComponent<Unit>().Cost;
    }

    static GameObject FindPrefab(string polishName, bool evolved)
    {
        foreach (List<GameObject> list in new[] { Chars.characters, Chars.Spells })
        {
            foreach (GameObject go in list)
            {
                Unit u = go.GetComponent<Unit>();
                Heros h = go.GetComponent<Heros>();
                if (u != null && u.Name != null && u.Name.Length > 0 && u.Name[0] == polishName
                    && (h == null || h.Evolution == evolved))
                    return go;
            }
        }
        return null;
    }

    // =====================================================================
    //  WALKA
    // =====================================================================

    static readonly int[] shieldsRestored = new int[2];
    static readonly HashSet<Unit> pendingShield = new HashSet<Unit>();

    /// <summary>Wołane w FightManager.Battle() dla każdej strony, przed OnBattleStart jednostek.</summary>
    public static IEnumerator OnBattleStart(bool enemy)
    {
        shieldsRestored[enemy ? 1 : 0] = 0;
        pendingShield.RemoveWhere(u => u == null || u.Enemy == enemy);

        List<Unit> all = FM.units.Where(u => u != null && u.Enemy == enemy && u.GetComponent<Heros>()).ToList();
        List<Unit> first = RowEnds(enemy, true);
        List<Unit> last = RowEnds(enemy, false);

        // ---- Ludzie ----
        if (Has(H.BestiaMaster, 1, enemy))
            yield return Each(all.Where(u => IsType(u, Unit.CreatureType.Wilki)), u => { u.Health += 5; u.MaxHealth += 5; }, "+5 HP", Color.green);
        if (Has(H.BestiaMaster, 2, enemy))
            yield return Each(all.Where(u => u.GetComponent<Gryf>() || u.GetComponent<GryfLucznik>()), u => u.BoskaTarcza = true, "TARCZA", Color.cyan);
        if (Has(H.BestiaMaster, 3, enemy))
            yield return Each(all.Where(u => IsType(u, Unit.CreatureType.Zwierzęta) && u.fraction != Fraction.fractionType.Ludzie), u => AddStats(u, 20, 20), "+20/20", Color.green);
        if (Has(H.Gogol, 2, enemy) && !enemy) // pieniądze przeciwnika nie są zapisywane
        {
            int kasa = MoneyManager.money;
            yield return Each(all.Where(u => u.GetComponent<Podatnik>() || u.GetComponent<PodatnikPro>()), u => AddStats(u, kasa, kasa), "+" + kasa + "/" + kasa, Color.yellow);
        }
        if (Has(H.Arcymag, 3, enemy))
            yield return Each(all.Where(IsAdept), u => AddStats(u, 40, 40), "+40/40", Color.green);

        // ---- Nekro ----
        if (Has(H.SzczuronMichael, 3, enemy))
        {
            int szczury = all.Count(u => u.GetComponent<Szczur>());
            yield return Each(all.Where(u => u.GetComponent<KingRat>()), u => AddStats(u, 7 * szczury, 7 * szczury), "+" + 7 * szczury + "/" + 7 * szczury, Color.green);
        }
        if (Has(H.WampirzyZiomek, 3, enemy))
            yield return Each(all.Where(u => IsType(u, Unit.CreatureType.Wampir)), u => u.MaxHealth *= 2, "MAX HP x2", Color.green);

        // ---- Nomadzi ----
        if (Has(H.Skoczek, 3, enemy))
            yield return Each(all.Where(u => u.GetComponent<Mantykora>() || u.GetComponent<Czerw>()), u => AddStats(u, 30, 30), "+30/30", Color.green);
        if (Has(H.Magik, 1, enemy))
            yield return Each(all, u => u.AP += 5, "+5", Purple);
        if (Has(H.Magik, 3, enemy))
            yield return Each(first, u => { u.AP += u.Attack; u.Attack = 0; u.attackAP = true; }, "SIŁA → AP", Purple);

        // ---- Krasnoludy ----
        if (Has(H.Kopacz, 2, enemy))
            yield return Each(all.Where(IsGolem), u => AddStats(u, 20, 20), "+20/20", Color.green);
        if (Has(H.MlotMistrz, 1, enemy))
            yield return Each(all.Where(u => u.GetComponent<Sciana>()), u => { u.Health += 40; u.MaxHealth += 40; }, "+40 HP", Color.green);
        if (Has(H.MlotMistrz, 3, enemy))
            yield return Each(all.Where(u => IsType(u, Unit.CreatureType.Obywatel)), u => { u.Initiative += 60; AddStats(u, 30, 30); }, "+30/30", Color.green);
        if (Has(H.Tarczownik, 1, enemy))
            yield return Each(all.Where(IsShieldBearer), u => AddStats(u, 5, 5), "+5/5", Color.green);
        if (Has(H.Tarczownik, 2, enemy))
            yield return Each(first, u => u.BoskaTarcza = true, "TARCZA", Color.cyan);

        // ---- Elfy ----
        if (Has(H.Drewniak, 3, enemy))
            yield return Each(all.Where(u => u.GetComponent<Drzewiec>()), u => { u.Health += 100; u.MaxHealth += 100; }, "+100 HP", Color.green);
        if (Has(H.Lucznik, 2, enemy)) // przed zdolnością 1, żeby "ostatni" nie dostawał +10 tylko za to, że właśnie zyskał zasięg
            yield return Each(all.Where(IsArcher), u => u.Attack += 10, "+10", Color.yellow);
        if (Has(H.Lucznik, 1, enemy))
            yield return Each(last, u => u.Range = Mathf.Max(u.Range, 1), "STRZAŁ", Color.yellow);
        if (Has(H.Saperito, 2, enemy))
            yield return Each(all.Where(u => u.GetComponent<Saper>()), u =>
            {
                u.Attack = Mathf.Max(u.Attack, u.AP);
                u.MaxHealth = Mathf.Max(u.MaxHealth, u.AP);
                u.Health = Mathf.Max(u.Health, u.AP);
            }, "= AP", Purple);
        if (Has(H.Saperito, 3, enemy))
            yield return Each(FirstOfType<Mine>(enemy), u => u.BoskaTarcza = true, "TARCZA", Color.cyan);
    }

    /// <summary>Tarczownik (3): wołane, gdy jednostka traci Boską Tarczę.</summary>
    public static void OnShieldLost(Unit u)
    {
        if (u == null || !Has(H.Tarczownik, 3, u.Enemy)) return;
        int side = u.Enemy ? 1 : 0;
        if (shieldsRestored[side] >= 3) return;
        shieldsRestored[side]++;
        pendingShield.Add(u); // tarcza wraca na początku następnej tury tej jednostki
    }

    /// <summary>Wołane w pętli walki tuż przed ruchem jednostki.</summary>
    public static void BeforeUnitTurn(Unit u)
    {
        if (u != null && pendingShield.Remove(u))
        {
            u.BoskaTarcza = true;
            u.ShowPopUp("TARCZA", Color.cyan);
        }
    }

    // ---- pomocnicze ----

    static void AddStats(Unit u, int attack, int health)
    {
        u.Attack += attack;
        u.Health += health;
        u.MaxHealth += health;
    }

    static IEnumerator Each(IEnumerable<Unit> units, Action<Unit> effect, string popup, Color color)
    {
        bool any = false;
        foreach (Unit u in units.ToList())
        {
            effect(u);
            u.ShowPopUp(popup, color);
            any = true;
        }
        if (any) yield return new WaitForSeconds(0.5f);
    }

    /// <summary>Pierwsza (front = najniższy numer pola) albo ostatnia jednostka bojowa w każdym rzędzie strony.</summary>
    static List<Unit> RowEnds(bool enemy, bool front)
    {
        var result = new List<Unit>();
        int off = enemy ? SaveService.EnemyOffset : 0;
        for (int i = 0; i < 3; i++)
        {
            var units = FM.linie[i + off].pola
                .Where(p => p != null && p.unit != null)
                .Select(p => p.unit.GetComponent<Unit>())
                .Where(u => u != null && u.Enemy == enemy && u.GetComponent<Heros>())
                .OrderBy(u => u.GetComponent<DragObject>().pole.nr)
                .ToList();
            if (units.Count > 0) result.Add(front ? units[0] : units[units.Count - 1]);
        }
        return result;
    }

    static List<Unit> FirstOfType<T>(bool enemy) where T : Component
    {
        var result = new List<Unit>();
        int off = enemy ? SaveService.EnemyOffset : 0;
        for (int i = 0; i < 3; i++)
        {
            Unit u = FM.linie[i + off].pola
                .Where(p => p != null && p.unit != null && p.unit.GetComponent<T>())
                .OrderBy(p => p.nr)
                .Select(p => p.unit.GetComponent<Unit>())
                .FirstOrDefault(x => x != null && x.Enemy == enemy);
            if (u != null) result.Add(u);
        }
        return result;
    }
}
