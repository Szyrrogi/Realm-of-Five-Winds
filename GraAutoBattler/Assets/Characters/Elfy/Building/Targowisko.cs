using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targowisko : Building
{
    public override IEnumerator OnBattleEnd()
    {
        if(GetComponent<DragObject>().pole.line.KtoWygral == 1)
        {
            if(!Enemy)
            {
                MoneyManager.money += 1;
            }
            GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
            pop.GetComponent<PopUp>().SetText("1", Color.yellow);
            yield return new WaitForSeconds(0.7f );
        }
    }
}
