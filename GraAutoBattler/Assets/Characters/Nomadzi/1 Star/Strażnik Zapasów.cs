using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrażnikZapasów : Heros
{
    public bool Synergy;
    public override IEnumerator OnBattleStart()
    {
        if(findPole() != null && findPole().GetComponent<Pole>().unit != null && Enemy == findPole().GetComponent<Pole>().unit.GetComponent<Unit>().Enemy)
        {
            Unit friendlyUnit = findPole().GetComponent<Pole>().unit.GetComponent<Unit>();
            yield return new WaitForSeconds(0.4f );
            int buff = Evolution ? 60 : 20;
            friendlyUnit.ShowPopUp(Evolution ? "+60" : "+20", Color.red);
            friendlyUnit.Health += buff;
            friendlyUnit.MaxHealth += buff;
        }
        if(PrefUnit() != null && Synergy)
        {
            Unit friendlyUnit = PrefUnit();
            yield return new WaitForSeconds(0.4f );
            int buff = Evolution ? 60 : 20;
            friendlyUnit.ShowPopUp(Evolution ? "+60" : "+20", Color.red);
            friendlyUnit.Health += buff;
            friendlyUnit.MaxHealth += buff;
        }
        yield return null;
    }
}
