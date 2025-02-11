using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wampir : Heros
{
    public override int BeforAttack(GameObject enemy, int damage)
    {
        StartCoroutine(Heal(Evolution ? damage : damage/2));
        return damage;
    }
}
