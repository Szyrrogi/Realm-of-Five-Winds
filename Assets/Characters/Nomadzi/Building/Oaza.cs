using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oaza : Building
{
    public override IEnumerator OnBattleStart()
    {
        foreach(Unit unit in EventSystem.eventSystem.GetComponent<FightManager>().units)
        {
            if(unit.Enemy == Enemy)
            {
                unit.AP += 15;
            }
        }
        yield return null;
    }
}
