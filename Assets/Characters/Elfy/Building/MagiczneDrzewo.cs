using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagiczneDrzewo : Building
{
    public override IEnumerator OnBattleStart()
    {
        int ile = Drzewo.IleDrzew(Enemy);
        foreach (Pole pole in GetComponent<DragObject>().pole.GetComponent<Pole>().line.GetComponent<Linia>().pola)
        {
            if (pole.unit != null)
            {
                int buff = ile * 10;
                pole.unit.GetComponent<Unit>().ShowPopUp(buff.ToString(), new Color(0.5f, 0, 0.5f));
                pole.unit.GetComponent<Unit>().AP += buff;
                yield return new WaitForSeconds(0.5f);
                break;
            }
        }
        yield return null;
    }
}
