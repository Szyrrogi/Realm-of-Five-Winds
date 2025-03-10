using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Balista : Building
{
    public override IEnumerator Fight()
    {
        if(Range >= 1)
        {   
            FightManager fightManager = EventSystem.eventSystem.GetComponent<FightManager>();
            Linia line = fightManager.linie[GetComponent<DragObject>().pole.line.nr];
            Linia linedwa = line.LineNext;
            List<Pole> help = line.pola.ToList();
            List<Pole> helpdwa = linedwa.pola.ToList();

            help.Reverse();

            help.AddRange(helpdwa);

            foreach (var pole in help)
            {
                if (pole.unit != null && pole.unit.GetComponent<Unit>().Enemy != Enemy)
                {
                    Unit enemyUnit = pole.unit.GetComponent<Unit>();
                    yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, Attack))));
                    break;
                }
            }
        }
    }
}
