using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WładcaDrzew : Heros
{
    public GameObject summonMinion;
    public override IEnumerator OnBattleStart()
    {
        foreach(Unit unit in EventSystem.eventSystem.GetComponent<FightManager>().units)
        {
            if((unit.gameObject.GetComponent<Drzewo>()) && unit.Enemy == Enemy)
            {
                yield return StartCoroutine(unit.Summon(summonMinion));
            }
        }
        if(Evolution)
        {
            foreach(Unit unit in EventSystem.eventSystem.GetComponent<FightManager>().units)
            {
                if((unit.gameObject.GetComponent<Ent>()) && unit.Enemy == Enemy)
                {
                    unit.gameObject.GetComponent<Ent>().Defense += 15;
                }
            }
        }
        yield return null;
    }
}
//public IEnumerator Summon(GameObject summonMinion)