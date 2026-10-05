using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FreeSpell : ShopVisitor
{
    public override void PostRoll()
    {
        for(int i = 0; i < 5; i++)
        {
            if(shop.character[i].unit.gameObject.GetComponent<Spell>())
            {
                Debug.Log(shop.character[i].name);
                shop.character[i].unit.GetComponent<Unit>().RealCost = 0;
                shop.character[i].price.text = (shop.character[i].unit.GetComponent<Unit>().RealCost.ToString());

            }
        }
    }

}
