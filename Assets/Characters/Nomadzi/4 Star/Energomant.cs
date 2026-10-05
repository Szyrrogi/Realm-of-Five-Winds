using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energomant : Heros
{
    public override IEnumerator OnBattleStart()
    {
        int line = gameObject.GetComponent<DragObject>().pole.line.nr;
        int pole = gameObject.GetComponent<DragObject>().pole.nr;
        FightManager fightManager = EventSystem.eventSystem.GetComponent<FightManager>();
        if(fightManager.GetPole(line, pole - 1) != null && fightManager.GetPole(line, pole - 1).unit != null)
        {
            if(fightManager.GetPole(line, pole + 1) != null && fightManager.GetPole(line, pole + 1).unit != null)
                {
                    yield return new WaitForSeconds(0.4f );
                    fightManager.GetPole(line, pole - 1).unit.GetComponent<Unit>().ShowPopUp(fightManager.GetPole(line, pole + 1).unit.GetComponent<Unit>().AP.ToString(), Color.green);
                    fightManager.GetPole(line, pole - 1).unit.GetComponent<Unit>().AP += fightManager.GetPole(line, pole + 1).unit.GetComponent<Unit>().AP;

                    if(Evolution)
                    {
                       yield return new WaitForSeconds(0.4f );
                        fightManager.GetPole(line, pole + 1).unit.GetComponent<Unit>().ShowPopUp(fightManager.GetPole(line, pole - 1).unit.GetComponent<Unit>().AP.ToString(), Color.green);
                        fightManager.GetPole(line, pole + 1).unit.GetComponent<Unit>().AP += fightManager.GetPole(line, pole - 1).unit.GetComponent<Unit>().AP; 
                    }
                }
        }
    }
}
