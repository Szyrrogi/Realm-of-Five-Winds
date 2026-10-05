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
            if (fightManager.GetPole(line, pole - 1) != null && fightManager.GetPole(line, pole - 1).unit != null)
            {
                if (fightManager.GetPole(line, pole + 1) != null && fightManager.GetPole(line, pole + 1).unit != null)
                {
                    Unit unitLeft = fightManager.GetPole(line, pole - 1).unit.GetComponent<Unit>();
                    Unit unitRight = fightManager.GetPole(line, pole + 1).unit.GetComponent<Unit>();
                    
                    Unit withAP = (unitLeft.attackAP == true) ? unitLeft : unitRight;
                    Unit withoutAP = (unitLeft.attackAP == true) ? unitRight : unitLeft;

                    Debug.Log(withAP.name);
                    Debug.Log(withoutAP.name);

                    yield return new WaitForSeconds(0.4f);
                    Attack += withoutAP.Attack;
                    if (Evolution)
                    {
                        Health += withAP.AP;
                        MaxHealth += withAP.AP;
                    }
                    else
                    {
                    }
                }


                
            }
        }
    }
}
