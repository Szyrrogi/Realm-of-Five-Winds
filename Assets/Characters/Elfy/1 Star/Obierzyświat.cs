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

        int frakcja = fractionList.Count - 1;
        int ile = 0;
        if(frakcja == 1)
        {
            ile = 15;
        }
        if(frakcja >= 2)
        {
            ile = Evolution ? 75 : 30; 
        }
        
        Attack += ile;
        Health += ile;
        MaxHealth += ile;
        if(ile != 0)
            ShowPopUp("+" + (ile) + "/" + (ile), Color.green);
        yield return null;
        yield return null;
    }
}
