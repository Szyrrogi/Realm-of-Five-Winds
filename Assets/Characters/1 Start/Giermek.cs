using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giermek : Heros
{
    public GameObject unit;
    public override void Evolve()
    {
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
        newUnit.GetComponent<DragObject>().pole = poleDocelowe.gameObject;
        base.Evolve();
    }
}
