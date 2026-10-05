using UnityEngine;
using UnityEngine.UI;
using TMPro;
// using System.Diagnostics;

public class Bohater : MonoBehaviour
{
    public BohaterData data; // ← tutaj przeciągasz stworzony plik danych
    public TextMeshProUGUI Name;

    public Image Image;
    public TextMeshProUGUI Text1;
    public TextMeshProUGUI Text2;
    public TextMeshProUGUI Text3;
    public BohaterManager bohaterManager;

    public void Start()
    {
        if (data == null)
        {
            Debug.LogWarning("Brak przypisanego BohaterData!");
            return;
        }
        if (Name != null)
        {
            Name.text = data.name;
        }

        if (Image != null && data.Image != null)
            Image.sprite = data.Image;

        if (Text1 != null)
            Text1.text = data.text1;

        if (Text2 != null)
            Text2.text = data.text2;

        if (Text3 != null)
            Text3.text = data.text3;
    }
    public void OnMouseDown()
    {
        bohaterManager.ChoseBohater = data;
        bohaterManager.Pick();
    }
}
