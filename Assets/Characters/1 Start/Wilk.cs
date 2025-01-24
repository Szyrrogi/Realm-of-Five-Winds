using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wilk : Heros
{
    public override IEnumerator OnBattleStart()
    {
        foreach(Unit unit in EventSystem.eventSystem.GetComponent<FightManager>().units)
        {
            if(unit.Name == Name && !unit.Enemy)
            {
                if(Evolution)
                    Attack += 10;
                else
                    Attack += 5;

            }
        }
        yield return null;
    }
}
