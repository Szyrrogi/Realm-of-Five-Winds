using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpancerzonyRycerz : Heros
{
    public override int BeforDamage(GameObject enemy, int damage)
    {
        enemy.GetComponent<Unit>().Health -= (Evolution ? 25 : 10);
        GameObject pop = Instantiate(PopUp, enemy.transform.position, Quaternion.identity);
        pop.GetComponent<PopUp>().SetText((Evolution ? 25 : 10).ToString(),  Color.red);

        //Unit enemyUnit = enemy.GetComponent<Unit>();
        //StartCoroutine(enemyUnit.TakeDamage(this, Evolution ? 25 : 10 ,TypeDamage.typeDamage.TrueDamage));
        
        return damage;
    }
}
