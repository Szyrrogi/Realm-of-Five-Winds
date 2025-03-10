using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynOltarz : Synergy
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
                
            }
            if (newUnits.Count == 0)
            {
                Debug.Log("czego1");
                yield return StartCoroutine(Buff(line.nr));
            }
        }
        yield return null;

    }

    public IEnumerator Buff(int linenumber)
    {
        Linia line = EventSystem.eventSystem.GetComponent<FightManager>().linie[linenumber];
        bool jeden = false;
        Debug.Log("czego3");
        bool dwa = false;
        foreach (var pole in line.pola)
        {
            if(!jeden && pole.unit != null && pole.unit.GetComponent<Unit>().Name == units[0].Name)
            {
                jeden = true;
                yield return StartCoroutine(pole.unit.GetComponent<MrocznyOltarz>().OnBattleStartExtra());
            }
        }
        yield return null;
    }
    

}
