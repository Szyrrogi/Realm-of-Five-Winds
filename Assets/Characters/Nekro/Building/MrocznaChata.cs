using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrocznaChata : Building
{
    public override void AfterBattle()
    {
        ShopManager.nizka++;
    }
}
