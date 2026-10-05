using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghul : Heros
{
    public static void IsGhul(Unit unit)
    {
        foreach(Pole pole in unit.gameObject.GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Ghul>() && unit.Enemy == pole.unit.GetComponent<Unit>().Enemy)
            {
                pole.unit.GetComponent<Ghul>().GhulBuff();

            }
        }
        foreach(Pole pole in unit.gameObject.GetComponent<DragObject>().pole.line.LineNext.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Ghul>() && unit.Enemy == pole.unit.GetComponent<Unit>().Enemy)
            {
                pole.unit.GetComponent<Ghul>().GhulBuff();

            }
        }
    }

    public void GhulBuff()
    {
        int buff = Evolution ? 22 : 12;
        Health += buff;
        MaxHealth += buff;
        Attack += buff;               
        ShowPopUp("+" + buff + "/" + buff, Color.green);
        //yield return new WaitForSeconds(0.7f);
    }
}
