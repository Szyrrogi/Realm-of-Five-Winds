using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfiDowódca : Heros
{
    public override IEnumerator OnBattleStart()
    {
        if(Evolution)
        {
            foreach(Unit unit in EventSystem.eventSystem.GetComponent<FightManager>().units)
                {
                    if(unit != null && unit.Range > 0 && unit.Enemy == Enemy && unit.gameObject.GetComponent<Heros>())
                    {
                        unit.Attack += AP;
                        unit.ShowPopUp(AP.ToString(), Color.green);
                        yield return new WaitForSeconds(0.5f );
                    }
                }
        }
        else
        {
            foreach(Pole pole in GetComponent<DragObject>().pole.line.pola)
            {
                if(pole.unit != null && pole.unit.GetComponent<Unit>().Range > 0 && pole.unit.GetComponent<Unit>().Enemy == Enemy && pole.unit.GetComponent<Heros>())
                {
                    pole.unit.GetComponent<Unit>().Attack += AP;
                    pole.unit.GetComponent<Unit>().ShowPopUp(AP.ToString(), Color.green);
                    yield return new WaitForSeconds(0.5f );
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
                case 0: return ("<b>Początek Walki: </b>Zwiększ <b>Siłę</b> jednostkom z atakiem dystansowym w tym rzędzie o <color=#B803FF>" + (AP) + "</color>");
                case 1: return "<b>Start of Battle: </b>Increase <b>Strength</b> of units with ranged attack in this row by <color=#B803FF>" + (AP) + "</color>";
                case 2: return "<b>Inicio de la Batalla: </b>Aumenta la <b>Fuerza</b> de las unidades con ataque a distancia en esta fila en <color=#B803FF>" + (AP) + "</color>";
                case 3: return "<b>Début du Combat: </b>Augmente la <b>Force</b> des unités avec attaque à distance dans cette rangée de <color=#B803FF>" + (AP) + "</color>";
                case 4: return "<b>Kampfbeginn: </b>Erhöht die <b>Stärke</b> von Einheiten mit Fernangriff in dieser Reihe um <color=#B803FF>" + (AP) + "</color>";
            }
        }
        else
        {
            switch (PauseMenu.Language)
            {
                case 0: return ("<b>Początek Walki: </b>Zwiększ <b>SIłę</b> jednsotkom z atakiem dystansowym na <b>Arenie</b> o <color=#B803FF>" + (AP) + "</color>");
                case 1: return "<b>Start of Battle:</b> Increase <b>Strength</b> of units with ranged attack on the <b>Arena</b> by <color=#B803FF>" + (AP) + "</color>";
                case 2: return "<b>Inicio de la Batalla:</b> Aumenta la <b>Fuerza</b> de las unidades con ataque a distancia en la <b>Arena</b> en <color=#B803FF>" + (AP) + "</color>";
                case 3: return "<b>Début du Combat:</b> Augmente la <b>Force</b> des unités avec attaque à distance sur l'<b>Arena</b> de <color=#B803FF>" + (AP) + "</color>";
                case 4: return "<b>Kampfbeginn:</b> Erhöhe die <b>Stärke</b> von Einheiten mit Fernangriff auf der <b>Arena</b> um <color=#B803FF>" + (AP) + "</color>";
            }
        }
        return null;
    }
}
