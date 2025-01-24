using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aniol : Heros
{
    public override IEnumerator Action()
    {
         if(findPole() != null && findPole().GetComponent<Pole>().unit != null && Enemy != findPole().GetComponent<Pole>().unit.GetComponent<Unit>().Enemy)
        {
            Unit enemyUnit = findPole().GetComponent<Pole>().unit.GetComponent<Unit>();
            yield return StartCoroutine(enemyUnit.TakeDamageMagic(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, AP))));
        }
        if(Evolution)
        {
            if(findPole() != null && findPole().GetComponent<Pole>().unit != null && Enemy != findPole().GetComponent<Pole>().unit.GetComponent<Unit>().Enemy)
            {
                Unit enemyUnit = findPole().GetComponent<Pole>().unit.GetComponent<Unit>();
                int rng = Random.Range(0,2);
                if(rng == 0)
                    yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, Attack))));
                else
                    yield return StartCoroutine(enemyUnit.TakeDamageMagic(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, AP))));
            }
        }
        yield return null;
    }
}
