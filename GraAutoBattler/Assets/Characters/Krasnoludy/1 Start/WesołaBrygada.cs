using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WesołaBrygada : Heros
{
    public override void Evolve()
    {
        GameObject newUnitObject = Instantiate(EvolveHeroes, gameObject.transform.position, Quaternion.identity);
        Heros newUnit = newUnitObject.GetComponent<Heros>();

        newUnit.Cost = Cost;

        newUnit.Initiative = Initiative;
        newUnit.Health = Health;
        newUnit.MaxHealth = MaxHealth;
        newUnit.Attack = Attack;
        newUnit.Defense = Defense + 15;
        newUnit.AP = AP;
        newUnit.MagicResist = MagicResist;

        newUnit.UpgradeLevel = 1;
        newUnit.UpgradeNeed = 2;
        newUnit.RealCost = 0;
        newUnit.Evolution = false;

        GetComponent<DragObject>().pole.unit = newUnitObject;
        GetComponent<DragObject>().pole.Start();
        Destroy(gameObject);
    }
}
