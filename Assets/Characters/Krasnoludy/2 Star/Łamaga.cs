using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Łamaga : Heros
{
    public bool synergy;
    public override int BeforDamage(GameObject enemy, int damage)
    {
        int Buff = Evolution ? 20 : 10;
        Attack += Buff;
        Health += Buff;
        MaxHealth += Buff;
        return damage;
    }

    public override IEnumerator PreAction()
    {
        if(synergy)
        {
            int Buff = Evolution ? 20 : 10;
            Attack += Buff;
            Health += (Buff - 5);
            MaxHealth += Buff;
        }
        yield return null;
    }
}
