using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DruidZycia : Heros
{
    public override IEnumerator OnBattleStart()
    {
        foreach(Unit unit in EventSystem.eventSystem.GetComponent<FightManager>().units)
        {
            if((unit.gameObject.GetComponent<Drzewo>() || unit.gameObject.GetComponent<Ent>() || unit.gameObject.GetComponent<Drzewiec>()) && unit.Enemy == Enemy)
            {
                unit.Health += AP;
                unit.MaxHealth += AP;
                unit.ShowPopUp(AP.ToString(), Color.green);
                if(Evolution)
                {
                    unit.Attack += AP;
                }
            }
        }
        yield return null;
    }
    public override string DescriptionEdit()
    {
        if(Evolution)
            return "<b>Początek Walki: </b>Zwiększ atak i zdrowie wszystkich drzew na <b>arenie</b> o <color=#B803FF>" + (AP) + "</color>" ;
        else
            return "<b>Początek Walki: </b>Zwiększ zdrowie wszystkich drzew na <b>arenie</b> o <color=#B803FF>" + (AP) + "</color>" ;
    }
}
