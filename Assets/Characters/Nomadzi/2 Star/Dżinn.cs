using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dżinn : Heros
{
    public override void Evolve()
    {
        AP += 30;
        base.Evolve();
    }
    Wizard wizard;

    public override void Start()
    {
        base.Start();
        wizard = GetComponent<Wizard>();
    }

    public override IEnumerator OnBattleStart()
    {
        yield return wizard.spell.GetComponent<Spell>().OnBattleStart();
    }

    public override IEnumerator Fight()
    {
        Debug.Log(wizard.spell.GetComponent<Spell>().OffensifSpell);
        if (wizard.spell.GetComponent<Spell>().OffensifSpell)
        {
            wizard.spell.GetComponent<Spell>().unit = this;
            yield return wizard.spell.GetComponent<Spell>().Fight();
        }
        else
            yield return base.Fight();
    }

    public override IEnumerator Action()
    {
        yield return wizard.spell.GetComponent<Spell>().Action();
    }

    public bool Dzin;
    
    public override string DescriptionEdit()
    {
        if (Dzin)
            return base.DescriptionEdit();
        if (!Evolution)
            {
                switch (PauseMenu.Language)
                {
                    case 0: return ("<b>Nauka: </b> Zaklęcia ognia\nZna zaklęcie zwiększające <b>Siłę</b> jednostce przed sobą o <color=#B803FF>" + AP + "</color>\n<b>Ulepszenie: </b>Otrzymuje 30 <b>Mocy Zaklęć</b>");
                    case 1: return "<b>Learning: </b> Fire Spells\nKnows a spell that increases <b>Strength</b> of the unit in front by <color=#B803FF>" + AP + "</color>\n<b>Upgrade: </b>Gains 30 <b>Spell Power</b>";
                    case 2: return "<b>Aprendizaje: </b> Hechizos de Fuego\nConoce un hechizo que aumenta la <b>Fuerza</b> de la unidad delante en <color=#B803FF>" + AP + "</color>\n<b>Mejora: </b>Obtiene 30 <b>Poder de Hechizos</b>";
                    case 3: return "<b>Apprentissage: </b> Sorts de Feu\nConnaît un sort qui augmente la <b>Force</b> de l’unité devant de <color=#B803FF>" + AP + "</color>\n<b>Amélioration: </b>Reçoit 30 <b>Puissance des Sorts</b>";
                    case 4: return "<b>Lernen: </b>Feuerzauber\nKennt einen Zauber, der die <b>Stärke</b> der Einheit davor um <color=#B803FF>" + AP + "</color> erhöht\n<b>Verbesserung: </b>Erhält 30 <b>Zauberkraft</b>";
                }
            }
            else
            {
                switch (PauseMenu.Language)
                {
                    case 0: return ("<b>Nauka: </b> Zaklęcia ognia\nZna zaklęcie zwiększające <b>Siłę</b> jednostce przed sobą o <color=#B803FF>" + AP + "</color>");
                    case 1: return "<b>Learning: </b> Fire Spells\nKnows a spell that increases <b>Strength</b> of the unit in front by <color=#B803FF>" + AP + "</color>";
                    case 2: return "<b>Aprendizaje: </b> Hechizos de Fuego\nConoce un hechizo que aumenta la <b>Fuerza</b> de la unidad delante en <color=#B803FF>" + AP + "</color>";
                    case 3: return "<b>Apprentissage: </b> Sorts de Feu\nConnaît un sort qui augmente la <b>Force</b> de l’unité devant de <color=#B803FF>" + AP + "</color>";
                    case 4: return "<b>Lernen: </b>Feuerzauber\nKennt einen Zauber, der die <b>Stärke</b> der Einheit davor um <color=#B803FF>" + AP + "</color> erhöht";
                }
            }
        return null;
    }
}
