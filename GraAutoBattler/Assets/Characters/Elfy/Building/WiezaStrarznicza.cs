using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiezaStrarznicza : Building
{
    public override IEnumerator Action()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Heros>() && pole.unit.GetComponent<Heros>().Enemy != Enemy)
            {
                Unit enemyUnit = pole.unit.GetComponent<Unit>();
                yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, Attack))));
            }
        }
        yield return null;
    }
}
