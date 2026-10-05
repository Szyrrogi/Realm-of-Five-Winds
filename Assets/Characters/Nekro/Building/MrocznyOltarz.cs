using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrocznyOltarz : Building
{
    public override IEnumerator OnBattleStart()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.GetComponent<Pole>().line.GetComponent<Linia>().pola)
        {
            if(pole.unit != null)
            {
                pole.unit.GetComponent<Unit>().ShowPopUp(pole.unit.GetComponent<Unit>().MaxHealth.ToString(), Color.red);
                pole.unit.GetComponent<Unit>().Health *= 2;
                pole.unit.GetComponent<Unit>().MaxHealth *= 2;
                yield return new WaitForSeconds (0.5f);
                break;
            }
        }
        yield return null;
    }

    public IEnumerator OnBattleStartExtra()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.GetComponent<Pole>().line.GetComponent<Linia>().pola)
        {
            if(pole.unit != null)
            {
                pole.unit.GetComponent<Unit>().ShowPopUp("20", new Color(0.5f,0.25f,0.1f));
                pole.unit.GetComponent<Unit>().Defense += 20;
                pole.unit.GetComponent<Unit>().MagicResist += 20;
                yield return new WaitForSeconds (0.5f);
                break;
            }
        }
        yield return null;
    }
}
