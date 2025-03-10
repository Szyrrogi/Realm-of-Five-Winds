using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nekromanta : Heros
{
    public Unit wampir;
    public Unit wampirEvolve;
    public bool Synergy;
    public static int IsNekromanta(Unit unit)
    {
        foreach(Pole pole in unit.gameObject.GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Nekromanta>() && unit.Enemy == pole.unit.GetComponent<Unit>().Enemy )
            {
                if(((!pole.unit.GetComponent<Nekromanta>().Synergy && unit.Name != pole.unit.GetComponent<Wizard>().spell.GetComponent<SummonSpell>().suumonObject.GetComponent<Unit>().Name)
                || (pole.unit.GetComponent<Nekromanta>().Synergy && unit.Name != pole.unit.GetComponent<Nekromanta>().wampir.Name)))
                {
                    if(pole.unit.GetComponent<Nekromanta>().Synergy)
                    {
                        return pole.unit.GetComponent<Nekromanta>().Evolution ?  pole.unit.GetComponent<Nekromanta>().wampirEvolve.Id :
                        pole.unit.GetComponent<Nekromanta>().wampir.Id;
                    }
                    return pole.unit.GetComponent<Nekromanta>().Evolution ? pole.unit.GetComponent<Wizard>().spell.GetComponent<SummonSpell>().suumonEvolutionObject.GetComponent<Unit>().Id :
                    pole.unit.GetComponent<Wizard>().spell.GetComponent<SummonSpell>().suumonObject.GetComponent<Unit>().Id;
                }
                return 0;
            }
        }
        return 0;
    }
}
