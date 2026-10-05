using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sciana : Heros
{
    public bool Synergia;
    public bool Synergy2;

    public override IEnumerator Fight()
    {
        if (Synergia)
            yield return StartCoroutine(base.Fight());
        yield return new WaitForSeconds(0);
    }
    public override IEnumerator Move()
    {
        yield return new WaitForSeconds(0);
    }
    public override int BeforDamage(GameObject enemy, int damage)
    {
        if (Synergy2)
        {
            enemy.GetComponent<Unit>().Health -= (5);
            GameObject pop = Instantiate(PopUp, enemy.transform.position, Quaternion.identity);
            pop.GetComponent<PopUp>().SetText((5).ToString(), Color.red);
        }
        if (Evolution)
        {
            if (1 >= FightManager.Turn)
                return 0;
        }
        return damage;
    }
}
