using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tarczownik : Heros
{
    public override IEnumerator OnBattleStart()
    {

        if(PrefUnit() != null && Evolution)
        {
            PrefUnit().BoskaTarcza = true;
        }
        
        yield return null;
    }
}
