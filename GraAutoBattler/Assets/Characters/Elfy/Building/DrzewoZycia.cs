using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrzewoZycia : Building
{
    public override IEnumerator OnBattleStart()
    {
        int ile = Drzewo.IleDrzew(Enemy);
        foreach (Pole pole in GetComponent<DragObject>().pole.GetComponent<Pole>().line.GetComponent<Linia>().pola)
        {
            if (pole.unit != null)
            {
                int buff = ile * 10;
                pole.unit.GetComponent<Unit>().ShowPopUp(buff.ToString(), Color.red);
                pole.unit.GetComponent<Unit>().Health += buff;
                pole.unit.GetComponent<Unit>().MaxHealth += buff;
                yield return new WaitForSeconds(0.5f);
                break;
            }
        }
        yield return null;
    }
}
