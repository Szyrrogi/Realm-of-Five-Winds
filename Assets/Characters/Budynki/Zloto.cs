using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zloto : Building
{
    void Start()
    {
        MoneyManager.money += 2;
        Destroy(gameObject);
    }

    
}
