using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynTrenerPodatnik : Synergy
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
            if (pole.unit != null && pole.unit.GetComponent<Unit>().Name[0] == units[0].Name[0])
            {
                yield return new WaitForSeconds(0.3f);
                Unit unit = pole.unit.GetComponent<Unit>();
                if(!unit.Enemy)
                {
                    MoneyManager.money += 1;
                }
                unit.ShowPopUp("1", Color.yellow);
                yield return new WaitForSeconds(0.7f);
            }
        }
    }

}
