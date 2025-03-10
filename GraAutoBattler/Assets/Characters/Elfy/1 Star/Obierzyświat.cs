using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obierzyświat : Heros
{
    public override IEnumerator OnBattleStart()
    {
        List<Fraction.fractionType> fractionList = new List<Fraction.fractionType>();
        foreach(Unit unit in EventSystem.eventSystem.GetComponent<FightManager>().units)
        {
            if(unit.Enemy == Enemy)
            {
                if(!fractionList.Contains(unit.fraction))
                {
                    fractionList.Add(unit.fraction);
                }
            }
        }

        int buff = Evolution ? 30 : 15;
        int ile = fractionList.Count - 1;
        Attack += buff * ile;
        Health += buff * ile;
        MaxHealth += buff * ile;
        if(ile != 0)
            ShowPopUp("+" + (buff * ile) + "/" + (buff * ile), Color.green);
        yield return null;
        yield return null;
    }
}
