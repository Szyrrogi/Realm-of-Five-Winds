using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BohaterManager : MonoBehaviour
{
    public List<BohaterData> AllBohater;   // wszystkie 15 plików BohaterData
    public List<Bohater> BohaterObject;    // 2 karty wyboru (bez zmian)
    public GameObject PickObject;          // panel wyboru (bez zmian)
    public BohaterData ChoseBohater;
    public BohaterInGame bohaterInGame;

    [Header("Losowanie (NOWE)")]
    public GameObject RerollButton;        // np. istniejący "Button (Legacy)" w panelu Choose Hero
    public TMP_Text RerollText;            // tekst TextMeshPro na przycisku (jeśli taki masz)
    public Text RerollTextLegacy;          // albo zwykły tekst (Legacy) – przycisk Legacy ma taki w dziecku "Text (Legacy)"

    public static BohaterManager Instance;

    /// <summary>Stare wywołania SaveManager.Save(..., BohaterManager.bohaterId) dalej się kompilują.</summary>
    public static int bohaterId => HeroState.Id;

    static readonly string[] RerollWord = { "Losuj", "Reroll", "Volver a tirar", "Relancer", "Neu würfeln" };

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // O tym, czy pokazać wybór, decyduje SaveManager po wczytaniu zapisu (ShowStateAfterLoad).
        PickObject.SetActive(false);
        bohaterInGame.gameObject.SetActive(false);
    }

    /// <summary>Wołane przez SaveManager.Start() po wczytaniu zapisu.</summary>
    public void ShowStateAfterLoad()
    {
        if (StoryManager.Active && HeroState.Id == HeroIds.Brak) return; // misja bez bohatera (sekcja 5)

        if (HeroState.Id != HeroIds.Brak)
            ShowInGame();
        else
            BeginPick();
    }

    void BeginPick()
    {
        HeroState.Picking = true;
        if (HeroState.RerollsLeft < 0)
        {
            // Ranked: tyle losowań, ile wybranych frakcji - 1. Tryb swobodny: bez limitu.
            int frakcje = Fraction.fractionList != null ? Fraction.fractionList.Count : 1;
            HeroState.RerollsLeft = RankedManager.Ranked ? Mathf.Max(0, frakcje - 1) : int.MaxValue;
        }
        PickObject.SetActive(true);

        // Po restarcie pokazujemy te same karty, co przed wyjściem z gry.
        if (HeroState.Offer.Count > 0 && HeroState.Offer.All(id => Find(id) != null))
            ShowOffer();
        else
            NewOffer();
    }

    List<BohaterData> Pool()
    {
        var f = Fraction.fractionList;
        return AllBohater.Where(b => b != null && (f == null || f.Count == 0 || f.Contains(b.fraction))).ToList();
    }

    void NewOffer()
    {
        List<BohaterData> pool = Pool();
        List<BohaterData> fresh = pool.Where(b => !HeroState.Offer.Contains(b.bohaterId)).ToList();
        if (fresh.Count >= 2) pool = fresh; // jeśli się da, pokaż innych niż poprzednio

        HeroState.Offer = pool.OrderBy(x => Random.value).Take(2).Select(b => b.bohaterId).ToList();
        ShowOffer();
        SaveService.SaveAll();
    }

    void ShowOffer()
    {
        for (int i = 0; i < BohaterObject.Count; i++)
        {
            BohaterData d = i < HeroState.Offer.Count ? Find(HeroState.Offer[i]) : null;
            BohaterObject[i].gameObject.SetActive(d != null);
            if (d == null) continue;
            BohaterObject[i].data = d;
            BohaterObject[i].Start();
        }
        RefreshRerollUI();
    }

    /// <summary>Stara nazwa – istniejący przycisk w scenie ma OnClick ustawiony na Roll().</summary>
    public void Roll() => Reroll();

    /// <summary>Przycisk losowania.</summary>
    public void Reroll()
    {
        if (!HeroState.Picking || HeroState.RerollsLeft <= 0) return;
        if (HeroState.RerollsLeft != int.MaxValue) HeroState.RerollsLeft--;
        NewOffer();
    }

    void RefreshRerollUI()
    {
        bool bezLimitu = HeroState.RerollsLeft == int.MaxValue;
        if (RerollButton != null) RerollButton.SetActive(bezLimitu || HeroState.RerollsLeft > 0);
        string slowo = RerollWord[PauseMenu.Language];
        string napis = bezLimitu ? slowo : $"{slowo} ({HeroState.RerollsLeft})";
        if (RerollText != null) RerollText.text = napis;
        if (RerollTextLegacy != null) RerollTextLegacy.text = napis;
    }

    /// <summary>Bohater.OnMouseDown() ustawia ChoseBohater i woła Pick() – tak jak wcześniej.</summary>
    public void Pick()
    {
        if (!HeroState.Picking || ChoseBohater == null) return;

        HeroState.Id = ChoseBohater.bohaterId;
        HeroState.Level = 1;
        HeroState.Picking = false;
        HeroState.Offer.Clear();
        PickObject.SetActive(false);
        ShowInGame();

        HeroAbilities.OnUnlocked(1);
        HeroAbilities.OnShopPhase(StatsManager.Round);
        SaveService.SaveAll();
    }

    void ShowInGame()
    {
        bohaterInGame.gameObject.SetActive(true);
        bohaterInGame.Show(Find(HeroState.Id));
    }

    public BohaterData Find(int id) => AllBohater.FirstOrDefault(b => b != null && b.bohaterId == id);
}
