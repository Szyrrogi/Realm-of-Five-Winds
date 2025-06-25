using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopObject : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI name;
    public TextMeshProUGUI price;

    public GameObject unit;
    public GameObject nullObject;

    public GameObject stats;

    public TextMeshProUGUI atakText;
    public TextMeshProUGUI healthText;
    public Image atakImage;

    public List<Image> Type;
    public Sprite[] SpritesType;

    public void SetLook()
    {
                image.sprite = unit.GetComponent<SpriteRenderer>().sprite;
                name.text = unit.GetComponent<Unit>().Name;
                price.text = 
                (unit.GetComponent<Unit>().RealCost == 0 ? unit.GetComponent<Unit>().Cost.ToString() : unit.GetComponent<Unit>().RealCost.ToString());
                if(unit.GetComponent<Heros>())
                    SetStats();
                else
                    stats.SetActive(false);
                
    }


    void OnMouseDown()
    {
        if(unit.GetComponent<Unit>().RealCost == 0)
            unit.GetComponent<Unit>().RealCost = unit.GetComponent<Unit>().Cost;
        if(czyKupic())
        {
            kupno();
        }
    }

    void kupno()
    {
        MoneyManager.money -= unit.GetComponent<Unit>().RealCost;
        Pole poleDocelowe = null;
        foreach(Pole pole in EventSystem.eventSystem.GetComponent<ShopManager>().lawka)
        {
            if(pole.unit == null)
            {
                poleDocelowe = pole;
                break;
            }
        }
        Vector3 pos = poleDocelowe.gameObject.transform.position;
        pos.z -= 2f;
        GameObject newUnit = Instantiate(unit, pos, Quaternion.identity);
        poleDocelowe.unit = newUnit; // Przypisanie jednostki do pola
        newUnit.GetComponent<DragObject>().pole = poleDocelowe;
        newUnit.GetComponent<Unit>().AfterBuy();

        unit = nullObject;
        image.sprite = nullObject.GetComponent<SpriteRenderer>().sprite;
        name.text = nullObject.GetComponent<Unit>().Name;
        price.text = nullObject.GetComponent<Unit>().RealCost.ToString();
        stats.SetActive(false);
        UpdateType();
        // if(trzyma)
        //     newUnit.GetComponent<DragObject>().OnMouseDown();
    }

    public void UpdateType()
    {
        Unit unitComponent = unit.GetComponent<Unit>();
        for(int i = 0; i < Type.Count && i < 3; i++)
        {
            Type[i].gameObject.SetActive(true);
        }

        int typeDisplayIndex = 0;

        // Handle ranged unit type (priority)
        if (unitComponent.Range > 0 && typeDisplayIndex < Type.Count && SpritesType.Length > 0)
        {
            Type[typeDisplayIndex].sprite = SpritesType[0];
            typeDisplayIndex++; 
        }

        // Handle creature types
        if (unitComponent.Typy != null)
        {
            foreach (Unit.CreatureType type in unitComponent.Typy)
            {
                if (typeDisplayIndex >= 3) break; // Only show max 3 types
                
                int typeIndex = (int)type;
                if (typeIndex >= 0 && typeIndex + 1 < SpritesType.Length)
                {
                    Type[typeDisplayIndex].sprite = SpritesType[typeIndex + 1]; // +1 to skip range sprite
                    typeDisplayIndex++;
                }
            }
        }
        if(unitComponent.gameObject.GetComponent<Heros>() && unitComponent.gameObject.GetComponent<Heros>().Evolution  && typeDisplayIndex < 2)
        {
            Type[typeDisplayIndex].sprite = SpritesType[10]; // +1 to skip range sprite
            typeDisplayIndex++;
        }

        // Deactivate unused type indicators
        for(int i = typeDisplayIndex; i < 3 && i < Type.Count; i++)
        {
            Type[i].gameObject.SetActive(false);
        }
    }

    public void SetStats()
    {
        // Cache the unit component
        Unit unitComponent = unit.GetComponent<Unit>();
        
        // Activate stats panel
        stats.SetActive(true);

        // Set attack text and color
        bool useAP = unitComponent.attackAP;
        atakText.text = useAP ? unitComponent.AP.ToString() : unitComponent.Attack.ToString();
        atakImage.color = useAP ? new Color(0.5f, 0, 1f) : Color.yellow;
        
        // Set health text
        healthText.text = unitComponent.Health.ToString();

        // Initialize type indicators
        UpdateType();
        
    }

    bool czyKupic()
    {
        if(unit == nullObject)
            return false;
        if(unit.GetComponent<Unit>().RealCost > MoneyManager.money)
            return false;
        foreach(Pole pole in EventSystem.eventSystem.GetComponent<ShopManager>().lawka)
        {
            if(pole.unit == null)
            {
                return true;
            }
        }
        return false;
    }
    private bool showOpis;
    public void OnMouseEnter()
    {
        if(unit != nullObject)
        {
            showOpis = true;
            StartCoroutine(WaitDescription());
        }
        
    }
    public void OnMouseExit()
    {
        showOpis = false;
        DescriptionManager.opis.SetActive(false);
    }

    IEnumerator WaitDescription()
    {
        yield return new WaitForSeconds(0.2f);
        if(showOpis && unit != nullObject)
        {
            DescriptionManager.opis.SetActive(true);
            DescriptionManager.opis.GetComponent<DescriptionManager>().unit = unit.GetComponent<Unit>();
            showOpis = false;
        }
    }
}
