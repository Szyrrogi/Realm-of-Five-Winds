using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kapliczka : Building
{
    public override void AfterBattle()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.GetComponent<Pole>().line.GetComponent<Linia>().pola)
        {
            if(pole.unit != null)
            {
                pole.unit.GetComponent<Unit>().ShowPopUp("+1", Color.white);
                pole.unit.GetComponent<Unit>().UpgradeLevel++;
                if(!pole.unit.GetComponent<Heros>().Evolution && pole.unit.GetComponent<Unit>().UpgradeLevel >= pole.unit.GetComponent<Unit>().UpgradeNeed)
                {
                    pole.unit.GetComponent<Heros>().Evolve();
                }
                break;
            }
        }
    }
}
