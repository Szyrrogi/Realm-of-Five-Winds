using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpadlyKsiaze : Heros
{
    public override IEnumerator OnBattleStart()
    {
        int buff = Evolution ? 2 : 1;
        buff *= MoneyManager.money;
        foreach (Unit unit in EventSystem.eventSystem.GetComponent<FightManager>().units)
        {
            if (unit.Enemy == Enemy)
            {
                unit.MaxHealth += buff;
                unit.Health += buff;
                unit.Attack += buff;
                unit.ShowPopUp(buff.ToString() + " " + buff.ToString(), Color.green);

            }
        }
        yield return null;
    }
}
