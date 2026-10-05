using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PustynnyPrzewodnik : Heros
{
    public override IEnumerator OnBattleStart()
    {
        int i = 0;
        foreach(Pole pole in GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Unit>().Name[0] == Name[0])
            {
                i++;
            }
        }
        if(i == 3)
        {
            yield return new WaitForSeconds(0.4f );
            int buff = Evolution ? 90 : 40;
            ShowPopUp(Evolution ? "+90/90" : "+40/40", Color.green);
            Attack += buff;
            Health += buff;
            MaxHealth += buff;
        }
        yield return null;
    }
}
