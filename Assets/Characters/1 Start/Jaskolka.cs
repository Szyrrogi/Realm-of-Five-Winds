using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jaskolka : Heros
{
    public override IEnumerator OnBattleStart()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null)
            {
                pole.unit.GetComponent<Unit>().AP += 10;
            }
        }
        yield return null;
    }

    public override void Evolve()
    {
        GameObject newUnitObject = Instantiate(EvolveHeroes, gameObject.transform.position, Quaternion.identity);
        Heros newUnit = newUnitObject.GetComponent<Heros>();

        newUnit.Cost = Cost;

        newUnit.Evolution = true;

        GetComponent<DragObject>().pole.unit = newUnitObject;
        GetComponent<DragObject>().pole.Start();
        Destroy(gameObject);
        
    }
}
