using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nekromanta : Heros
{
    public Unit wampir;
    public Unit wampirEvolve;
    public bool Synergy;

    // Lista konkretnych obiektów, które ten nekromanta już wskrzesił
    public List<Unit> resurrectedUnits = new List<Unit>();

    public static int IsNekromanta(Unit unit)
    {
        foreach (Pole pole in unit.gameObject.GetComponent<DragObject>().pole.line.pola)
        {
            if (pole.unit != null && pole.unit.GetComponent<Nekromanta>()
                && unit.Enemy == pole.unit.GetComponent<Unit>().Enemy)
            {
                Nekromanta nekromanta = pole.unit.GetComponent<Nekromanta>();

                // Sprawdzenie czy ten konkretny obiekt już był wskrzeszony
                if (nekromanta.resurrectedUnits.Contains(unit))
                {
                    return 0; // nie można wskrzesić ponownie
                }

                int resurrectedId;
                if (nekromanta.Synergy)
                {
                    resurrectedId = nekromanta.Evolution ? nekromanta.wampirEvolve.Id : nekromanta.wampir.Id;
                }
                else
                {
                    resurrectedId = nekromanta.Evolution ?
                        pole.unit.GetComponent<Wizard>().spell.GetComponent<SummonSpell>().suumonEvolutionObject.GetComponent<Unit>().Id :
                        pole.unit.GetComponent<Wizard>().spell.GetComponent<SummonSpell>().suumonObject.GetComponent<Unit>().Id;
                }

                // // zapamiętaj że ten obiekt został już wskrzeszony
                // nekromanta.resurrectedUnits.Add(unit);

                return resurrectedId;
            }
        }
        return 0;
    }

    public static Nekromanta GetNekromanta(Unit unit)
    {
        foreach (Pole pole in unit.gameObject.GetComponent<DragObject>().pole.line.pola)
        {
            if (pole.unit != null && pole.unit.GetComponent<Nekromanta>()
                && unit.Enemy == pole.unit.GetComponent<Unit>().Enemy)
            {
                return pole.unit.GetComponent<Nekromanta>();
            }
        }
        return null;
    }
}

