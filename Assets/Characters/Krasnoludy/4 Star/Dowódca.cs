using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dowódca : Heros
{
    public override IEnumerator Action()    
    {
        yield return StartCoroutine(Fight());
        if(Evolution)
            yield return StartCoroutine(Fight());
        yield return null;
    }
}
