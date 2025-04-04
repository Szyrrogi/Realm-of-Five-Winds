using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilkAlfa : Heros
{
    public override IEnumerator OnBattleStart()
    {
        int buff = Evolution ? 24 : 15;
        Attack -= buff;
        Health -= buff;
        MaxHealth -= buff;
        foreach(Unit unit in EventSystem.eventSystem.GetComponent<FightManager>().units)
        {
            if((unit.gameObject.GetComponent<Wilk>() || unit.gameObject.GetComponent<WilkAlfa>() || unit.gameObject.GetComponent<Wilkolak>()) && unit.Enemy == Enemy)
            {
                
                Health += buff;
                Attack += buff;
                MaxHealth += buff;
                
            }
        }
        yield return null;
    }
}
