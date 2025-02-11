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
        yield return new WaitForSeconds(0.2f / FightManager.GameSpeed);
        if(showOpis && unit != nullObject)
        {
            DescriptionManager.opis.SetActive(true);
            DescriptionManager.opis.GetComponent<DescriptionManager>().unit = unit.GetComponent<Unit>();
            showOpis = false;
        }
    }
}
