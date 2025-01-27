using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucznik : Heros
{

    public override IEnumerator Action()    
    {
        if(Evolution)
            yield return StartCoroutine(Fight());;
        yield return null;
    }
}
