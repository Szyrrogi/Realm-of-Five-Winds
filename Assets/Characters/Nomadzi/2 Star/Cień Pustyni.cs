using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CieńPustyni : Heros
{
    public override IEnumerator OnBattleStart()
    {
        Linia line = EventSystem.eventSystem.GetComponent<FightManager>().linie[GetComponent<DragObject>().pole.line.nr];
        List<Pole> help = line.pola.ToList();

            // Odwracamy kopię listy
        help.Reverse();
        bool czyBuff = false;
            // Przechodzimy przez odwróconą kopię
        foreach (var pole in help)
        {
            if(pole.unit != null && pole.unit == gameObject)
            {
                czyBuff = true;
                break;
            }
            if(pole.unit != null && pole.unit != gameObject)
            {
                czyBuff = false;
                break;
            }
        }
        if(czyBuff)
        {
            foreach (var pole in help)
            {
                if(pole.unit != null)
                {
                    yield return new WaitForSeconds(0.4f);
                    int buff = Evolution ? 25 : 10;
                    ShowPopUp(Evolution ? "+25" : "+10", Color.green);
                    Attack += buff;
                }
            }
        }
        yield return null;
    }
}
