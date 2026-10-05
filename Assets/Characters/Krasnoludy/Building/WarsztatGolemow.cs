using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarsztatGolemow : ShopVisitor
{
    public GameObject golem;
    public GameObject DuzyGolem;
    public GameObject kopalnia;


    public override List<GameObject> Filter(List<GameObject> prev)
    {
        prev.Add(golem);
        if(StatsManager.Round >= 12)
        {
            prev.Add(DuzyGolem);
        }
        if(prev.Contains(kopalnia))
            prev.Remove(kopalnia);
        return prev;
    }
}
