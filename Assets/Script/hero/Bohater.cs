using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Karta bohatera w panelu wyboru (UI na CanvasMain).
/// Klik działa przez system UI (IPointerClickHandler) – OnMouseDown nie trafia w elementy
/// Canvasa w trybie Screen Space – Overlay, dlatego wcześniej wybór nie reagował.
/// Wymaga: Image z zaznaczonym Raycast Target na tym obiekcie (albo na dziecku) i obiektu EventSystem w scenie.
/// </summary>
public class Bohater : MonoBehaviour, IPointerClickHandler
{
    public BohaterData data; // ustawiane przez BohaterManager przy losowaniu
    public TextMeshProUGUI Name;

    public Image Image;
    public TextMeshProUGUI Text1;
    public TextMeshProUGUI Text2;
    public TextMeshProUGUI Text3;
    public BohaterManager bohaterManager;

    public void Start()
    {
        if (data == null)
            return; // karta czeka, aż BohaterManager wylosuje bohatera

        if (Name != null)
            Name.text = data.name;

        if (Image != null && data.Image != null)
            Image.sprite = data.Image;

        if (Text1 != null)
            Text1.text = data.text1;

        if (Text2 != null)
            Text2.text = data.text2;

        if (Text3 != null)
            Text3.text = data.text3;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Wybierz();
    }

    /// <summary>Można też podpiąć pod Button.OnClick.</summary>
    public void Wybierz()
    {
        if (data == null) return;
        BohaterManager manager = bohaterManager != null ? bohaterManager : BohaterManager.Instance;
        if (manager == null) return;
        manager.ChoseBohater = data;
        manager.Pick();
    }
}
