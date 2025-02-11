using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plaga : Heros
{
    public override IEnumerator Fight()
    {
        foreach(Pole pole in gameObject.GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Unit>().Enemy != Enemy)
            {
                Unit enemyUnit = pole.unit.GetComponent<Unit>();
                yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, (int)(AP / (Evolution ? 7 :  10)) * (FightManager.Turn))), TypeDamage.typeDamage.Magic));
            }
        }

        foreach(Pole pole in gameObject.GetComponent<DragObject>().pole.line.LineNext.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Unit>().Enemy != Enemy)
            {
                Unit enemyUnit = pole.unit.GetComponent<Unit>();
                yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, (int)(AP / (Evolution ? 7 : 10)) * (FightManager.Turn))), TypeDamage.typeDamage.Magic));
            }
        }
        yield return null;
        if(Health <= 0)
        {
            StartCoroutine(Death());
        }
    }
}
