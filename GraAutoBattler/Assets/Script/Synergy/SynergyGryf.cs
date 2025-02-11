using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyGryf : Synergy
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
                }
            }
        }

    }

    public void Buff(int linenumber)
    {
        Linia line = EventSystem.eventSystem.GetComponent<FightManager>().linie[linenumber];
        bool jeden = false;
        bool dwa = false;
        foreach (var pole in line.pola)
        {
            if(!jeden && pole.unit != null && pole.unit.GetComponent<Unit>().Name == units[1].Name)
            {
                jeden = true;
                Destroy(pole.unit);
            }
            if (pole.unit != null && pole.unit.GetComponent<Unit>().Name == units[0].Name && !dwa)
            {
                dwa = true;

                Vector3 pos = pole.unit.transform.position;
                GameObject old = pole.unit;
                GameObject newUnit = Instantiate(EvolutionObject, pos, Quaternion.identity);
                
                newUnit.GetComponent<DragObject>().pole = pole.unit.GetComponent<DragObject>().pole;
                newUnit.GetComponent<DragObject>().pole.unit = newUnit;

                Destroy(old);
            }
        }
    }

}
