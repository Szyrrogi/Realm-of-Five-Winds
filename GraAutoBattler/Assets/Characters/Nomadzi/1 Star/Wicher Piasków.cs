using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WicherPiasków : Heros
{
    public bool strzela;
    public override IEnumerator OnBattleStart()
    {
        int h = 0;
        for(int i = 0; i < 3 ; i++)
        {
            int j = Enemy ? i + 3 : i;
            FightManager fightManager = EventSystem.eventSystem.GetComponent<FightManager>();
            GameObject unit = fightManager.linie[j].pola[GetComponent<DragObject>().pole.nr].unit;
            if(unit != null && unit.GetComponent<Unit>().Name == Name && unit != gameObject)
            {
                yield return new WaitForSeconds(0.4f );
                Debug.Log("dziala");
                int buff = Evolution ? 12 : 5;
                if(!strzela)
                    ShowPopUp(Evolution ? "+12/12" : "+5/5", Color.green);
                else
                {
                    buff = Evolution ? 15 : 6;
                    ShowPopUp(Evolution ? "+12" : "+5", Color.green);
                }
                Attack += buff;
                if(!strzela)
                {
                    Health += buff;
                    MaxHealth += buff;
                }
            }
        }
        yield return null;
    }
}
