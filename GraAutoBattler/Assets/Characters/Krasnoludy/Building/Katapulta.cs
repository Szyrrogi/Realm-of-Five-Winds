using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katapulta : Building
{
    public override IEnumerator OnBattleStart()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.line.LineNext.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Heros>())
            {
                Unit enemyUnit = pole.unit.GetComponent<Unit>();
                yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, Attack))));
            }
        }
        yield return null;
    }
}
