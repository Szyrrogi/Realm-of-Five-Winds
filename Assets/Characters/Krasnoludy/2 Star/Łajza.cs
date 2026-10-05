using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Łajza : Heros
{
    public override IEnumerator PreAction()
    {
        if(PrefUnit() != null)
        {
            Unit unitXp = PrefUnit();
            yield return StartCoroutine(unitXp.TakeDamage(this, unitXp.BeforDamage(gameObject, BeforAttack(unitXp.gameObject, AP)),TypeDamage.typeDamage.Magic));
            if(Evolution)
            {
                yield return StartCoroutine(unitXp.TakeDamage(this, unitXp.BeforDamage(gameObject, BeforAttack(unitXp.gameObject, AP)),TypeDamage.typeDamage.Magic));
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
                case 0: return ("<b>Akcja: </b>Zaatakuj jednostkę za sobą, zadając <color=#B803FF>" + AP + "</color> obrażeń");
                case 1: return "<b>Action: </b>Attacks the unit behind, dealing <color=#B803FF>" + AP + "</color> damage";
                case 2: return "<b>Acción: </b>Ataca a la unidad detrás, infligiendo <color=#B803FF>" + AP + "</color> de daño";
                case 3: return "<b>Action: </b>Attaque l’unité derrière, infligeant <color=#B803FF>" + AP + "</color> dégâts";
                case 4: return "<b>Aktion: </b>Greife die Einheit hinter dir an und verursache <color=#B803FF>" + AP + "</color> Schaden)";
            }
        }
        else
        {
            switch (PauseMenu.Language)
            {
                case 0: return ("<b>Akcja: </b>Zaatakuj dwukrotnie jednostkę za sobą, zadając <color=#B803FF>" + AP + "</color> obrażeń");
                case 1: return "<b>Action: </b>Attack the unit behind you twice, dealing <color=#B803FF>" + AP + "</color> damage";
                case 2: return "<b>Acción: </b>Ataca dos veces a la unidad que está detrás, infligiendo <color=#B803FF>" + AP + "</color> de daño";
                case 3: return "<b>Action : </b>Attaque deux fois l’unité derrière toi, infligeant <color=#B803FF>" + AP + "</color> dégâts";
                case 4: return "<b>Aktion: </b>Greife die Einheit hinter dir zweimal an und füge <color=#B803FF>" + AP + "</color> Schaden zu";
            }
        }
        return null;
    }
}
