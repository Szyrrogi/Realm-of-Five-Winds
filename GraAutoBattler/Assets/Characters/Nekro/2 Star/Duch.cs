using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duch : Heros
{
    public override int BeforDamage(GameObject enemy, int damage)
    {
        Debug.Log("1 " + (findPole().GetComponent<Pole>().unit == null));
        Debug.Log("2 " + (findPole().GetComponent<Pole>().unit != enemy));
        if(findPole().GetComponent<Pole>().unit == null ||  findPole().GetComponent<Pole>().unit != enemy)
            {
                return 0;
            }
        return damage;
    }

    public override IEnumerator OnBattleStart()
    {
        if(Evolution)
        {
            foreach(Pole pole in gameObject.GetComponent<DragObject>().pole.line.LineNext.pola)
            {
                if(pole.unit != null)
                {
                    pole.unit.GetComponent<Unit>().Range = 0;
                }
            }
        }
        yield return null;
    }
}
