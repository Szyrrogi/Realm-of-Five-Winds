using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZagadkowyDuch : Heros
{
    public override void AfterBuy()
    {
        // Cost -= 2;
        // RealCost -= 2;
        for(int i = 0; i < 5; i++)
            {
                int rng = Random.Range(0, EventSystem.eventSystem.GetComponent<CharacterManager>().characters.Count);
                EventSystem.eventSystem.GetComponent<ShopManager>().character[i].unit = EventSystem.eventSystem.GetComponent<CharacterManager>().characters[rng];
                EventSystem.eventSystem.GetComponent<ShopManager>().character[i].SetLook();
            }
    }
    public override void Evolve()
    {
        AfterBuy();
        base.Evolve();
    }
}
