using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oaza : Building
{
    public override IEnumerator OnBattleStart()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.GetComponent<Pole>().line.GetComponent<Linia>().pola)
        {
            if(pole.unit != null)
            {
                pole.unit.GetComponent<Unit>().ShowPopUp("Skok", new Color(1f, 0.66f, 0f));
                pole.unit.GetComponent<Unit>().CanJump = true;
                yield return new WaitForSeconds (0.5f);
                break;
            }
        }
        yield return null;
    }
}
