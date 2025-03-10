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
        if(poleDocelowe == null)
        {
            base.Evolve();
            return;
        }
        Vector3 pos = poleDocelowe.gameObject.transform.position;
        pos.z -= 2f;
        GameObject newUnit = Instantiate(unit, pos, Quaternion.identity);
        poleDocelowe.unit = newUnit;
        newUnit.GetComponent<DragObject>().pole = poleDocelowe;
        base.Evolve();
    }
}
