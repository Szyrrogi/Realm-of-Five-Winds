using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KopalniaBudynek : Building
{
    public override void Sell()
    {
        Kopalnia.ShowZloto = false;
        MoneyManager.money += Cost;
        Destroy(gameObject);
    }
}
