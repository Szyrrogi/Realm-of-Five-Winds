using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MartwyCzerw : Heros
{
    public static void IsGhul(Unit unit)
    {
        foreach(Pole pole in unit.gameObject.GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<MartwyCzerw>())
            {
                pole.unit.GetComponent<MartwyCzerw>().GhulBuff();

            }
        }
        foreach(Pole pole in unit.gameObject.GetComponent<DragObject>().pole.line.LineNext.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<MartwyCzerw>())
            {
                pole.unit.GetComponent<MartwyCzerw>().GhulBuff();

            }
        }
    }

    public override void Evolve()
    {
        Debug.Log("siema");
        Bestiariusz.AddAchivments(15);
        base.Evolve();
    }

    public void GhulBuff()
    {
        int buff = Evolution ? 20 : 10;
        Health += buff;
        MaxHealth += buff;
        Attack += buff;               
        ShowPopUp("+" + buff + "/" + buff, Color.green);
        //yield return new WaitForSeconds(0.7f);
    }
}
