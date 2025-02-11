using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnielskaSwiatynia : ShopVisitor
{
    public List<GameObject> anily;

    public override void FirstRoll()
    {
        int i =  Random.Range(0, 5);
        int rng = Random.Range(0, anily.Count - 1);
        shop.character[i].image.sprite = anily[rng].GetComponent<SpriteRenderer>().sprite;
        shop.character[i].name.text = anily[rng].GetComponent<Unit>().Name;
        shop.character[i].price.text = anily[rng].GetComponent<Unit>().Cost.ToString();
        shop.character[i].unit = anily[rng];
    }
}
