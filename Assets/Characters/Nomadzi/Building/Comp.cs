using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comp : Building
{
    public override IEnumerator OnBattleStart()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.GetComponent<Pole>().line.GetComponent<Linia>().pola)
        {
            if(pole.unit != null)
            {
                if(pole.unit.GetComponent<Unit>().CanJump == true)
                {
                    pole.unit.GetComponent<Unit>().Attack += 18;
                    pole.unit.GetComponent<Unit>().Health += 18;
                    pole.unit.GetComponent<Unit>().MaxHealth += 18;
                    GameObject pop = Instantiate(PopUp, pole.unit.gameObject.transform.position, Quaternion.identity);
                    pop.GetComponent<PopUp>().SetText("+18/18", Color.green);
                    yield return new WaitForSeconds(0.7f);
                }
            }
        }
        yield return null;
    }
}
