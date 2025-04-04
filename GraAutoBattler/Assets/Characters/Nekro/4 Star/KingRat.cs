using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingRat : Heros
{
    public override IEnumerator OnBattleStart()
    {
        Attack -= 5;
        foreach(Unit unit in EventSystem.eventSystem.GetComponent<FightManager>().units)
        {
            if(unit != null && (unit.gameObject.GetComponent<Szczur>()) && unit.Enemy == Enemy)
            {
                int buff = 10;
                if(Evolution && unit.GetComponent<Heros>().Evolution)
                {
                    buff = 20;
                }
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
