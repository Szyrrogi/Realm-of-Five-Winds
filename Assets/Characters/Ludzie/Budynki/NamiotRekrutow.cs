using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamiotRekrutow : Building
{
    public override void AfterBattle()
    {
        EventSystem.eventSystem.GetComponent<ShopManager>().FreeRoll += 2;
    }

    
}
