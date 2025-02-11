using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samotnia : Building
{
    public override IEnumerator OnBattleStart()
    {
        foreach(Pole poleee in GetComponent<DragObject>().pole.GetComponent<Pole>().line.GetComponent<Linia>().pola)
        {
            if(poleee.unit != null)
            {
                Unit unit = poleee.unit.GetComponent<Unit>();
                foreach(Pole pole in unit.gameObject.GetComponent<DragObject>().pole.line.pola)
                {
                    if(pole.unit == null)
                    {
                        yield return new WaitForSeconds(0.4f / FightManager.GameSpeed);
                        int buff = 10;
                        unit.ShowPopUp("10/10", Color.green);
                        unit.Attack += buff;
                        unit.Health += buff;
                        unit.MaxHealth += buff;

                    }
                }
                break;
            }
        }
        yield return null;
    }
}
