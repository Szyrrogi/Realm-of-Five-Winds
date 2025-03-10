using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feniks : Heros
{
    public int ult;
    public override IEnumerator Death()
    {
        
        if(ult > 0)
        {
            Health = MaxHealth;
            ult--;
        }
        else
            StartCoroutine(base.Death());
        yield return null;
    }
}
