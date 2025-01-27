using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oltarz : Building
{
    public override IEnumerator OnBattleStart()
    {
        FightManager fight = EventSystem.eventSystem.GetComponent<FightManager>();
        if(fight.GetPole(GetComponent<DragObject>().pole.line.nr, 0).unit != null)
        {
            fight.GetPole(GetComponent<DragObject>().pole.line.nr, 0).unit.GetComponent<Unit>().Attack *= 2;
        }
        yield return null;
    }
}
