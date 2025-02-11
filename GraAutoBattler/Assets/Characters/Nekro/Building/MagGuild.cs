using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagGuild : Building
{
    public override IEnumerator OnBattleStart()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.GetComponent<Pole>().line.GetComponent<Linia>().pola)
        {
            if(pole.unit != null)
            {

                pole.unit.GetComponent<Unit>().AP += 15;
                        
                pole.unit.GetComponent<Unit>().ShowPopUp("+15", new Color(0.5f, 0, 1f));
                yield return new WaitForSeconds(0.6f / FightManager.GameSpeed);

            }
        }
        yield return null;
    }
}
