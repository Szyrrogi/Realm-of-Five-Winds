using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piaskowydżinn : Heros
{
    // Update is called once per frame
    void Update()
    {
        Attack = Health;
        base.Update();
    }

    public override void Evolve()
    {
        Health += 50;
        MaxHealth += 50;
        base.Evolve();
    }

    public override string DescriptionEdit()
    {
        if (!Evolution)
            {
                switch (PauseMenu.Language)
                {
                    case 0: return ("<b>Siła</b> Piaskowego Widma jest równa  <color=#FF0000>" + (Health) + "</color>\n<b>Ulepszenie: </b> Otrzymuje 50 <b>Zdrowia</b>");
                    case 1: return "<b>Strength</b> of the Sand Wraith is equal to <color=#FF0000>" + (Health) + "</color>\n<b>Upgrade: </b>Gains 50 <b>Health</b>";
                    case 2: return "La <b>Fuerza</b> del Espectro de Arena es igual a <color=#FF0000>" + (Health) + "</color>\n<b>Mejora: </b>Obtiene 50 <b>Salud</b>";
                    case 3: return "La <b>Force</b> du Spectre de Sable est égale à <color=#FF0000>" + (Health) + "</color>\n<b>Amélioration: </b>Reçoit 50 <b>Santé</b>";
                    case 4: return "Die <b>Stärke</b> des Sandgeistes ist gleich <color=#FF0000>" + (Health) + "</color>\n<b>Verbesserung: </b>Erhält 50 <b>Gesundheit</b>";
                }
            }
            else
            {
                switch (PauseMenu.Language)
                {
                    case 0: return ("<b>Siła</b> Piaskowego Widma jest równa  <color=#FF0000>" + (Health) + "</color>");
                    case 1: return "<b>Strength</b> of the Sand Wraith is equal to <color=#FF0000>" + (Health) + "</color>";
                    case 2: return "La <b>Fuerza</b> del Espectro de Arena es igual a <color=#FF0000>" + (Health) + "</color>";
                    case 3: return "La <b>Force</b> du Spectre de Sable est égale à <color=#FF0000>" + (Health) + "</color>";
                    case 4: return "Die <b>Stärke</b> des Sandgeistes ist gleich <color=#FF0000>" + (Health) + "</color>";
                }
            }
        return null;
    }
}
