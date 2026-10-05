using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcyWampir : Heros
{
    public override int BeforAttack(GameObject enemy, int damage)
    {
        int heal = Evolution ? damage / 10 * 7 : damage / 2;
        if (heal > enemy.GetComponent<Unit>().Health)
            heal = enemy.GetComponent<Unit>().Health;

        StartCoroutine(Heal(heal));
        return damage;
    }

}
