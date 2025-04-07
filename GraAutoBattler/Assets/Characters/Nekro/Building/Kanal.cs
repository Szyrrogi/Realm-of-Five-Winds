using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kanal : Building
{
    public List <Unit> pattern;
    public CreatureType type;

    public override IEnumerator OnBattleStart()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.GetComponent<Pole>().line.GetComponent<Linia>().pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Unit>().Typy.Contains(type) && pole.unit.GetComponent<Heros>())
            {

                pole.unit.GetComponent<Unit>().Health += 15;
                pole.unit.GetComponent<Unit>().MaxHealth += 15;
                pole.unit.GetComponent<Unit>().Attack += 15;


                        
                pole.unit.GetComponent<Unit>().ShowPopUp("+15/15", Color.green);
                yield return new WaitForSeconds(0.7f);

            }
        }
        yield return null;
    }
}
