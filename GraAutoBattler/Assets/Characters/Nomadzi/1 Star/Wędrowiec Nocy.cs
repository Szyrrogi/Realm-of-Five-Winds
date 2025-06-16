using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WędrowiecNocy : Heros
{
    public override IEnumerator OnBattleStart()
    {
        int line = gameObject.GetComponent<DragObject>().pole.line.nr;
        int pole = gameObject.GetComponent<DragObject>().pole.nr;
        FightManager fightManager = EventSystem.eventSystem.GetComponent<FightManager>();
        if(fightManager.GetPole(line, pole - 1) != null && fightManager.GetPole(line, pole - 1).unit != null)
        {
            if(fightManager.GetPole(line, pole + 1) != null && fightManager.GetPole(line, pole + 1).unit != null)
                if(fightManager.GetPole(line, pole - 1).unit.GetComponent<Unit>().attackAP != fightManager.GetPole(line, pole + 1).unit.GetComponent<Unit>().attackAP)
                {
                    yield return new WaitForSeconds(0.4f );
                    int buff = Evolution ? 50 : 20;
                    ShowPopUp(Evolution ? "+50/50" : "+20/20", Color.green);
                    Attack += buff;
                    Health += buff;
                    MaxHealth += buff;
                }
        }
    }
}
