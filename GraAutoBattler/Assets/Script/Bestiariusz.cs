using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;


public class Bestiariusz : MonoBehaviour
{
    public CharacterManager characterManager;
    public List<GameObject> Show;
    public int Page;
    public List<Image> Face;
    public List<DescriptionManager> Opisy;
    public List<TextMeshProUGUI> Lore;

    public Fraction.fractionType Frakcja;
    public int type;

    public GameObject OpisSzczegolu;
    public TextMeshProUGUI OpisCaly;

    public void ShowSzczegoly(int i)
    {
        int index = i + Page * 4;
        OpisCaly.text = Show[index].GetComponent<Unit>().Lore;
        OpisSzczegolu.SetActive(true);
    }

    public void Showaj()
    {
        OpisSzczegolu.SetActive(false);
    }

    public void Next()
    {
        if(Page * 4 < Show.Count - 4)
        {
            Page++;
            Complete() ;
        }
    }

    public void Prev()
    {
        if(Page != 0)
        {
            Page--;
            Complete() ;
        }
    }

    public void Start()
    {
        Show = characterManager.characters;
        Show = Filter(Show);
        Complete();


    }

    public void Complete()
    {
        if (type == 0)
        {
            // Oryginalna logika dla type == 0 (4 elementy × 2 miejsca = 8 slotów)
            for (int i = 0; i < 4; i++)
            {
                int index = i + Page * 4;

                if (index >= Show.Count || Show[index] == null)
                {
                    Face[i*2].gameObject.SetActive(false);
                    Face[i*2 + 1].gameObject.SetActive(false);
                    Opisy[i*2].gameObject.SetActive(false);
                    Opisy[i*2 + 1].gameObject.SetActive(false);
                    
                    if (i < Lore.Count)
                        Lore[i].text = "";
                    
                    continue;
                }

                // Aktywuj sloty
                Face[i*2].gameObject.SetActive(true);
                Face[i*2 + 1].gameObject.SetActive(true);
                Opisy[i*2].gameObject.SetActive(true);
                Opisy[i*2 + 1].gameObject.SetActive(true);

                var spriteRenderer = Show[index].GetComponent<SpriteRenderer>();
                var herosComponent = Show[index].GetComponent<Heros>();
                var unitComponent = Show[index].GetComponent<Unit>();

                // Postać podstawowa
                Face[i*2].sprite = spriteRenderer?.sprite;
                Opisy[i*2].unit = unitComponent;
                Opisy[i*2].gameObject.SetActive(unitComponent != null);

                // Ewolucja (jeśli istnieje)
                if (herosComponent?.EvolveHeroes != null)
                {
                    var evolveSpriteRenderer = herosComponent.EvolveHeroes.GetComponent<SpriteRenderer>();
                    Face[i*2 + 1].sprite = evolveSpriteRenderer?.sprite;
                    
                    var evolveUnit = herosComponent.EvolveHeroes.GetComponent<Unit>();
                    Opisy[i*2 + 1].unit = evolveUnit;
                    Opisy[i*2 + 1].gameObject.SetActive(evolveUnit != null);
                }
                else
                {
                    Face[i*2 + 1].sprite = null;
                    Opisy[i*2 + 1].gameObject.SetActive(false);
                }

                // Lore tylko dla type == 0
                if (i < Lore.Count)
                {
                    Lore[i].text = unitComponent != null 
                        ? (unitComponent.Lore?.Length > 100 
                            ? unitComponent.Lore.Substring(0, 100) + " (czytaj dalej)" 
                            : unitComponent.Lore) ?? ""
                        : "";
                }
            }
        }
        else // type == 1
        {
            // Nowa logika dla type == 1 (8 elementów w 8 slotach, bez ewolucji i bez LORE)
            for (int i = 0; i < 8; i++)
            {
                int index = i + Page * 8;

                if (index >= Show.Count || Show[index] == null)
                {
                    Face[i].gameObject.SetActive(false);
                    Opisy[i].gameObject.SetActive(false);
                    continue;
                }

                Face[i].gameObject.SetActive(true);
                Opisy[i].gameObject.SetActive(true);

                var spriteRenderer = Show[index].GetComponent<SpriteRenderer>();
                var unitComponent = Show[index].GetComponent<Unit>();

                Face[i].sprite = spriteRenderer?.sprite;
                Opisy[i].unit = unitComponent;
                Opisy[i].gameObject.SetActive(unitComponent != null);
            }
            for(int i = 0; i < 4; i++)
            {
                Lore[i].text = "";
            }
        }
    }

    public void SetType(int newType)
    {
        type = newType;
        Start();
    }

    public List<GameObject> Filter(List<GameObject> gameObjects)
    {
        if(type == 0)
        return gameObjects
            .Where(go => go.GetComponent<Heros>() != null && !go.GetComponent<Heros>().Evolution
            && go.GetComponent<Heros>().Star != 0 && go.GetComponent<Heros>().fraction == Frakcja)
            .OrderBy(go => go.GetComponent<Heros>().Cost)
            .ToList();
        else
        return gameObjects
            .Where(go => go.GetComponent<Building>() != null && go.GetComponent<Unit>().Star != 0 && 
             go.GetComponent<Unit>().fraction == Frakcja)
            .OrderBy(go => go.GetComponent<Unit>().Cost)
            .ToList();

    }
    public void UstawFrakcja(int frakcjaIndex)
    {
        // Sprawdź czy indeks jest w zakresie dostępnych frakcji
        if (System.Enum.IsDefined(typeof(Fraction.fractionType), frakcjaIndex))
        {
            Frakcja = (Fraction.fractionType)frakcjaIndex;
            Page = 0;
            Start(); // Odśwież widok
        }
        else
        {
            Debug.LogWarning($"Nieznany indeks frakcji: {frakcjaIndex}");
            // Możesz tutaj ustawić domyślną frakcję lub zignorować
        }
    }
}
