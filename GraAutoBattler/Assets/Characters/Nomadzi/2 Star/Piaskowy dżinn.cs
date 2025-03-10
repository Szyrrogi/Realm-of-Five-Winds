using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piaskowydżinn : Heros
{
    // Update is called once per frame
    void Update()
    {
        Attack = Health;
        base.Update();
    }

    public override void Evolve()
    {
        Health += 30;
        MaxHealth += 30;
        base.Evolve();
    }
}
