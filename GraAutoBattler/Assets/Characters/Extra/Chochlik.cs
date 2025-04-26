using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chochlik : Heros
{
    public override void AfterBuy()
    {
        Bestiariusz.AddAchivments(9);
    }
}
