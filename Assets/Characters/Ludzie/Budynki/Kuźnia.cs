using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kuźnia : Building
{
    public List <Unit> pattern;
    public CreatureType type;

    public override IEnumerator OnBattleStart()
    {
        int buff = 20 + (type == CreatureType.Szkielety ? HeroAbilities.CmentarzBonus(Enemy) : 0); // Cmentarz + Szkieletor
        foreach(Pole pole in GetComponent<DragObject>().pole.GetComponent<Pole>().line.GetComponent<Linia>().pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Unit>().Typy.Contains(type) && pole.unit.GetComponent<Heros>())
            {

                pole.unit.GetComponent<Unit>().Health += buff;
                pole.unit.GetComponent<Unit>().MaxHealth += buff;
                pole.unit.GetComponent<Unit>().Attack += buff;


                        
                pole.unit.GetComponent<Unit>().ShowPopUp("+" + buff + "/" + buff, Color.green);
                yield return new WaitForSeconds(0.7f);

            }
        }
        yield return null;
    }
}
