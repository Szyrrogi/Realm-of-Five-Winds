using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General : Heros
{
    public GameObject Rycerz;
    public override IEnumerator Death()
    {
        yield return Summon(Rycerz);
    }
}
