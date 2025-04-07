using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wampir : Heros
{
    public override int BeforAttack(GameObject enemy, int damage)
    {
        int heal = Evolution ? damage * 2 / 3 : damage/2;
        if(heal > enemy.GetComponent<Unit>().Health)
            heal = enemy.GetComponent<Unit>().Health;

        StartCoroutine(Heal(heal));
        return damage;
    }
}
