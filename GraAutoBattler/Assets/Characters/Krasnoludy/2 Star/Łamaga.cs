using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Łamaga : Heros
{
    public override int BeforDamage(GameObject enemy, int damage)
    {
        int Buff = Evolution ? 20 : 10;
        Attack += Buff;
        Health += Buff;
        MaxHealth += Buff;
        return damage;
    }
}
