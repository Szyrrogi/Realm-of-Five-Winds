using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpadlyKsiaze : Heros
{
    public override IEnumerator OnBattleStart()
    {
        if (Evolution)
        {
            int buff = MoneyManager.money * 5;

            MaxHealth += buff;
            Health += buff;
            Attack += buff;
            ShowPopUp(buff.ToString() + " " + buff.ToString(), Color.green);
        }

        yield return null;
    }
}
