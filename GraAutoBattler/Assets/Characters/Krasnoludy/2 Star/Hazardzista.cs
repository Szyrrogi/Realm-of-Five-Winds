using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazardzista : Heros
{
    public override IEnumerator OnBattleStart()
    {
        int RNG = Random.Range(0, 2);
        if(Evolution)
            RNG = Random.Range(0, 3);
        if (!Enemy)
            {
                if (RNG == 0)
                    MoneyManager.money -= 2;
                else
                    MoneyManager.money += 2;
            }
        GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
        if(RNG == 0)
            pop.GetComponent<PopUp>().SetText("-2", Color.red);
        else
            pop.GetComponent<PopUp>().SetText("2", Color.yellow);
        yield return new WaitForSeconds(0.7f);
    }
}
