using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wilk : Heros
{
    public Unit wilkolak;
    public Unit Alfa;
    public override IEnumerator OnBattleStart()
    {
        Attack -= 5;
        foreach(Unit unit in EventSystem.eventSystem.GetComponent<FightManager>().units)
        {
            if((unit.Name == Name || unit.Name == wilkolak.Name || unit.Name == Alfa.Name) && unit.Enemy == Enemy)
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
