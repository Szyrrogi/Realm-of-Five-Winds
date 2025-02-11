using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kaplan : Heros
{
    public override IEnumerator OnBattleStart()
    {
        if(Evolution)
        {
            foreach(Pole pole in GetComponent<DragObject>().pole.line.pola)
            {
                if(pole.unit != null)
                {
                    pole.unit.GetComponent<Unit>().BoskaTarcza = true;
                }
            }

        }
        else
        {
            // FightManager fight = EventSystem.eventSystem.GetComponent<FightManager>();
            // Debug.Log("aaaa" + GetComponent<DragObject>().pole.line.nr);
            // if(fight.GetPole(GetComponent<DragObject>().pole.line.nr, 0).unit != null)
            // {
            //     fight.GetPole(GetComponent<DragObject>().pole.line.nr, 0).unit.GetComponent<Unit>().BoskaTarcza = true;
            // }
            if(findPole().GetComponent<Pole>().unit != null)
            {
                findPole().GetComponent<Pole>().unit.GetComponent<Unit>().BoskaTarcza = true;
            }
        }
        yield return null;
    }
}
