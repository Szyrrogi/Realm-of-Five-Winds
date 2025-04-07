using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuchLasu : Heros
{
    public override IEnumerator OnBattleStart()
    {
        foreach(Unit unit in EventSystem.eventSystem.GetComponent<FightManager>().units)
        {
            if(unit.Cost < Cost && unit.Enemy == Enemy)
            {

                int buff = Evolution ? 10 : 7;
                Attack += buff;
                Health += buff;
                MaxHealth += buff;
                ShowPopUp("+" + (buff) + "/" + (buff), Color.green);
                yield return new WaitForSeconds(0.2f);
            }
        }
        yield return null;
    }
}
