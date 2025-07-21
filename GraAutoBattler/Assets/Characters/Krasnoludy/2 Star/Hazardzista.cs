using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hazardzista : Heros
{
    public override IEnumerator OnBattleStart()
    {
        int RNG = UnityEngine.Random.Range(0, 2); 
        if (Evolution)
        {
            RNG = UnityEngine.Random.Range(0, 100);
            if (RNG > AP)
                RNG = 0;
        }
        if (!Enemy)
            {
                if (RNG == 0)
                    MoneyManager.money -= 1;
                else
                    MoneyManager.money += 2;
            }
        GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
        if (RNG == 0)
            pop.GetComponent<PopUp>().SetText("-1", Color.red);
        else
            pop.GetComponent<PopUp>().SetText("2", Color.yellow);
        yield return new WaitForSeconds(0.7f);
    }
    
        public override string DescriptionEdit()
    {
        if(Evolution)
            return "<b>Początek Walki:</b> Zyskaj 2 sztuki złota lub strać 1 sztukę złota (<color=#B803FF>" + (Math.Min(100,AP)) + "</color>% szan na zwycięstwo)"  ;
        else
            return Description; 
    }
}
