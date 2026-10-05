using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcyDruid : Heros
{
    public override IEnumerator Death()
    {
        yield return Summon(Evolution ? GetComponent<Wizard>().spell.GetComponent<SummonSpell>().suumonEvolutionObject : GetComponent<Wizard>().spell.GetComponent<SummonSpell>().suumonObject); //zwierzeta[Random.Range(0,zwierzeta.Count-1)]
    }

    public override IEnumerator Summon(GameObject summonMinion)
    {
        Skip = true;
        GameObject newUnitObject = Instantiate(summonMinion, gameObject.transform.position, Quaternion.identity);
        Heros newUnit = newUnitObject.GetComponent<Heros>();
        newUnit.Enemy = Enemy;
        if(Enemy)
        {
            newUnit.GetComponent<SpriteRenderer>().flipX = !newUnit.GetComponent<SpriteRenderer>().flipX;
        }
        newUnit.Health *= 2;
        newUnit.Attack *= 2;
        newUnit.MaxHealth *= 2;
        GetComponent<DragObject>().pole.unit = newUnitObject;
        GetComponent<DragObject>().pole.Start();
        yield return StartCoroutine(newUnit.OnBattleStart());
        Destroy(gameObject);
        yield return null;
    }

}
