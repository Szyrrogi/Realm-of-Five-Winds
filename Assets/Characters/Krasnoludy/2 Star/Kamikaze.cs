using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : Heros
{
    public override int BeforDamage(GameObject enemy, int damage)
    {
        return damage * 4;
    }
}
