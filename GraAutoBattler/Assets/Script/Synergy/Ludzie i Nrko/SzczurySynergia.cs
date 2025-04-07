using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SzczurySynergia : Synergy
{
    public override void AfterBattle()
    {
        for (int i = 0; i < 3; i++)
        {
            Linia line = EventSystem.eventSystem.GetComponent<FightManager>().linie[i + (Enemy ? 3 : 0)];
            List<Unit> newUnits = new List<Unit>(units);
            foreach (var pole in line.pola)
            {
                foreach (Unit unit in units)
                {
                    if (pole.unit != null && unit.Name == pole.unit.GetComponent<Unit>().Name)
                    {
                        Unit unitToRemove = newUnits.Find(u => u.Name == pole.unit.GetComponent<Unit>().Name);

                        if (unitToRemove != null)
                        {
                            newUnits.Remove(unitToRemove);
                        }
                    }
                }
                
            }
            if (newUnits.Count == 0)
            {
                Buff(line.nr);
            }
        }

    }
    public GameObject szczur;
    public void Buff(int linenumber)
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
        GameObject newUnit = Instantiate(szczur, pos, Quaternion.identity);
        poleDocelowe.unit = newUnit;
        newUnit.GetComponent<DragObject>().pole = poleDocelowe;
    }
}
