using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fraction : MonoBehaviour
{
    public Image[] przyciski; // Tablica przycisków frakcji
    public static List<fractionType> fractionList;

    public enum fractionType
    {
        Ludzie,
        Nekro,
        Nomadzi,
        Elfy,
        Krasnoludy
    }

    void Start()
    {
        fractionList = new List<fractionType>();
        UpdateButtonsVisibility();
    }

    public void OnFractionButtonClick(int nr)
    {
        if (System.Enum.IsDefined(typeof(fractionType), nr))
        {
            fractionType fraction = (fractionType)nr;
            
            if (fractionList.Contains(fraction))
                fractionList.Remove(fraction);
            else
                fractionList.Add(fraction);
            
            UpdateButtonsVisibility();
        }
    }

    public void ResetSelection()
    {
        fractionList.Clear();
        UpdateButtonsVisibility();
    }

    void UpdateButtonsVisibility()
    {
        for (int i = 0; i < przyciski.Length; i++)
        {
            SetVisibility(przyciski[i], fractionList.Contains((fractionType)i));
        }
    }

    void SetVisibility(Image element, bool visible)
    {
        // Zachowuje oryginalny kolor, zmienia tylko przezroczystość
        Color color = element.color;
        color.a = visible ? 1f : 0f; // 1 = widoczne, 0 = niewidoczne
        element.color = color;
        
        // Opcjonalnie: wyłącza interakcję gdy niewidoczne
        //element.raycastTarget = visible;
    }
}