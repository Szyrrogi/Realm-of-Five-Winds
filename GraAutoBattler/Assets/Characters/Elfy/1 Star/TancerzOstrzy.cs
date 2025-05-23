using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TancerzOstrzy : Heros
{
    public bool Synergy;

    public override IEnumerator Fight()
    {
        if(!Synergy)
            yield return StartCoroutine( base.Fight());
        else
            {
            if(findPole() != null && findPole().GetComponent<Pole>().unit != null && Enemy != findPole().GetComponent<Pole>().unit.GetComponent<Unit>().Enemy)
            {
                Unit enemyUnit = findPole().GetComponent<Pole>().unit.GetComponent<Unit>();
                yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, Attack)),TypeDamage.typeDamage.TrueDamage));
            }
            else
            {
                if(Range > 0)
                {
                    if(findPole() != null)
                    {
                        GameObject pole = findPole();
                        if(findPole(pole.GetComponent<Pole>()) != null && findPole(pole.GetComponent<Pole>()).GetComponent<Pole>().unit != null && Enemy != findPole(pole.GetComponent<Pole>()).GetComponent<Pole>().unit.GetComponent<Unit>().Enemy)
                        {
                            Unit enemyUnit = findPole(pole.GetComponent<Pole>()).GetComponent<Pole>().unit.GetComponent<Unit>();
                            yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, Attack)),TypeDamage.typeDamage.TrueDamage));
                        }
                    }
                }
            }
        }
        yield return null;
    }

    public override IEnumerator Action()    
    {
        yield return StartCoroutine(Fight());
        if(Evolution)
            yield return StartCoroutine(Fight());
        yield return null;
    }
}
