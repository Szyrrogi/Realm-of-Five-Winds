using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DruidZycia : Heros
{
    public override IEnumerator OnBattleStart()
    {
        foreach(Unit unit in EventSystem.eventSystem.GetComponent<FightManager>().units)
        {
            if(unit.Typy.Contains(CreatureType.Drzewo) && unit.Enemy == Enemy)
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
        if (!Evolution)
        {
            switch (PauseMenu.Language)
            {
                case 0: return ("<b>Początek Walki: </b>Zwiększ <b>Zdrowie</b> wszystkich <b>Drzew</b> na <b>Arenie</b> o <color=#B803FF>" + (AP) + "</color>");
                case 1: return "<b>Start of Battle: </b>Increase <b>Health</b> of all <b>Trees</b> on the <b>Arena</b> by <color=#B803FF>" + (AP) + "</color>";
                case 2: return "<b>Inicio de la Batalla: </b>Aumenta la <b>Salud</b> de todos los <b>Árboles</b> en la <b>Arena</b> en <color=#B803FF>" + (AP) + "</color>";
                case 3: return "<b>Début du Combat: </b>Augmente la <b>Santé</b> de tous les <b>Arbres</b> sur l’<b>Arène</b> de <color=#B803FF>" + (AP) + "</color>";
                case 4: return "<b>Kampfbeginn: </b>Erhöht die <b>Gesundheit</b> aller <b>Bäume</b> auf der <b>Arena</b> um <color=#B803FF>" + (AP) + "</color>";
            }
        }
        else
        {
            switch (PauseMenu.Language)
            {
                case 0: return ("<b>Początek Walki: </b>Zwiększ <b>Zdrowie</b> i <b>Siłe</b> wszystkich <b>Drzew</b> na <b>Arenie</b> o <color=#B803FF>" + (AP) + "</color>");
                case 1: return "<b>Start of Battle:</b> Increases <b>Health</b> and <b>Strength</b> of all <b>Trees</b> on the <b>Arena</b> by <color=#B803FF>" + (AP) + "</color>";
                case 2: return "<b>Inicio de la Batalla:</b> Aumenta la <b>Salud</b> y la <b>Fuerza</b> de todos los <b>Árboles</b> en la <b>Arena</b> en <color=#B803FF>" + (AP) + "</color>";
                case 3: return "<b>Début du Combat:</b> Augmente la <b>Santé</b> et la <b>Force</b> de tous les <b>Arbres</b> sur l'<b>Arène</b> de <color=#B803FF>" + (AP) + "</color>";
                case 4: return "<b>Kampfbeginn:</b> Erhöht <b>Gesundheit</b> und <b>Stärke</b> aller <b>Bäume</b> auf der <b>Arena</b> um <color=#B803FF>" + (AP) + "</color>";
            }
        }
        return null;
    }
}
