using UnityEngine;

/// <summary>Przycisk losowania bohaterów (obiekt świata z Collider2D).</summary>
public class BohaterRerollButton : MonoBehaviour
{
    void OnMouseDown() => BohaterManager.Instance.Reroll();
}
