using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medyk : Heros
{
    public override IEnumerator Action()
    {
        if(findPole() != null && findPole().GetComponent<Pole>().unit != null && Enemy == findPole().GetComponent<Pole>().unit.GetComponent<Unit>().Enemy)
        {
            Unit friendlyUnit = findPole().GetComponent<Pole>().unit.GetComponent<Unit>();
            yield return StartCoroutine(friendlyUnit.Heal(Evolution ? 10 + AP : 10));
        }
        yield return null;
    }
}
