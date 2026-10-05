using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tragarz : Heros
{
    public override void AfterBuy()
    {
        EventSystem.eventSystem.GetComponent<ShopManager>().FreeRoll += 1;
    }

    public override void Evolve()
    {
        EventSystem.eventSystem.GetComponent<ShopManager>().FreeRoll += 1;
        base.Evolve();
    }
}
