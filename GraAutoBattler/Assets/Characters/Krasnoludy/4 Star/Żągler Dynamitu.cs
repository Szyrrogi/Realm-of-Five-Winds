using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ŻąglerDynamitu : Heros
{
    public override IEnumerator Fight()
    {
        if(Evolution)
        {
            if(findPole() != null)
            {
                GameObject polee = findPole();
                if((findPole() != null && findPole().GetComponent<Pole>().unit != null && Enemy != findPole().GetComponent<Pole>().unit.GetComponent<Unit>().Enemy)||
                (findPole(polee.GetComponent<Pole>()) != null && findPole(polee.GetComponent<Pole>()).GetComponent<Pole>().unit != null && Enemy != findPole(polee.GetComponent<Pole>()).GetComponent<Pole>().unit.GetComponent<Unit>().Enemy))
                {
                    foreach(Pole pole in gameObject.GetComponent<DragObject>().pole.line.pola)
                    {
                        if(pole.unit != null && pole.unit.GetComponent<Unit>().Enemy != Enemy)
                        {
                            Unit enemyUnit = pole.unit.GetComponent<Unit>();
                            yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, Attack))));
                        }
                    }

                    foreach(Pole pole in gameObject.GetComponent<DragObject>().pole.line.LineNext.pola)
                    {
                        if(pole.unit != null && pole.unit.GetComponent<Unit>().Enemy != Enemy)
                        {
                            Unit enemyUnit = pole.unit.GetComponent<Unit>();
                            yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, Attack))));
                        }
                    }
                }
            }
            yield return null;
        }
        else
        {
            if(findPole() != null && findPole().GetComponent<Pole>().unit != null && Enemy != findPole().GetComponent<Pole>().unit.GetComponent<Unit>().Enemy)
            {
                Unit enemyUnit = findPole().GetComponent<Pole>().unit.GetComponent<Unit>();
                yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, Attack))));
                if(enemyUnit.PrefUnit() != null)
                    yield return StartCoroutine(enemyUnit.PrefUnit().TakeDamage(this, enemyUnit.PrefUnit().BeforDamage(gameObject, BeforAttack(enemyUnit.PrefUnit().gameObject, Attack))));
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
                            yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, Attack))));
                            if(enemyUnit.PrefUnit() != null)
                                yield return StartCoroutine(enemyUnit.PrefUnit().TakeDamage(this, enemyUnit.PrefUnit().BeforDamage(gameObject, BeforAttack(enemyUnit.PrefUnit().gameObject, Attack))));

                        }
                    }
                }
            }
        }

        yield return null;
    }
}
