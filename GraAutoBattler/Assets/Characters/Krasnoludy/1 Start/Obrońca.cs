using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obrońca : Heros
{
    public override IEnumerator Move()
    {
        if(findPole().GetComponent<Pole>().unit != null)
        {
        int buff = Evolution ? 5 : 10; 
        Attack -= buff;
        Health -= buff;
        if(Health <= 0)
            Health = 1;
        if(Attack <= 0)
            Attack = 1;
        }
        yield return StartCoroutine(base.Move());
    }

}
