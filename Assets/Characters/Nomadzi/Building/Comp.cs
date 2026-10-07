using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comp : Building
{
    public override IEnumerator OnBattleStart()
    {
        int buff = 18 + HeroAbilities.ObozBonus(Enemy);
        foreach(Pole pole in GetComponent<DragObject>().pole.GetComponent<Pole>().line.GetComponent<Linia>().pola)
        {
            if(pole.unit != null)
            {
                if(pole.unit.GetComponent<Unit>().CanJump == true)
                {
                    pole.unit.GetComponent<Unit>().Attack += buff;
                    pole.unit.GetComponent<Unit>().Health += buff;
                    pole.unit.GetComponent<Unit>().MaxHealth += buff;
                    GameObject pop = Instantiate(PopUp, pole.unit.gameObject.transform.position, Quaternion.identity);
                    pop.GetComponent<PopUp>().SetText("+" + buff + "/" + buff, Color.green);
                    yield return new WaitForSeconds(0.7f);
                }
            }
        }
        yield return null;
    }
}
