using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargulec2 : ShopVisitor
{
    public override void PostRoll()
    {
        gameObject.GetComponent<Unit>().Attack += gameObject.GetComponent<Heros>().Evolution ? 10 : 5;
        gameObject.GetComponent<Unit>().Health += gameObject.GetComponent<Heros>().Evolution ? 10 : 5;
        gameObject.GetComponent<Unit>().MaxHealth += gameObject.GetComponent<Heros>().Evolution ? 10 : 5;
    }
}
