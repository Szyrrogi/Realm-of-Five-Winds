using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Czerw : Heros
{
    public void buff()
    {
        int buff = Evolution ? 40 : 20;
        ShowPopUp(Evolution ? "+40/40" : "+20/20", Color.green);
        Attack += buff;
        Health += buff;
        MaxHealth += buff;
    }
}
