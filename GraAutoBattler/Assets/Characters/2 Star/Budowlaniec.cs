using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Budowlaniec : ShopVisitor
{
    public override void PostRoll()
    {
        for(int i = 0; i < 5; i++)
        {
            if(shop.character[i].unit.gameObject.GetComponent<Building>())
            {
                Debug.Log(shop.character[i].name);
                shop.character[i].unit.GetComponent<Unit>().RealCost = shop.character[i].unit.GetComponent<Unit>().Cost - 2;
                shop.character[i].price.text = (shop.character[i].unit.GetComponent<Unit>().RealCost.ToString());

            }
        }
    }

    public List<GameObject> budynki;

    public override void FirstRoll()
    {
        if(GetComponent<Heros>().Evolution)
        {
            int i =  Random.Range(0, 5);
            int rng = Random.Range(0, budynki.Count - 1);
            shop.character[i].image.sprite = budynki[rng].GetComponent<SpriteRenderer>().sprite;
            shop.character[i].name.text = budynki[rng].GetComponent<Unit>().Name;
            shop.character[i].price.text = budynki[rng].GetComponent<Unit>().Cost.ToString();
            shop.character[i].unit = budynki[rng];
        }
    }

    
}
