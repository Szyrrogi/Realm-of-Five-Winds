using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynZapasy : Synergy
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
                    yield return StartCoroutine(Buff());
                    break;
                }
            }
        }

        yield return null;
    }

    public IEnumerator Buff()
    {
        EventSystem.eventSystem.GetComponent<ShopManager>().FreeRoll += 1;
        yield return null;
    }

}
