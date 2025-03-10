using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwierdzaŻelaznegoOporu : Building
{
    public override IEnumerator OnBattleStart()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Heros>())
            {
                pole.unit.GetComponent<Unit>().Defense += 10;
                pole.unit.GetComponent<Unit>().ShowPopUp("+10", new Color(0.33f, 0.33f, 0.33f));
                yield return new WaitForSeconds(0.5f);
            }
        }
        yield return null;
    }
}
