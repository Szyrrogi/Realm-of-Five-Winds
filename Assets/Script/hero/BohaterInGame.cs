using TMPro;
using UnityEngine;

/// <summary>
/// Panel bohatera w trakcie gry.
/// Od początku widać opis zdolności 1, a pod nim plus z ceną zdolności 2.
/// Po kupnie pojawia się kolejne tło (pod HeroBack) z opisem, a plus przesuwa się niżej z ceną zdolności 3.
/// Po kupnie wszystkich trzech plus znika.
/// </summary>
public class BohaterInGame : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;   // portret bohatera

    [Header("Opisy zdolności – po 3 elementy, kolejno zdolność 1, 2, 3")]
    [Tooltip("Tła opisów pod HeroBack: Text1, Text1 (1), Text1 (2). Pokazują się po odblokowaniu zdolności.")]
    public GameObject[] abilitySlots;
    [Tooltip("Teksty opisów (3D Object → Text - TextMeshPro), najlepiej dzieci teł z listy wyżej.")]
    public TMP_Text[] abilityTexts;

    [Header("Plus z ceną")]
    [Tooltip("Obiekt z SpriteRenderer (plus) + BoxCollider2D + HeroAbilityButton.")]
    public GameObject buyButton;
    [Tooltip("Tekst z ceną przy plusie, np. \"6 $\".")]
    public TMP_Text buyPriceText;
    [Tooltip("Opcjonalne: gdzie stoi plus przed kupnem zdolności 2 i 3 (2 elementy). Puste = w miejscu tła następnej zdolności.")]
    public Transform[] buyButtonPlaces;

    public static BohaterInGame Instance;
    BohaterData data;
    int shownLevel = -1;

    static readonly Color CanBuy = Color.white;
    static readonly Color CantBuy = new Color(0.5f, 0.5f, 0.5f, 0.8f);

    void Awake()
    {
        Instance = this;
    }

    // Stare API – zostaje dla zgodności.
    public void Show(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void Show(BohaterData d)
    {
        data = d;
        if (d != null) spriteRenderer.sprite = d.Image;
        shownLevel = -1; // wymuś odświeżenie opisów
        Refresh();
    }

    void Update()
    {
        Refresh();
    }

    /// <summary>Plus: kupuje następną zdolność (najpierw 2, potem 3).</summary>
    public void BuyNext()
    {
        Buy(HeroState.Level + 1);
    }

    /// <summary>Kupno zdolności nr 2 albo 3. Tylko po kolei i tylko w fazie sklepu.</summary>
    public void Buy(int ability)
    {
        if (FightManager.IsFight || FightManager.IsOptions || HeroState.Picking) return;
        if (ability != HeroState.Level + 1) return;

        int cost = HeroState.NextCost;
        if (cost < 0 || MoneyManager.money < cost) return;

        MoneyManager.money -= cost;
        HeroState.Level = ability;
        HeroAbilities.OnUnlocked(ability);
        SaveService.SaveAll();
    }

    void Refresh()
    {
        if (shownLevel != HeroState.Level)
        {
            shownLevel = HeroState.Level;
            RefreshSlots();
        }
        RefreshBuyButton();
    }

    /// <summary>Pokazuje tła i opisy odblokowanych zdolności, ukrywa resztę.</summary>
    void RefreshSlots()
    {
        for (int i = 0; i < 3; i++)
        {
            bool unlocked = HeroState.Level >= i + 1;

            if (abilitySlots != null && i < abilitySlots.Length && abilitySlots[i] != null)
                abilitySlots[i].SetActive(unlocked);

            if (abilityTexts != null && i < abilityTexts.Length && abilityTexts[i] != null)
            {
                abilityTexts[i].text = Opis(i + 1);
                abilityTexts[i].gameObject.SetActive(unlocked);
            }
        }

        // Plus stoi pod ostatnim odblokowanym opisem, czyli tam, gdzie pojawi się następny.
        if (buyButton != null)
        {
            Transform place = PlusPlace(HeroState.Level);
            if (place != null)
            {
                Vector3 p = place.position;
                p.z = buyButton.transform.position.z; // zostaw własną głębokość plusa
                buyButton.transform.position = p;
            }
        }
    }

    Transform PlusPlace(int level)
    {
        // level 1 -> miejsce przed kupnem zdolności 2, level 2 -> przed kupnem zdolności 3
        int i = level - 1;
        if (buyButtonPlaces != null && i >= 0 && i < buyButtonPlaces.Length && buyButtonPlaces[i] != null)
            return buyButtonPlaces[i];
        if (abilitySlots != null && level >= 0 && level < abilitySlots.Length && abilitySlots[level] != null)
            return abilitySlots[level].transform; // miejsce tła następnej zdolności
        return null;
    }

    void RefreshBuyButton()
    {
        if (buyButton == null) return;

        int cost = HeroState.NextCost; // -1, gdy nie ma czego kupić
        bool show = cost >= 0 && !HeroState.Picking;
        if (buyButton.activeSelf != show) buyButton.SetActive(show);
        if (!show) return;

        bool stac = MoneyManager.money >= cost;
        if (buyPriceText != null) buyPriceText.text = cost + " $";
        SpriteRenderer sr = buyButton.GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = stac ? CanBuy : CantBuy;
    }

    string Opis(int ability)
    {
        if (data == null) return "";
        return ability == 1 ? data.text1 : ability == 2 ? data.text2 : data.text3;
    }
}
