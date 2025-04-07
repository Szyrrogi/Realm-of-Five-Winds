using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Włucznik : Heros
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

            if(Evolution)
                help.Reverse();

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
        else
        {
            yield return StartCoroutine(base.Fight());
        }
    }
    public override IEnumerator Move()
    {
        yield return new WaitForSeconds(0);
    }
}
