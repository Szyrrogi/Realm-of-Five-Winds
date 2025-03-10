using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oltarz : Building
{
    public override IEnumerator OnBattleStart()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.GetComponent<Pole>().line.GetComponent<Linia>().pola)
        {
            if(pole.unit != null)
            {
                pole.unit.GetComponent<Unit>().ShowPopUp(pole.unit.GetComponent<Unit>().Attack.ToString(), new Color(1f, 0.66f, 0f));
                pole.unit.GetComponent<Unit>().Attack *= 2;
                yield return new WaitForSeconds (0.5f);
                break;
            }
        }
        yield return null;
    }
}
