using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rycerz : Heros
{
    public override int BeforDamage(GameObject enemy, int damage)
    {
        if (enemy.GetComponent<Unit>().Range >= 1)
        {
            float x = damage;
            for (int i = 0; i < (Evolution ? 60 : 30); i++)
            {
                x *= 0.99f;
            }
            damage = (int)x;
        }
        return damage;
    }

    public override IEnumerator OnBattleStart()
    {
        int count = 0;
        foreach (Pole pole in GetComponent<DragObject>().pole.line.pola)
        {
            if (pole.unit != null && pole.unit.GetComponent<Unit>().Name[0] != Name[0])
            {

                if (pole.unit.GetComponent<Unit>().Range > 0)
                {
                    count++;
                }
            }
        }
        count *= Evolution ? 15 : 10;
        MaxHealth += count;
        Health += count;
        if(count > 0)
            ShowPopUp(count.ToString(), Color.red);
        yield return new WaitForSeconds(0.5f );
        yield return null;
    }
}
