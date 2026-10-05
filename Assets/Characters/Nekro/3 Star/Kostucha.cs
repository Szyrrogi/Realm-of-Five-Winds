using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kostucha : Heros
{
    public override IEnumerator Fight()
    {
        if(findPole() != null && findPole().GetComponent<Pole>().unit != null && Enemy != findPole().GetComponent<Pole>().unit.GetComponent<Unit>().Enemy)
        {
            Unit enemyUnit = findPole().GetComponent<Pole>().unit.GetComponent<Unit>();
            yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, Evolution ? Attack + AP : Attack)),TypeDamage.typeDamage.TrueDamage));
        }
        else
        {
            if(Range > 0)
            {
                if(findPole() != null)
                {
                    GameObject pole = findPole();
                    if(findPole(pole.GetComponent<Pole>()) != null && findPole(pole.GetComponent<Pole>()).GetComponent<Pole>().unit != null && Enemy != findPole(pole.GetComponent<Pole>()).GetComponent<Pole>().unit.GetComponent<Unit>().Enemy)
                    {
                        Unit enemyUnit = findPole(pole.GetComponent<Pole>()).GetComponent<Pole>().unit.GetComponent<Unit>();
                        yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, Evolution ? Attack + AP: Attack)),TypeDamage.typeDamage.TrueDamage));
                    }
                }
            }
        }

        yield return null;
        if(Health <= 0)
        {
            StartCoroutine(Death());
        }
    }

    public override string DescriptionEdit()
    {
        if (!Evolution)
        {
            switch (PauseMenu.Language)
            {
                case 0: return "<b>Atak: </b>Zadaje zawsze równo <color=#FF1111>" + (Attack) + "</color>  Obrażeń" ;
                case 1: return "<b>Attack: </b>Always deals exactly <color=#FF1111>" + (Attack) + "</color> damage";
                case 2: return "<b>Ataque: </b>Siempre inflige exactamente <color=#FF1111>" + (Attack) + "</color> de daño";
                case 3: return "<b>Attaque: </b>Inflige toujours exactement <color=#FF1111>" + (Attack) + "</color> dégâts";
                case 4: return "<b>Angriff: </b>Fügt immer genau <color=#FF1111>" + (Attack) + "</color> Schaden zu";
            }
        }
        else
        {
            switch (PauseMenu.Language)
            {
                case 0: return "<b>Atak: </b>Zadaje zawsze równo <color=#FF1111>" + (Attack) + "</color> + <color=#B803FF>" + (AP) + "</color> Obrażeń" ;
                case 1: return "<b>Attack: </b>Always deals exactly <color=#FF1111>" + (Attack) + "</color> + <color=#B803FF>" + (AP) + "</color> damage";
                case 2: return "<b>Ataque: </b>Siempre inflige exactamente <color=#FF1111>" + (Attack) + "</color> + <color=#B803FF>" + (AP) + "</color> de daño";
                case 3: return "<b>Attaque: </b>Inflige toujours exactement <color=#FF1111>" + (Attack) + "</color> + <color=#B803FF>" + (AP) + "</color> dégâts";
                case 4: return "<b>Angriff: </b>Fügt immer genau <color=#FF1111>" + (Attack) + "</color> + <color=#B803FF>" + (AP) + "</color> Schaden zu";
            }
        }
        return null;
    }
}
