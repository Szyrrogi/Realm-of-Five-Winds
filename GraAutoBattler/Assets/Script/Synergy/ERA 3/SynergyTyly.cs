using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyTyly : Synergy
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
            if (pole.unit != null)
            {
                yield return new WaitForSeconds(0.3f);
                if(pole.unit.GetComponent<StrażnikZapasów>())
                {
                    pole.unit.GetComponent<StrażnikZapasów>().Synergy = true;
                }  
                if(pole.unit.GetComponent<SzamanPiasków>())
                {
                    pole.unit.GetComponent<SzamanPiasków>().Synergy = true;
                }
            }
        }
    }

}
