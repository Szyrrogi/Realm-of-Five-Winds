using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MartwyLucznik : Heros
{
    public override void Evolve()
    {
        GameObject newUnitObject = Instantiate(EvolveHeroes, gameObject.transform.position, Quaternion.identity);
        Heros newUnit = newUnitObject.GetComponent<Heros>();

        newUnit.Cost = Cost;
        newUnit.Initiative = Initiative;
        newUnit.Health = Health;
        newUnit.MaxHealth = MaxHealth;
        newUnit.Attack = Attack + 30;
        newUnit.Defense = Defense;
        newUnit.AP = AP;
        newUnit.MagicResist = MagicResist;

        newUnit.UpgradeLevel = 0;
        newUnit.UpgradeNeed = 0;
        newUnit.RealCost = 0;
        newUnit.Evolution = true;

        GetComponent<DragObject>().pole.unit = newUnitObject;
        GetComponent<DragObject>().pole.Start();
        Destroy(gameObject);
    }
}
