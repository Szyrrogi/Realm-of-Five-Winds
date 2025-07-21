using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nauczyciel : Heros
{
    public override IEnumerator OnBattleStart()
    {
        Pole poleee = findPole().GetComponent<Pole>();
        if(poleee.unit != null)
        {
            Unit unitt = poleee.unit.GetComponent<Unit>();
            unitt.Range = 1;
            unitt.ShowPopUp("+Zasięg", Color.green);
            yield return new WaitForSeconds(0.3f);
        }
        if(Evolution)
        {
            foreach (Pole pole in GetComponent<DragObject>().pole.line.pola)
            {
                if (pole.unit != null)
                {
                    pole.unit.GetComponent<Unit>().Range = 1;
                    pole.unit.GetComponent<Unit>().ShowPopUp("+Zasięg", Color.green);
                    yield return new WaitForSeconds(0.3f);
                }
            }
        }
        yield return null;
    }
}
