using System.Collections.Generic; // Dodaj tę dyrektywę
using UnityEngine;
using UnityEngine.UI;

public class Fraction : MonoBehaviour
{
    public Image[] wycinek; // Pierwszy wycinek (1/3)

    public Image[] przyciski;


    public static List<fractionType> fractionList; // Teraz List<T> będzie rozpoznawany

    public enum fractionType
    {
        Ludzie,
        Nekro,
        Nomadzi,
        Elfy,
        Krasnoludy
    }

     // Funkcja przyjmująca parametr typu int
    public void OnMouseDown(int nr)
    {
        // Sprawdź, czy nr mieści się w zakresie enum fractionType
        if (System.Enum.IsDefined(typeof(fractionType), nr))
        {
            // Konwertuj int na fractionType
            fractionType fraction = (fractionType)nr;

            // Sprawdź, czy lista zawiera podany element
            if (fractionList.Contains(fraction))
            {
                // Jeśli zawiera, usuń go
                fractionList.Remove(fraction);
                UstawFrakcje();
                przyciski[nr].color = Color.white;
            }
            else
            {
                // Jeśli nie zawiera, dodaj go
                fractionList.Add(fraction);
                UstawFrakcje();
                przyciski[nr].color = Color.gray;
            }

        }
    }

    public void Rester()
    {
        for(int i = 0; i < 5; i ++)
        if (System.Enum.IsDefined(typeof(fractionType), i))
        {
            // Konwertuj int na fractionType
            fractionType fraction = (fractionType)i;

            // Sprawdź, czy lista zawiera podany element
            if (fractionList.Contains(fraction))
            {
                // Jeśli zawiera, usuń go
                fractionList.Remove(fraction);
                UstawFrakcje();
                przyciski[i].color = Color.white;
            }
        }
    }

    void Start()
    {
        fractionList = new List<fractionType>();
        OnMouseDown(0);
    }

    void UstawFrakcje()
    {
        int ilosc = fractionList.Count;
        int j = 1;
        for(int i = 0; i < 5; i++)
        {
                fractionType fraction = (fractionType)i;

                // Sprawdź, czy lista zawiera podany element
                if (fractionList.Contains(fraction))
                {
                    UstawWycinek(wycinek[i], 0f, j / (float)ilosc);
                    j++;
                }
                else
                {
                    UstawWycinek(wycinek[i], 0f, 0f); // chce puste
                }
        }
        
        //UstawWycinek(wycinek[0], 0f, 1f / (float)ilosc);

    }


    void UstawWycinek(Image wycinek, float startFill, float fillAmount)
    {
        // Ustaw tryb na Filled
        wycinek.type = Image.Type.Filled;

        // Ustaw metodę wypełniania na Radial (wykres kołowy)
        wycinek.fillMethod = Image.FillMethod.Radial360;

        // Ustaw początek wypełniania na górę (360 stopni)
        wycinek.fillOrigin = (int)Image.Origin360.Top;

        // Ustaw wypełnienie na odpowiednią wartość
        wycinek.fillAmount = fillAmount;

        // Obróć wycinek, aby zaczynał się w odpowiednim miejscu
        wycinek.transform.rotation = Quaternion.Euler(0, 0, -startFill * 360f);
    }
}