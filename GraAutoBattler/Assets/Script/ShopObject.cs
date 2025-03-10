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

    //  private bool isHolding = false;
    // private float holdTime = 0f;
    // private float holdThreshold = 0.1f; 
    // private bool holdActionTriggered = false;

    //  void Update()
    // {
    //     if (isHolding)
    //     {
    //         holdTime += Time.deltaTime;

    //         // Sprawdź, czy czas przytrzymania przekroczył próg i czy akcja przytrzymania nie została jeszcze wywołana
    //         if (holdTime >= holdThreshold && !holdActionTriggered)
    //         {
    //             HandleClick(true); // Wywołaj akcję przytrzymania
    //             holdActionTriggered = true; // Ustaw flagę, aby uniknąć ponownego wywołania
    //         }
    //     }
    // }

    // void OnMouseDown()
    // {
    //     isHolding = true;
    //     holdTime = 0f;
    //     holdActionTriggered = false; // Zresetuj flagę przy nowym kliknięciu
    // }

    // void OnMouseUp()
    // {
    //     if (isHolding)
    //     {
    //         if (holdTime < holdThreshold)
    //         {
    //             // Krótkie kliknięcie
    //             HandleClick(false);
    //         }
    //         // Jeśli przytrzymanie zostało już obsłużone w Update(), nie musimy nic robić
    //         isHolding = false;
    //     }
    // }

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
        // if(trzyma)
        //     newUnit.GetComponent<DragObject>().OnMouseDown();
    }

    public void SetStats()
    {
        stats.SetActive(true);
        atakText.text = unit.GetComponent<Unit>().attackAP ? unit.GetComponent<Unit>().AP.ToString() : unit.GetComponent<Unit>().Attack.ToString();
        healthText.text = unit.GetComponent<Unit>().Health.ToString();
        atakImage.color = unit.GetComponent<Unit>().attackAP ? new Color(0.5f, 0, 1f) : Color.yellow;
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
