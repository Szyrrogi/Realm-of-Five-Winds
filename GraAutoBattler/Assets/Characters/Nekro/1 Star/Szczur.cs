using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Szczur : Heros
{
    public GameObject szczur;
    public override IEnumerator Death()
    {
        if(Evolution)
            yield return Summon(szczur); 
        else
            StartCoroutine(base.Death());
        yield return null;
    }
}
