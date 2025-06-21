using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muraż : Heros
{
    public override IEnumerator OnBattleStart()
    {
        if(Evolution)
        {
            foreach(Unit unit in EventSystem.eventSystem.GetComponent<FightManager>().units)
            {
                if (unit.gameObject.GetComponent<Building>() && unit.Enemy == Enemy)
                {
                    Attack += unit.Health;
                    Health += unit.Health;
                    MaxHealth += unit.Health;
                }
            }
        }
        else
        {
        foreach(Pole pole in GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Building>())
            {
                Attack += pole.unit.GetComponent<Building>().Health;
                Health += pole.unit.GetComponent<Building>().Health;
                MaxHealth += pole.unit.GetComponent<Building>().Health;
                yield return new WaitForSeconds(0.2f);
            }
        }
        }
        yield return null;
    }
}
