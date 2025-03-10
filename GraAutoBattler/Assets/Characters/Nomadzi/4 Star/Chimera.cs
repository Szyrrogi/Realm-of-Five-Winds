using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chimera : Heros
{
    public override IEnumerator OnBattleStart()
    {
        if(findPole() != null && findPole().GetComponent<Pole>().unit != null && Enemy == findPole().GetComponent<Pole>().unit.GetComponent<Unit>().Enemy)
        {
            int bonus = findPole().GetComponent<Pole>().unit.GetComponent<Unit>().Initiative;
            yield return new WaitForSeconds(0.4f );
            Attack += bonus;
            Health += bonus;
            MaxHealth += bonus;
        }
        yield return null;
    }
}
