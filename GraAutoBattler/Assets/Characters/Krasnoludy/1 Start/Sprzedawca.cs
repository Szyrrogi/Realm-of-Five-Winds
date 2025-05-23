using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprzedawca : Heros
{

    public ShopManager shop;

    void Start()
    {
        shop = EventSystem.eventSystem.GetComponent<ShopManager>();
        base.Start();
    }
    public override void AfterBuy()
    {
        RealCost += 1;
    }

    public override void UpgradeHeros(Unit newUnit)
    {
        RealCost-=1;
        base.UpgradeHeros(newUnit);
    }

    public override void Evolve()
    {
        for(int i = 0; i < 5; i++)
        {
                shop.character[i].unit.GetComponent<Unit>().RealCost = shop.character[i].unit.GetComponent<Unit>().Cost - 1;
                shop.character[i].price.text = (shop.character[i].unit.GetComponent<Unit>().RealCost.ToString());
        }
        RealCost += 1;
        base.Evolve();
    }
}
