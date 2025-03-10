using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drzewiec : Heros
{
    public override int BeforDamage(GameObject enemy, int damage)
    {
        int Buff = Evolution ? 10 : 5;
        Defense += Buff;
        if(Defense > (Evolution ? 70 : 50))
        {
            Defense = Evolution? 70 : 50;
        }
        return damage;
    }
}
