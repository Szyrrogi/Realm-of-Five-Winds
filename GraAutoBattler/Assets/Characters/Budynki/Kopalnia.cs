using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kopalnia : ShopVisitor
{

    public GameObject zloto;


    public override List<GameObject> Filter(List<GameObject> prev)
    {
        prev.Add(zloto);
        return prev;
    }
}
