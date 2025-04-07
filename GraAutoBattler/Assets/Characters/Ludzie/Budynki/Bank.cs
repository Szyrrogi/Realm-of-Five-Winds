using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : Building
{
    public override IEnumerator OnBattleStart()
    {
        if(!Enemy)
            {

            int bonus = (int)(MoneyManager.money * 0.25);
            MoneyManager.money += bonus;
            
            GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
            pop.GetComponent<PopUp>().SetText(bonus.ToString(), Color.yellow);
            yield return new WaitForSeconds(0.7f );
            }
    }
}
