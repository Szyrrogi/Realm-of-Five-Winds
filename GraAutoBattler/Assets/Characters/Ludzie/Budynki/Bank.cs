using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : Building
{
    public override void AfterBuy()
    {
        Cost -= 4;
        RealCost -= 4;
    }
    public override IEnumerator OnBattleStart()
    {
        if(!Enemy)
        {

            int bonus = Health;
            MoneyManager.money += bonus;
            
            GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
            pop.GetComponent<PopUp>().SetText(bonus.ToString(), Color.yellow);
            yield return new WaitForSeconds(0.7f );
        }
    }

    public override void AfterBattle()
    {
        if(Health < 4)
        {
            Health += 1;
            MaxHealth += 1;
        }
    }

}
