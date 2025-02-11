using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kostucha : Heros
{
    public override IEnumerator Fight()
    {
        //Debug.Log("Klepa " + gameObject.name + " " + findPole().name);
        if(findPole() != null && findPole().GetComponent<Pole>().unit != null && Enemy != findPole().GetComponent<Pole>().unit.GetComponent<Unit>().Enemy)
        {
            //Debug.Log("Doszło " + gameObject.name);
            Unit enemyUnit = findPole().GetComponent<Pole>().unit.GetComponent<Unit>();
            yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, Evolution ? Attack + AP : Attack)),TypeDamage.typeDamage.TrueDamage));
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
                        yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, Evolution ? Attack + AP: Attack)),TypeDamage.typeDamage.TrueDamage));
                        //yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject ,BeforAttack(enemyUnit.gameObject, Attack))));
                    }
                }
            }
        }

        yield return null;
        if(Health <= 0)
        {
            StartCoroutine(Death());
        }
    }
}
