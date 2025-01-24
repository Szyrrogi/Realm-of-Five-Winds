using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palladyn : Heros
{
    public int turyNiesmiertlenosci;

    public override IEnumerator Action()
    {
        turyNiesmiertlenosci--;
        yield return null;
    }

    public override int BeforDamage(GameObject enemy, int damage)
    {
        if(turyNiesmiertlenosci > 0)
            return 0;
        return damage;
    }

}
