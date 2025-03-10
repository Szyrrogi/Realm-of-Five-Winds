using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lider : Heros
{
    public override IEnumerator OnBattleStart()
    {
        if(Evolution)
        {
            foreach(Pole pole in GetComponent<DragObject>().pole.line.pola)
            {
                if(pole.unit != null && pole.unit.GetComponent<Unit>().Name != Name)
                {
                    yield return StartCoroutine(pole.unit.GetComponent<Unit>().OnBattleStart());
                }
            }
        }
        else
        {
        if(findPole() != null && findPole().GetComponent<Pole>().unit != null && Enemy == findPole().GetComponent<Pole>().unit.GetComponent<Unit>().Enemy)
        {
            Unit friendlyUnit = findPole().GetComponent<Pole>().unit.GetComponent<Unit>();
            yield return StartCoroutine(friendlyUnit.OnBattleStart());
        }
        }
        yield return null;
    }
}
