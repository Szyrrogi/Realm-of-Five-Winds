using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zloto : Heros
{
    void Start()
    {
        MoneyManager.money += 2;
        Destroy(gameObject);
    }

    public override void Update()
    {
        
    }

    
}
