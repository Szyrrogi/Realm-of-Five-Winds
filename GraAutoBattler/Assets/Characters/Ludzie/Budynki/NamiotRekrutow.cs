using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamiotRekrutow : ShopVisitor
{
    public override void FirstRoll()
    {
        shop.FreeRoll += 2;
    }
}
