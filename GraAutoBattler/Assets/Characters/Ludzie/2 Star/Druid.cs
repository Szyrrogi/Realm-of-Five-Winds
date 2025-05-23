using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Druid : Heros
{
    public List <GameObject> zwierzeta;
    public override IEnumerator Death()
    {
        if(findPole().GetComponent<Pole>().unit != null && findPole().GetComponent<Pole>().unit.GetComponent<Czerw>())
        {
            findPole().GetComponent<Pole>().unit.GetComponent<Czerw>().buff();
        }
        Ghul.IsGhul(this);
        MartwyCzerw.IsGhul(this);
        Loch.FirstDeathCheck(this);
        int archaniol = Archaniol.IsArchaniol(this);
        if(archaniol == 0)
        {
            yield return Summon(Evolution ? GetComponent<Wizard>().spell.GetComponent<SummonSpell>().suumonEvolutionObject : GetComponent<Wizard>().spell.GetComponent<SummonSpell>().suumonObject); //zwierzeta[Random.Range(0,zwierzeta.Count-1)]
        }
        else
        {
            ShowPopUp(archaniol.ToString(), Color.green);
            Health = archaniol;
        }
    }
}
