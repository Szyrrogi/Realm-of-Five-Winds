using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyPiechur : Synergy
{
    public GameObject EvolutionObject;
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
                if (newUnits.Count == 0)
                {
                    Buff(line.nr);
                    break;
                }
            }
        }

    }

    public void Buff(int linenumber)
    {
        Linia line = EventSystem.eventSystem.GetComponent<FightManager>().linie[linenumber];
        foreach (var pole in line.pola)
        {
            if (pole.unit != null && pole.unit.GetComponent<Unit>().Name == units[0].Name)
            {
                Vector3 pos = pole.unit.transform.position;
                GameObject old = pole.unit;
                GameObject newUnit = Instantiate(EvolutionObject, pos, Quaternion.identity);
                
                newUnit.GetComponent<DragObject>().pole = pole.unit.GetComponent<DragObject>().pole;
                newUnit.GetComponent<DragObject>().pole.unit = newUnit;

                if(newUnit.GetComponent<Unit>().Attack < old.GetComponent<Unit>().Attack)
                    newUnit.GetComponent<Unit>().Attack = old.GetComponent<Unit>().Attack;
                if(newUnit.GetComponent<Unit>().AP < old.GetComponent<Unit>().AP)
                    newUnit.GetComponent<Unit>().AP = old.GetComponent<Unit>().AP;
                if(newUnit.GetComponent<Unit>().MaxHealth < old.GetComponent<Unit>().MaxHealth)
                {
                    newUnit.GetComponent<Unit>().MaxHealth = old.GetComponent<Unit>().MaxHealth;
                    newUnit.GetComponent<Unit>().Health = old.GetComponent<Unit>().Health;
                }

                Destroy(old);
            }
        }
    }

}
