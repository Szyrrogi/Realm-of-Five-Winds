using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabaż : Heros
{
    public List<GameObject> trupy;
    public override void AfterBattle()
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
        if(poleDocelowe == null)
        {
            return;
        }
        Vector3 pos = poleDocelowe.gameObject.transform.position;
        pos.z -= 2f;
        GameObject newUnit = Instantiate(trupy[Random.Range(0,trupy.Count)], pos, Quaternion.identity);
        poleDocelowe.unit = newUnit; // Przypisanie jednostki do pola
        newUnit.GetComponent<DragObject>().pole = poleDocelowe;
        newUnit.GetComponent<Unit>().Cost = 1;
        if(Evolution)
        {
            newUnit.GetComponent<Heros>().Evolve();
        }
    }
}
