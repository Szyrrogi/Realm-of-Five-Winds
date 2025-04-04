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
            if(pole.unit != null)
            {
                foreach(Unit unit in pattern)
                {
                    if(unit.Name == pole.unit.GetComponent<Unit>().Name)
                    {
                        pole.unit.GetComponent<Unit>().Health += 15;
                        pole.unit.GetComponent<Unit>().MaxHealth += 15;

                        
                        pole.unit.GetComponent<Unit>().ShowPopUp("+15", Color.green);
                        yield return new WaitForSeconds(0.7f);
                    }
                } 
            }
        }
        yield return null;
    }
}
