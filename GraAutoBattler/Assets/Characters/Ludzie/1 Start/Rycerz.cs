using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rycerz : Heros
{
    public override int BeforDamage(GameObject enemy, int damage)
    {
        if(enemy.GetComponent<Unit>().Range >= 1)
            {
                float x = damage;
                for(int i = 0; i < (Evolution ? 60 : 30); i++)
                {
                    x *= 0.99f;
                }
                damage = (int)x;
            }
        return damage;
    }
}
