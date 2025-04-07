using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General : Heros
{
    public GameObject Rycerz;
    public override IEnumerator Death()
    {
        if(findPole().GetComponent<Pole>().unit != null && findPole().GetComponent<Pole>().unit.GetComponent<Czerw>())
        {
            findPole().GetComponent<Pole>().unit.GetComponent<Czerw>().buff();
        }
        Ghul.IsGhul(this);
        Loch.FirstDeathCheck(this);
        int archaniol = Archaniol.IsArchaniol(this);
        if(archaniol == 0)
        {
            yield return Summon(Rycerz);
        }
        else
        {
            ShowPopUp(archaniol.ToString(), Color.green);
            Health = archaniol;
        }
    }
}
