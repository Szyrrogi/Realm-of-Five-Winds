using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zemsta : Heros
{
    public override void AfterBattle()
    {
        
        if(!Evolution)
        {
            Attack -= 10;
            MaxHealth -= 10;
            if(MaxHealth <= 0)
            {
                MaxHealth = 1;
            }
            Health = MaxHealth;
        }
    }
}
