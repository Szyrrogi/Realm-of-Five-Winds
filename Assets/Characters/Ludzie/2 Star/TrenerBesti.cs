using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrenerBesti : Heros
{
     public CreatureType type;
    public override IEnumerator OnBattleStart()
    {


        Pole pole = findPole().GetComponent<Pole>();
        if (pole.unit != null && pole.unit.GetComponent<Unit>().Typy.Contains(type))
        {
            Unit unitt = pole.unit.GetComponent<Unit>();
            if (Evolution)
            {
                unitt.ShowPopUp("+30", Color.green);
                unitt.BoskaTarcza = true;
            }
            else
            {
                unitt.ShowPopUp("+30/-10", Color.green);
                unitt.Health -= 10;
                unitt.MaxHealth -= 10;
            }
            unitt.Attack += 30;
            yield return new WaitForSeconds(0.3f);
        }

        Unit unit = PrefUnit();
        if (unit != null && unit.Typy.Contains(type))
        {
            if (Evolution)
            {
                unit.ShowPopUp("+30", Color.green);
                unit.BoskaTarcza = true;
            }
            else
            {
                unit.ShowPopUp("+30/-10", Color.green);
                unit.Health -= 10;
                unit.MaxHealth -= 10;
            }
            unit.Attack += 30;
            yield return new WaitForSeconds(0.3f);
        }

        yield return null;
    }
}
