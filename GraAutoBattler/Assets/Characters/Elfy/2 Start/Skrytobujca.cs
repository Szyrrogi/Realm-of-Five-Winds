using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skrytobujca : Heros
{
    public override IEnumerator OnBattleStart()
    {
        bool repet = false;
        for(int i = 0; i < 1; i++)
        { 
            if(Evolution && !repet )
            {
                repet = true;
                i--;
            }
            Unit potencial = null;
            foreach(Pole pole in GetComponent<DragObject>().pole.line.LineNext.pola)
            {
                if(pole.unit != null && pole.unit.GetComponent<Heros>())
                {
                    potencial = pole.unit.GetComponent<Unit>();
                }
            }
            if(potencial != null)
            {
                potencial.Health -= Attack;
                GameObject pop = Instantiate(PopUp, potencial.gameObject.transform.position, Quaternion.identity);
                pop.GetComponent<PopUp>().SetText(Attack.ToString(),  Color.red);
            }
            yield return null;
        }
        
    }
}
