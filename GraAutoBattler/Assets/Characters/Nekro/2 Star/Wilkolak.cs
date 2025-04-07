using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wilkolak : Heros
{
    public override IEnumerator OnBattleStart()
    {
        int buff = Evolution ? 40 : 20;
        if((Enemy && StatsManager.Round % 2 == 0) || (!Enemy && StatsManager.win % 2 == 0))
        {
            ShowPopUp(Evolution ? "+40/40" : "+20/20", Color.green);
            Attack += buff;
            Health += buff;
            MaxHealth += buff;
        }
        else
        {
            if(!Evolution)
            {
                ShowPopUp(Evolution ? "-40/40" :"-20/20", Color.red);
                Attack -= buff;
                Health -= buff;
                MaxHealth -= buff;
            }
        }
        yield return null;
    }
}
