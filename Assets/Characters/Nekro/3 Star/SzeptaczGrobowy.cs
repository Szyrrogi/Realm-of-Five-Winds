using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SzeptaczGrobowy : Heros
{
    public override IEnumerator OnBattleStart()
    {
        foreach(Pole pole in gameObject.GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit == null)
            {
                yield return new WaitForSeconds(0.4f );
                int buff = Evolution ? 35 : 15;
                ShowPopUp(Evolution ? "+35/35" : "+15/15", Color.green);
                Attack += buff;
                Health += buff;
                MaxHealth += buff;

            }
        }
        yield return null;
    }
}
