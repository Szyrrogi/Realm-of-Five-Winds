using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyMiecz : Synergy
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
            if (pole.unit != null && pole.unit.GetComponent<Unit>().Name == units[0].Name)
            {
                yield return new WaitForSeconds(0.3f / FightManager.GameSpeed);
                pole.unit.GetComponent<Heros>().attackAP = true;
                pole.unit.GetComponent<Unit>().AP += pole.unit.GetComponent<Unit>().Attack + 5;
                pole.unit.GetComponent<Unit>().ShowPopUp((pole.unit.GetComponent<Unit>().Attack + 5).ToString(), Color.blue);
                
            }
        }
    }

}
