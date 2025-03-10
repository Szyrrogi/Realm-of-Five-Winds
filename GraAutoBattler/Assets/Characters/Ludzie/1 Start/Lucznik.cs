using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucznik : Heros
{
    void Update()
    {
        if(Health <= 0)
        {
            Skip = true;
        }
        base.Update();
    }
    public override IEnumerator Action()    
    {
        Debug.Log(Health);
        if(Evolution)
            yield return StartCoroutine(Fight());;
        yield return null;
    }
    public override IEnumerator Fight()
    {
        if(!Skip)
        {
            yield return StartCoroutine(base.Fight());
        }
    }
}
