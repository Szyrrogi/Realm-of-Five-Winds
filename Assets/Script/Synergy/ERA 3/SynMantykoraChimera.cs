using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynMantykoraChimera : Synergy
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
                        if(pole.unit.GetComponent<Unit>().Typy.Contains(Unit.CreatureType.Zwierzęta))
                        {
                            pole.unit.GetComponent<Unit>().Health += 30;
                            pole.unit.GetComponent<Unit>().MaxHealth += 30;
                            pole.unit.GetComponent<Unit>().Attack += 30;


                                    
                            pole.unit.GetComponent<Unit>().ShowPopUp("+30/30", Color.green);
                            yield return new WaitForSeconds(0.7f);
                        }
                    }
                }
                
            }
        }
        yield return null;

    }



}
