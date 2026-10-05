using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medyk : Heros
{
    public override IEnumerator Action()
    {
        if(findPole() != null && findPole().GetComponent<Pole>().unit != null && Enemy == findPole().GetComponent<Pole>().unit.GetComponent<Unit>().Enemy)
        {
            Unit friendlyUnit = findPole().GetComponent<Pole>().unit.GetComponent<Unit>();
            yield return StartCoroutine(friendlyUnit.Heal(Evolution ? 10 + AP : 10));
        }
        yield return null;
    }

    public override string DescriptionEdit()
    {
        if (Evolution)
        {
            switch (PauseMenu.Language)
            {
                case 0: return "<b>Akcja: </b>Przywraca jednostce przed sobą <color=#B803FF>" + (10 + AP) + "</color><b> <b>Zdrowia</b>";
                case 1: return "<b>Action: </b>Restores <color=#B803FF>" + (10 + AP) + "</color><b> <b>Health</b> to the unit in front";
                case 2: return "<b>Acción: </b>Restaura <color=#B803FF>" + (10 + AP) + "</color><b> de <b>Salud</b> a la unidad de delante";
                case 3: return "<b>Action: </b>Restaure <color=#B803FF>" + (10 + AP) + "</color><b> <b>Santé</b> à l’unité devant";
                case 4: return "<b>Aktion: </b>Stellt der vorderen Einheit <color=#B803FF>" + (10 + AP) + "</color><b> <b>Gesundheit</b> wieder her";
            }
            return null;
        }
        else
            return Description[PauseMenu.Language]; 
    }

}
