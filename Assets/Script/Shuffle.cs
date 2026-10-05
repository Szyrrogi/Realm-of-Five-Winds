using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuffle : ShopVisitor
{
    public List <GameObject> zaklecia;


    public override List<GameObject> Filter(List<GameObject> prev)
    {
        foreach(GameObject czar in zaklecia)
        {
            if(czar.GetComponent<Unit>().Star <= (StatsManager.Round / 3) + 1)
            {
                prev.Add(czar);
            }
        }
        return prev;
    }
}
