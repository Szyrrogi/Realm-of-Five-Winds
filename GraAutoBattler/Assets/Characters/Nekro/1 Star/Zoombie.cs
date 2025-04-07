using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoombie : Heros
{
    public bool ult = true;
    public override IEnumerator Death()
    {
        
        if(ult)
        {
            Ghul.IsGhul(this);
            if(findPole().GetComponent<Pole>().unit != null && findPole().GetComponent<Pole>().unit.GetComponent<Czerw>())
            {
                findPole().GetComponent<Pole>().unit.GetComponent<Czerw>().buff();
            }
            Health = Evolution ? MaxHealth : 1;
        }
        else
            StartCoroutine(base.Death());
        ult = false;
        yield return null;
    }
}
