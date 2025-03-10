using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leśnik : Heros
{
    public override IEnumerator OnBattleStart()
    {
        Pole pole = findPole().GetComponent<Pole>();
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
                newUnit.Health = AP;
                newUnit.MaxHealth = AP;
                yield return StartCoroutine(newUnit.OnBattleStart());
                yield return new WaitForSeconds(0.3f);
            }
    }
}
