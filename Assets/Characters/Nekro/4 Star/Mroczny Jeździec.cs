using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrocznyJeździec : Heros
{
    public GameObject LastAtack;
    public override int BeforAttack(GameObject enemy, int damage)
    {
        if (enemy != LastAtack)
        {
            Unit unit = enemy.GetComponent<Unit>();
            unit.Bezradnosc();
        }
        LastAtack = enemy;
        return damage;
    }

    public override IEnumerator OnBattleStart()
    {
        if(Evolution)
        {
            foreach(Pole pole in gameObject.GetComponent<DragObject>().pole.line.LineNext.pola)
            {
                if(pole.unit != null)
                {
                    pole.unit.GetComponent<Unit>().Bezradnosc();
                }
            }
        }
        yield return null;
    }
}
