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
            if(pole.unit != null && pole.unit.GetComponent<Unit>().Name == Name)
            {
                i++;
            }
        }
        if(i == 3)
        {
            yield return new WaitForSeconds(0.4f );
            int buff = Evolution ? 80 : 30;
            ShowPopUp(Evolution ? "+80/80" : "+30/30", Color.green);
            Attack += buff;
            Health += buff;
            MaxHealth += buff;
        }
        yield return null;
    }
}
