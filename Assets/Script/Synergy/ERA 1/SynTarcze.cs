using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynTarcze : Synergy
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
                    if (pole.unit != null)
                    {
                        pole.unit.GetComponent<Unit>().BoskaTarcza = true;
                    }
                }
                
            }
        }
        yield return null;

    }



}
