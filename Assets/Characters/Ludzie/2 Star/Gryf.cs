using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gryf : Heros
{
    public override IEnumerator OnBattleStart()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null)
            {
                pole.unit.GetComponent<Unit>().Initiative += (Evolution ? 30 : 10);
            }
        }
        yield return null;
    }
}
