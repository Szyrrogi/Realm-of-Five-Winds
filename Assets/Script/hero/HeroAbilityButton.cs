using UnityEngine;

/// <summary>
/// Plus z ceną pod opisami zdolności (obiekt świata z BoxCollider2D).
/// Klik kupuje następną zdolność – najpierw 2, potem 3.
/// </summary>
public class HeroAbilityButton : MonoBehaviour
{
    void OnMouseDown()
    {
        if (BohaterInGame.Instance != null)
            BohaterInGame.Instance.BuyNext();
    }
}
