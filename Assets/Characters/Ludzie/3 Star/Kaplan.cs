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
            if(findPole().GetComponent<Pole>().unit != null)
            {
                findPole().GetComponent<Pole>().unit.GetComponent<Unit>().BoskaTarcza = true;
            }
        }
        yield return null;
    }
}
