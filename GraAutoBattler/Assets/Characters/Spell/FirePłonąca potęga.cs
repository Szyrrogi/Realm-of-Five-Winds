using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePłonącapotęga : Spell
{
    public override IEnumerator OnBattleStart()
    {
        if(unit.findPole() != null && unit.findPole().GetComponent<Pole>().unit != null && unit.Enemy == unit.findPole().GetComponent<Pole>().unit.GetComponent<Unit>().Enemy)
        {
            Unit friendlyUnit = unit.findPole().GetComponent<Pole>().unit.GetComponent<Unit>();
            friendlyUnit.Attack += unit.AP;
            friendlyUnit.ShowPopUp(unit.AP.ToString(), new Color(0.5f, 0f, 1f));
            yield return new WaitForSeconds(0.5f / FightManager.GameSpeed);
        }
        yield return null;
    }
}
