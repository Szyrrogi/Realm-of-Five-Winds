using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoombie : Heros
{
    public bool ult = true;
    public override IEnumerator Death()
    {
        
        if(ult)
            Health = Evolution ? MaxHealth : 1;
        else
            StartCoroutine(base.Death());
        ult = false;
        yield return null;
    }
}
