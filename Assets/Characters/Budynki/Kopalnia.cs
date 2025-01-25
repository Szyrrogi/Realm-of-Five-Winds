using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kopalnia : ShopVisitor
{
    public static bool ShowZloto;

    public GameObject zloto;
    void Update()
    {
        if(GetComponent<DragObject>().pole.line != null)
        {
            if(!EventSystem.eventSystem.GetComponent<ShopManager>().shopVisitors.Contains(this))
                ShowZloto = true;
        }
        else
        {
            if(EventSystem.eventSystem.GetComponent<ShopManager>().shopVisitors.Contains(this))
            {
                ShowZloto = false;
            }
        }
        base.Update();
    }

    public override List<GameObject> Filter(List<GameObject> prev)
    {
        prev.Add(zloto);
        return prev;
    }
}
