using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wilkolak : Heros
{
    public override IEnumerator OnBattleStart()
    {
        foreach(Pole pole in gameObject.GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null)
            {
                yield return new WaitForSeconds(0.4f );
                int buff = Evolution ? 20 : 10;
                ShowPopUp(Evolution ? "+20/20" : "+10/10", Color.green);
                Attack += buff;
                Health += buff;
                MaxHealth += buff;

            }
        }
        yield return null;
    }
}
