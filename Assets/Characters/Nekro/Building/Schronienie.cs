using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schronienie : Building
{
    public CreatureType type;
    public CreatureType type2;

    public override IEnumerator OnBattleStart()
    {
        foreach (Pole pole in GetComponent<DragObject>().pole.GetComponent<Pole>().line.GetComponent<Linia>().pola)
        {
            if (pole.unit != null && (pole.unit.GetComponent<Unit>().Typy.Contains(type) || pole.unit.GetComponent<Unit>().Typy.Contains(type2)) && pole.unit.GetComponent<Heros>())
            {

                int extra = pole.unit.GetComponent<Unit>().Typy.Contains(CreatureType.Wampir) ? HeroAbilities.NawiedzonyDworBonus(Enemy) : 0;
                pole.unit.GetComponent<Unit>().Health += 20 + extra;
                pole.unit.GetComponent<Unit>().MaxHealth += 20 + extra;
                pole.unit.GetComponent<Unit>().Attack += 20 + extra;
                pole.unit.GetComponent<Unit>().AP += 20;



                pole.unit.GetComponent<Unit>().ShowPopUp("+20/20/20", Color.green);
                yield return new WaitForSeconds(0.7f);

            }
        }
        yield return null;
    }
}
