using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargulec2 : ShopVisitor
{
    public int maks = 4;
    void Start()
    {
        maks = 4;
    }
    public override void PostRoll()
    {
        if(maks > 0)
        {
        gameObject.GetComponent<Unit>().Attack += gameObject.GetComponent<Heros>().Evolution ? 10 : 5;
        gameObject.GetComponent<Unit>().Health += gameObject.GetComponent<Heros>().Evolution ? 10 : 5;
        gameObject.GetComponent<Unit>().MaxHealth += gameObject.GetComponent<Heros>().Evolution ? 10 : 5;
        maks--;
        }
    }
}
