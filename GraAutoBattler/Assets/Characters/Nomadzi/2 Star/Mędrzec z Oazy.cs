using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MędrzeczOazy : Heros
{
    public override IEnumerator OnBattleStart()
    {
        int i = 0;
        foreach(Pole pole in GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Unit>().Health % 2 == 0)
            {
                i++;
            }
        }
        if(i == 0)
        {
            yield return new WaitForSeconds(0.4f );
            int buff = Evolution ? 3 : 2;
            ShowPopUp("X % 2 = 1", Color.green);
            Attack *= buff;
            Health *= buff;
            MaxHealth *= buff;
        }
        yield return null;
    }
}
