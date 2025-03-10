using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dżinn : Heros
{
    public override void Evolve()
    {
        AP += 30;
        base.Evolve();
    }
    Wizard wizard;

    public override void Start()
    {
        base.Start();
        wizard = GetComponent<Wizard>();
    }

    public override IEnumerator OnBattleStart()
    {
        yield return wizard.spell.GetComponent<Spell>().OnBattleStart();
    }

    public override IEnumerator Fight()
    {
        Debug.Log(wizard.spell.GetComponent<Spell>().OffensifSpell);
        if(wizard.spell.GetComponent<Spell>().OffensifSpell)
        {
            wizard.spell.GetComponent<Spell>().unit = this;
            yield return wizard.spell.GetComponent<Spell>().Fight();
        }
        else
            yield return base.Fight();
    }

    public override IEnumerator Action()
    {
        yield return wizard.spell.GetComponent<Spell>().Action();
    }
}
