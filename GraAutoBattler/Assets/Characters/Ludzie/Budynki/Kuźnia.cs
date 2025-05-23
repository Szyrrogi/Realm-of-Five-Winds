using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kuźnia : Building
{
    public List <Unit> pattern;
    public CreatureType type;

    public override IEnumerator OnBattleStart()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.GetComponent<Pole>().line.GetComponent<Linia>().pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Unit>().Typy.Contains(type) && pole.unit.GetComponent<Heros>())
            {

                pole.unit.GetComponent<Unit>().Health += 20;
                pole.unit.GetComponent<Unit>().MaxHealth += 20;
                pole.unit.GetComponent<Unit>().Attack += 20;


                        
                pole.unit.GetComponent<Unit>().ShowPopUp("+20/20", Color.green);
                yield return new WaitForSeconds(0.7f);

            }
        }
        yield return null;
    }
}
