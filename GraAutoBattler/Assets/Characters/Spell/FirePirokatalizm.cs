using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePirokatalizm : Spell
{
    public override IEnumerator Fight()
    {
        AP = unit.AP;
        Debug.Log("YOOOOO");
        foreach(Pole pole in unit.gameObject.GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Unit>().Enemy != unit.Enemy)
            {
                Unit enemyUnit = pole.unit.GetComponent<Unit>();
                yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, (int)(AP / 3))), TypeDamage.typeDamage.Magic));
            }
        }

        foreach(Pole pole in unit.gameObject.GetComponent<DragObject>().pole.line.LineNext.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Unit>().Enemy != unit.Enemy)
            {
                Unit enemyUnit = pole.unit.GetComponent<Unit>();
                yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, (int)(AP / 3))), TypeDamage.typeDamage.Magic));
            }
        }
        yield return null;
        if(unit.Health <= 0)
        {
            StartCoroutine(unit.Death());
        }
    }
}
