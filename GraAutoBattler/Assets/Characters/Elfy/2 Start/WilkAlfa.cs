using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilkAlfa : Heros
{
    public override IEnumerator OnBattleStart()
    {
        Attack -= 12;
        Health -= 12;
        MaxHealth -= 12;
        foreach(Unit unit in EventSystem.eventSystem.GetComponent<FightManager>().units)
        {
            if((unit.gameObject.GetComponent<Wilk>() || unit.gameObject.GetComponent<WilkAlfa>() || unit.gameObject.GetComponent<Wilkolak>()) && unit.Enemy == Enemy)
            {
                if(Evolution)
                {
                    Health += 20;
                    Attack += 20;
                    MaxHealth += 20;
                }
                else
                {
                    Health += 12;
                    Attack += 12;
                    MaxHealth += 12;
                }

            }
        }
        yield return null;
    }
}
