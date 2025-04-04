using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodatnikPro : Heros
{
    public override IEnumerator OnBattleStart()
    {
        if(!Enemy)
        {
            MoneyManager.money += 2;
        }
        GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
        pop.GetComponent<PopUp>().SetText("2", Color.yellow);
        yield return new WaitForSeconds(0.7f);
    }
}