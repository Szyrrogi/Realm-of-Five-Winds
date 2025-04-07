using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GryfLucznik : Heros
{
    public override IEnumerator OnBattleStart()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null)
            {
                pole.unit.GetComponent<Unit>().Initiative += (Evolution ? 30 : 10);
            }
        }
        yield return null;
    }
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
