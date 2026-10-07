using UnityEngine;

/// <summary>Ikona zdolności bohatera (obiekt świata z Collider2D – tak jak karty Bohater).</summary>
public class HeroAbilityButton : MonoBehaviour
{
    [Tooltip("1, 2 albo 3")]
    public int ability = 1;

    void OnMouseDown() => BohaterInGame.Instance.Buy(ability);
    void OnMouseEnter() => BohaterInGame.Instance.ShowTooltip(ability);
    void OnMouseExit() => BohaterInGame.Instance.HideTooltip();
}
