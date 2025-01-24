using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kaplan : Heros
{
    public override IEnumerator OnBattleStart()
    {
        if(Evolution)
        {
            foreach(Pole pole in GetComponent<DragObject>().pole.GetComponent<Pole>().line.GetComponent<Linia>().pola)
            {
                if(pole.unit != null)
                {
                    pole.unit.GetComponent<Unit>().BoskaTarcza = true;
                }
            }

        }
        else
        {
            FightManager fight = EventSystem.eventSystem.GetComponent<FightManager>();
            Debug.Log("aaaa" + GetComponent<DragObject>().pole.GetComponent<Pole>().line.GetComponent<Linia>().nr);
            if(fight.GetPole(GetComponent<DragObject>().pole.GetComponent<Pole>().line.GetComponent<Linia>().nr, 0).unit != null)
            {
                fight.GetPole(GetComponent<DragObject>().pole.GetComponent<Pole>().line.GetComponent<Linia>().nr, 0).unit.GetComponent<Unit>().BoskaTarcza = true;
            }
        }
        yield return null;
    }
}
