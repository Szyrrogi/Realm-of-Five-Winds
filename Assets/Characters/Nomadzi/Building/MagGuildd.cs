using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagGuildd : ShopVisitor
{

    public List<GameObject> spells;
    public GameObject kopalnia;


    public override List<GameObject> Filter(List<GameObject> prev)
    {
        foreach(GameObject obj in spells)
        {
        if(!prev.Contains(obj) && obj.GetComponent<Unit>().Star <= (StatsManager.Round / 3) + 1)
            prev.Add(obj);
        }
        if(prev.Contains(kopalnia))
            prev.Remove(kopalnia);
        return prev;
    }
}
