using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kopalnia : ShopVisitor
{

    public GameObject zloto;
    public GameObject kopalnia;
    public bool Extra;


    public override List<GameObject> Filter(List<GameObject> prev)
    {
        if (!prev.Contains(zloto) || Extra)
            prev.Add(zloto);
        if (prev.Contains(kopalnia))
            prev.Remove(kopalnia);
        return prev;
    }
}
