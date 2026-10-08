using TMPro;
using UnityEngine;

public class BohaterInGame : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    [Header("Zdolności (NOWE) – po 3 elementy: zdolność 1, 2, 3")]
    public SpriteRenderer[] abilityIcons;   // każdy z colliderem i HeroAbilityButton
    public TMP_Text[] abilityLabels;        // koszt albo pusto, gdy kupione
    public TMP_Text tooltip;                // JEDEN wspólny opis: pokazuje tekst tej zdolności, nad którą jest mysz (text1/2/3)

    public static BohaterInGame Instance;
    BohaterData data;

    static readonly Color Owned = Color.white;
    static readonly Color Buyable = new Color(1f, 1f, 1f, 0.75f);
    static readonly Color Locked = new Color(0.4f, 0.4f, 0.4f, 0.6f);

    void Awake()
    {
        Instance = this;
        HideTooltip(); // opis pokazuje się dopiero po najechaniu na ikonę
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
        Refresh();
    }

    void Update()
    {
        Refresh();
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
        for (int i = 0; i < 3; i++)
        {
            int ability = i + 1;
            bool owned = HeroState.Level >= ability;
            bool next = ability == HeroState.Level + 1;
            int cost = HeroState.AbilityCost[ability];

            if (abilityIcons != null && i < abilityIcons.Length && abilityIcons[i] != null)
                abilityIcons[i].color = owned ? Owned : (next && MoneyManager.money >= cost ? Buyable : Locked);
            if (abilityLabels != null && i < abilityLabels.Length && abilityLabels[i] != null)
                abilityLabels[i].text = owned ? "" : cost + " $";
        }
    }

    public void ShowTooltip(int ability)
    {
        if (tooltip == null || data == null) return;
        tooltip.text = ability == 1 ? data.text1 : ability == 2 ? data.text2 : data.text3;
        tooltip.gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        if (tooltip != null) tooltip.gameObject.SetActive(false);
    }
}
