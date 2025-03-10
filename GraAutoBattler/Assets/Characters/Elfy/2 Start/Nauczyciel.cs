using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nauczyciel : Heros
{
    public override IEnumerator OnBattleStart()
    {
        Pole pole = findPole().GetComponent<Pole>();
        if(pole.unit != null)
        {
            Unit unitt = pole.unit.GetComponent<Unit>();
            unitt.Range = 1;
            unitt.ShowPopUp("+Zasięg", Color.green);
            yield return new WaitForSeconds(0.3f);
        }
        Unit unit = PrefUnit();
        if(unit != null)
        {
            unit.Range = 1;
            unit.ShowPopUp("+Zasięg", Color.green);
            yield return new WaitForSeconds(0.3f);
        }
        yield return null;
    }
}
