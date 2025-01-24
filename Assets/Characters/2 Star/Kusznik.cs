using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kusznik : Heros
{
    bool afterFirstShoot;
    public override int BeforAttack(GameObject enemy, int damage)
    {
        if(!afterFirstShoot)
        {
            damage *= (Evolution ? 3 : 2);
            afterFirstShoot = true;
        }
        return damage;
    }
}
