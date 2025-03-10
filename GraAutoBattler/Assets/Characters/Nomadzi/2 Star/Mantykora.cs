using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mantykora : Heros
{
    public override IEnumerator OnBattleStart()
    {
        int h = 0;
        for(int i = 0; i < 3 ; i++)
        {
            int j = Enemy ? i + 3 : i;
            FightManager fightManager = EventSystem.eventSystem.GetComponent<FightManager>();
            GameObject unit = fightManager.linie[j].pola[GetComponent<DragObject>().pole.nr].unit;
            if(unit == null)
            {
                h++;
            }
        }
        if(h == 2)
        {
            yield return new WaitForSeconds(0.4f );
            int buff = Evolution ? 90 : 50;
            ShowPopUp(Evolution ? "+90/90" : "+50/50", Color.green);
            Attack += buff;
            Health += buff;
            MaxHealth += buff;
        }
        yield return null;
    }
}
