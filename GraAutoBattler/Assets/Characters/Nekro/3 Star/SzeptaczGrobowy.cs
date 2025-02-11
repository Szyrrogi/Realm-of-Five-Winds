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
                yield return new WaitForSeconds(0.4f / FightManager.GameSpeed);
                int buff = Evolution ? 40 : 20;
                ShowPopUp(Evolution ? "+40/40" : "+20/20", Color.green);
                Attack += buff;
                Health += buff;
                MaxHealth += buff;

            }
        }
        yield return null;
    }
}
