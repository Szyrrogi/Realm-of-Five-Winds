using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Heros
{
    public override IEnumerator OnBattleStart()
    {
        AP += Mine.IleMin(Enemy);
        yield return new WaitForSeconds(0);
    }
    public override IEnumerator Fight()
    {
        yield return new WaitForSeconds(0);
    }
    public override IEnumerator Move()
    {
        yield return new WaitForSeconds(0);
    }

    public static int IleMin(bool isEnemy)
    {
        int ile = 0;
        foreach(Unit unit in EventSystem.eventSystem.GetComponent<FightManager>().units)
        {
            if(unit != null && unit.gameObject.GetComponent<Saper>() && unit.Enemy == isEnemy)
            {
                ile += unit.gameObject.GetComponent<Saper>().AP;
            }
        }
        return ile;
    }
    public override int BeforDamage(GameObject enemy, int damage)
    {
        if(enemy.GetComponent<Unit>().Range == 0 || Evolution)
        {
            if(enemy.GetComponent<Unit>().BoskaTarcza)
            {
                GameObject pop = Instantiate(PopUp, enemy.transform.position, Quaternion.identity);
                pop.GetComponent<PopUp>().SetText("0",  new Color(0.5f,0,1f));
                enemy.GetComponent<Unit>().BoskaTarcza = false;
            }
            else
            {
                enemy.GetComponent<Unit>().Health -= AP;
                GameObject pop = Instantiate(PopUp, enemy.transform.position, Quaternion.identity);
                pop.GetComponent<PopUp>().SetText(AP.ToString(),  new Color(0.5f,0,1f));
            }
        }
        
        return damage;
    }
}
