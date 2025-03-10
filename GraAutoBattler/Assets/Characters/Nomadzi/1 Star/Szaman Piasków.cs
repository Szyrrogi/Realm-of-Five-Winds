using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SzamanPiasków : Heros
{
    public override IEnumerator OnBattleStart()
    {
        if(findPole() != null && findPole().GetComponent<Pole>().unit != null && Enemy == findPole().GetComponent<Pole>().unit.GetComponent<Unit>().Enemy)
        {
            Unit friendlyUnit = findPole().GetComponent<Pole>().unit.GetComponent<Unit>();
            yield return new WaitForSeconds(0.4f );
            int buff = Evolution ? 35 : 15;
            friendlyUnit.ShowPopUp(Evolution ? "+35" : "+15", new Color(0.5f ,0 , 1f));
            friendlyUnit.AP += buff;
        }
        yield return null;
    }
}
