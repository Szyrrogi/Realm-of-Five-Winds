using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprzedawca : Heros
{
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
        RealCost += 1;
        base.Evolve();
    }
}
