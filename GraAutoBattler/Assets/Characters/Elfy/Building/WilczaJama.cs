using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilczaJama : Building
{
    public List <Unit> pattern;

    public override IEnumerator OnBattleStart()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.GetComponent<Pole>().line.GetComponent<Linia>().pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Unit>().Typy.Contains(CreatureType.Wilki))
            {

                pole.unit.GetComponent<Unit>().Health += 15;
                pole.unit.GetComponent<Unit>().MaxHealth += 15;

                        
                pole.unit.GetComponent<Unit>().ShowPopUp("+15", Color.green);
                yield return new WaitForSeconds(0.7f);

            }
        }
        yield return null;
    }
}
