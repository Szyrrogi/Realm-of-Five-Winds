using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BudowlaniecHero : Heros
{
    public override void Sell()
    {
        EventSystem.eventSystem.GetComponent<ShopManager>().shopVisitors.Remove(GetComponent<ShopVisitor>());
        base.Sell();
    }
}
