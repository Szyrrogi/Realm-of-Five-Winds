using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynNekromanta : Synergy
{   
    public override IEnumerator BeforBattle()
    {
        for (int i = 0; i < 3; i++)
        {
            Linia line = EventSystem.eventSystem.GetComponent<FightManager>().linie[i + (Enemy ? 3 : 0)];
            List<Unit> newUnits = new List<Unit>(units);
            foreach (var pole in line.pola)
            {
                foreach (Unit unit in units)
                {
                    if (pole.unit != null && unit.Name[0] == pole.unit.GetComponent<Unit>().Name[0])
                    {
                        Unit unitToRemove = newUnits.Find(u => u.Name[0] == pole.unit.GetComponent<Unit>().Name[0]);

                        if (unitToRemove != null)
                        {
                            newUnits.Remove(unitToRemove);
                        }
                    }
                }
                
            }
            if (newUnits.Count == 0)
            {
                Debug.Log("czego1");
                Buff(line.nr);
            }
        }
        yield return null;

    }

    public void Buff(int linenumber)
    {
        Linia line = EventSystem.eventSystem.GetComponent<FightManager>().linie[linenumber];
        bool jeden = false;
        bool dwa = false;
        foreach (var pole in line.pola)
        {
            if(!jeden && pole.unit != null && pole.unit.GetComponent<Unit>().Name[0] == units[0].Name[0])
            {
                jeden = true;
                pole.unit.GetComponent<Nekromanta>().Synergy = true;
            }
        }
    }
    

}
