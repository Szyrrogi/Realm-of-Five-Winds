using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wampir : Heros
{
    public override int BeforAttack(GameObject enemy, int damage)
    {
        StartCoroutine(Heal(Evolution ? damage * 2 / 3 : damage/2));
        return damage;
    }
}
