using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagiczneWzmocnienieRuniczne : Spell
{
    public override IEnumerator Action()
    {
        AP = unit.AP;
        foreach(Pole pole in unit.gameObject.GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Unit>().Enemy == unit.Enemy)
            {
                pole.unit.GetComponent<Unit>().AP += (int)(AP/10);
                pole.unit.GetComponent<Unit>().ShowPopUp(((int)(AP/10)).ToString(), new Color(0.5f, 0f, 1f));
            }
        }
        foreach(Pole pole in unit.gameObject.GetComponent<DragObject>().pole.line.LineNext.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Unit>().Enemy == unit.Enemy)
            {
                pole.unit.GetComponent<Unit>().AP += (int)(AP/10);
                pole.unit.GetComponent<Unit>().ShowPopUp(((int)(AP/10)).ToString(), new Color(0.5f, 0f, 1f));
            }
        }
        yield return new WaitForSeconds(0.7f / FightManager.GameSpeed);
        yield return null;
    }
}
