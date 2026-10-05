using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SzczurzyLord : Heros
{
    public GameObject summonMinion;
    public override IEnumerator OnBattleStart()
    {
        Linia line = EventSystem.eventSystem.GetComponent<FightManager>().linie[GetComponent<DragObject>().pole.line.nr];
        List<Pole> help = line.pola.ToList();

        foreach (var pole in help)
        {
            if(pole.unit == null)
            {
                GameObject newUnitObject = Instantiate(Evolution ? GetComponent<Wizard>().spell.GetComponent<SummonSpell>().suumonEvolutionObject : GetComponent<Wizard>().spell.GetComponent<SummonSpell>().suumonObject, gameObject.transform.position, Quaternion.identity);
                Heros newUnit = newUnitObject.GetComponent<Heros>();
                newUnit.Enemy = Enemy;
                if(Enemy)
                {
                    newUnit.GetComponent<SpriteRenderer>().flipX = !newUnit.GetComponent<SpriteRenderer>().flipX;
                }

                pole.unit = newUnitObject;
                pole.Start();
                newUnit.Health = AP / 2;
                newUnit.MaxHealth = AP / 2;
                yield return StartCoroutine(newUnit.OnBattleStart());
                yield return new WaitForSeconds(0.3f);
                // if(Evolution)
                // {
                //     newUnit.Evolve();
                // }
            }
        }
        
    yield return null;
    }
}
