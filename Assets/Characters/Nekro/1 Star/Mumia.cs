using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mumia : Heros
{
    public override IEnumerator Action()
    {
        yield return(StartCoroutine(Heal(Evolution ? AP : 10)));
        yield return null;
    }

    public override string DescriptionEdit()
    {
        if (Evolution)
            switch (PauseMenu.Language)
            {
                case 0: return ("<b>Akcja: </b>Przywraca sobie <color=#B803FF>" + (AP) + "</color> <b>Zdrowia</b>");
                case 1: return "<b>Action: </b>Restores <color=#B803FF>" + (AP) + "</color> <b>Health</b> to self";
                case 2: return "<b>Acción: </b>Se restaura <color=#B803FF>" + (AP) + "</color> de <b>Salud</b>";
                case 3: return "<b>Action: </b>Restaure <color=#B803FF>" + (AP) + "</color> <b>Santé</b> à soi-même";
                case 4: return "<b>Aktion: </b>Stellt sich selbst <color=#B803FF>" + (AP) + "</color> <b>Gesundheit</b> wieder her";
            }

        return Description[PauseMenu.Language];
    }
}
