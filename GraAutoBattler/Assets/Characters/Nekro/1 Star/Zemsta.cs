using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zemsta : Heros
{
    public override void AfterBattle()
    {
        
        if(!Evolution)
        {
            Debug.Log("sialala");
            Attack -= 10;
            MaxHealth -= 10;
            if(MaxHealth <= 0)
            {
                MaxHealth = 1;
            }
            Health = MaxHealth;
            Cost --;
        }
    }
}
