using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Druid : Heros
{
    public List <GameObject> zwierzeta;
    public override IEnumerator Death()
    {
        yield return Summon(Evolution ? GetComponent<Wizard>().spell.GetComponent<SummonSpell>().suumonEvolutionObject : GetComponent<Wizard>().spell.GetComponent<SummonSpell>().suumonObject); //zwierzeta[Random.Range(0,zwierzeta.Count-1)]
    }
}
