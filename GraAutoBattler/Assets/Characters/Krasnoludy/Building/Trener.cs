using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trener : Building
{
    public override IEnumerator OnBattleStart()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Heros>() && pole.unit.GetComponent<Heros>().Evolution)
            {
                pole.unit.GetComponent<Unit>().Morale();
                yield return new WaitForSeconds(0.4f);
            }
        }
        yield return null;
    }
}
