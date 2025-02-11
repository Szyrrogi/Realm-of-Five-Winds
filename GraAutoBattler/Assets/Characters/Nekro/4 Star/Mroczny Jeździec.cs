using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrocznyJeździec : Heros
{
    public override int BeforAttack(GameObject enemy, int damage)
    {
        Unit unit = enemy.GetComponent<Unit>();
        unit.Defense -= 5;
        if(unit.MagicResist < 0)
            unit.Defense = 0;
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
                    pole.unit.GetComponent<Unit>().Defense = 0;
                }
            }
        }
        yield return null;
    }
}
