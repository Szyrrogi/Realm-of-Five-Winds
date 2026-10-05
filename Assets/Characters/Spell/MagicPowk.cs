using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPowk : Spell
{
    public override IEnumerator Fight()
    {

        AP = unit.AP;
        if(unit.findPole() != null && unit.findPole().GetComponent<Pole>().unit != null && unit.Enemy != unit.findPole().GetComponent<Pole>().unit.GetComponent<Unit>().Enemy)
        {
            Unit enemyUnit = unit.findPole().GetComponent<Pole>().unit.GetComponent<Unit>();
            yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, AP)), TypeDamage.typeDamage.Magic));
        }
        yield return null;
        if(unit.Health <= 0)
        {
            StartCoroutine(unit.Death());
        }
    }
}
