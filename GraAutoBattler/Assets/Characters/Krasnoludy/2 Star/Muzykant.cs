using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muzykant : Heros
{
    public override void Morale()
    {
        float buff = Evolution ? 0.6f : 0.4f;
        ShowPopUp("MORALE", Color.green);
        Attack += (int)(Attack * buff);
        AP += (int)(AP * buff);
        Health += (int)(Health * buff);
        MaxHealth += (int)(MaxHealth * buff);
    }
}
