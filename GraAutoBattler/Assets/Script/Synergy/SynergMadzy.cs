using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergMadzy : Synergy
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
                    yield return StartCoroutine(Buff(line.nr));
                    break;
                }
            }
        }

        yield return null;
    }

    public IEnumerator Buff(int linenumber)
    {
        Linia line = EventSystem.eventSystem.GetComponent<FightManager>().linie[linenumber];
        foreach (var pole in line.pola)
        {
            for(int i = 0; i < 3 ; i++)
            {
                if (pole.unit != null && pole.unit.GetComponent<Unit>().Name == units[i].Name)
                {
                    yield return new WaitForSeconds(0.3f);
                    pole.unit.GetComponent<Unit>().AP += 20;
                    pole.unit.GetComponent<Unit>().ShowPopUp("20", new Color(0.5f, 0, 1f));
                }
            }
        }
    }

}
