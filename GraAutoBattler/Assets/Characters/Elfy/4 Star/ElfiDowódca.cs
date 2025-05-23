using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfiDowódca : Heros
{
    public override IEnumerator OnBattleStart()
    {
        if(Evolution)
        {
            foreach(Unit unit in EventSystem.eventSystem.GetComponent<FightManager>().units)
                {
                    if(unit != null && unit.Range > 0 && unit.Enemy == Enemy && unit.gameObject.GetComponent<Heros>())
                    {
                        unit.Attack += AP;
                        unit.ShowPopUp(AP.ToString(), Color.green);
                        yield return new WaitForSeconds(0.5f );
                    }
                }
        }
        else
        {
            foreach(Pole pole in GetComponent<DragObject>().pole.line.pola)
            {
                if(pole.unit != null && pole.unit.GetComponent<Unit>().Range > 0 && pole.unit.GetComponent<Unit>().Enemy == Enemy && pole.unit.GetComponent<Heros>())
                {
                    pole.unit.GetComponent<Unit>().Attack += AP;
                    pole.unit.GetComponent<Unit>().ShowPopUp(AP.ToString(), Color.green);
                    yield return new WaitForSeconds(0.5f );
                }
            }
        }
        yield return null;
    }

    public override string DescriptionEdit()
    {
        if(!Evolution)
            return "<b>Początek Walki: </b>Zwiększ atak jednsotkom dystansowym w tym rzędzie o <color=#B803FF>" + (AP) + "</color>"  ;
        else
            return "<b>Początek Walki: </b>Zwiększ atak jednsotkom dystansowym na arenie o <color=#B803FF>" + (AP) + "</color>"; 
    }
}
