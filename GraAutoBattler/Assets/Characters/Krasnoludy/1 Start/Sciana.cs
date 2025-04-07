using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sciana : Heros
{
    public bool Synergia;
    public override IEnumerator Fight()
    {
        if(Synergia)
            yield return StartCoroutine(base.Fight());
        yield return new WaitForSeconds(0);
    }
    public override IEnumerator Move()
    {
        yield return new WaitForSeconds(0);
    }
    public override int BeforDamage(GameObject enemy, int damage)
    {
        if(Evolution)
        {
        if(1 >= FightManager.Turn)
            return 0;
        }
        return damage;
    }
}
