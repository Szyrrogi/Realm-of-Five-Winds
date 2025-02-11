using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Namiot : Building
{
    public override IEnumerator Action()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Unit>().Enemy == Enemy)
            {
                StartCoroutine(pole.unit.GetComponent<Unit>().Heal(5));
                yield return new WaitForSeconds(0.5f / FightManager.GameSpeed);
            }
        }
        foreach(Pole pole in GetComponent<DragObject>().pole.line.LineNext.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Unit>().Enemy == Enemy)
            {
                StartCoroutine(pole.unit.GetComponent<Unit>().Heal(5));
                yield return new WaitForSeconds(0.5f / FightManager.GameSpeed);
            }
        }
        yield return null;
    }
}
