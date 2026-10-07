using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarsztatGolemow : ShopVisitor
{
    public GameObject golem;
    public GameObject DuzyGolem;
    public GameObject kopalnia;


    public override List<GameObject> Filter(List<GameObject> prev)
    {
        prev.Add(golem);
        if(StatsManager.Round >= 12)
        {
            // Tylko prawdziwy Warsztat (Wielki Golem ma klasę Golem). Hala Rekrutów używa tego skryptu z Rekrutem.
            int kopie = DuzyGolem.GetComponent<Golem>() ? HeroAbilities.WielkiGolemCopies() : 1;
            for (int k = 0; k < kopie; k++)
                prev.Add(DuzyGolem);
        }
        if(prev.Contains(kopalnia))
            prev.Remove(kopalnia);
        return prev;
    }
}
