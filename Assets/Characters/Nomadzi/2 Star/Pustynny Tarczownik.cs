using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PustynnyTarczownik : Heros
{
    public override IEnumerator OnBattleStart()
    {
        Linia line = EventSystem.eventSystem.GetComponent<FightManager>().linie[GetComponent<DragObject>().pole.line.nr];
        List<Pole> help = line.pola.ToList();

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
                    int buff = Evolution ? 30 : 15;
                    ShowPopUp(Evolution ? "+30" : "+15", Color.green);
                    Health += buff;
                    MaxHealth += buff;
                }
            }
        }
        yield return null;
    }
}
