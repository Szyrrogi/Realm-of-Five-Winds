using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jajo : Heros
{
    public override void AfterBattle()
    {
        Evolve();
    }

    public override void Evolve()
    {
        GameObject newUnitObject = Instantiate(EvolveHeroes, gameObject.transform.position, Quaternion.identity);
        Heros newUnit = newUnitObject.GetComponent<Heros>();
        newUnit.Cost = 2;

        GetComponent<DragObject>().pole.unit = newUnitObject;
        GetComponent<DragObject>().pole.Start();

        Destroy(gameObject);
    }
}
