using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bard : Heros
{
    public override IEnumerator OnBattleStart()
    {
        if(findPole() != null && findPole().GetComponent<Pole>().unit != null && Enemy == findPole().GetComponent<Pole>().unit.GetComponent<Unit>().Enemy)
        {
            Unit friendlyUnit = findPole().GetComponent<Pole>().unit.GetComponent<Unit>();
            yield return new WaitForSeconds(0.4f );
            friendlyUnit.Morale();
            if(Evolution)
            {
                yield return new WaitForSeconds(0.4f );
                friendlyUnit.Morale();
            }
        }
        yield return null;
    }
}
