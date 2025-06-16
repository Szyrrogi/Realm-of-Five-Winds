using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Architekt : Heros
{
    public override IEnumerator OnBattleStart()
    {
        if(Evolution)
        {
            foreach(Unit unit in EventSystem.eventSystem.GetComponent<FightManager>().units)
            {
                if (unit != null && unit.gameObject != null)
                {
                    Building building = unit.GetComponent<Building>();
                    if (building != null && unit.Enemy == Enemy)
                    {
                        yield return StartCoroutine(building.OnBattleStart());
                        yield return new WaitForSeconds(0.2f);
                    }
                }
            }
        }
        else
        {
            foreach(Pole pole in GetComponent<DragObject>().pole.line.pola)
            {
                if(pole.unit != null && pole.unit.GetComponent<Building>())
                {
                    yield return StartCoroutine(pole.unit.GetComponent<Building>().OnBattleStart());
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }
        yield return null;
    }
}
