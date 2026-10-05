using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wampir : Heros
{
    public override int BeforAttack(GameObject enemy, int damage)
    {
        int heal = damage * AP / 100;
        if (heal > enemy.GetComponent<Unit>().Health)
            heal = enemy.GetComponent<Unit>().Health;

        StartCoroutine(Heal(heal));
        return damage;
    }

    public override string DescriptionEdit()
    {
        if (!Evolution)
        {
            switch (PauseMenu.Language)
            {
                case 0: return ("<b>Atak: </b>Przywraca sobie <b>Zdrowie</b> równe <color=#B803FF>" + (AP) + "</color>% zadanych obrażeń\n<b>Ulepszenie:</b> otrzymuje 30 <b>Mocy Zaklęć</b>");
                case 1: return "<b>Attack: </b>Restores <b>Health</b> to self equal to <color=#B803FF>" + (AP) + "</color>% of damage dealt\n<b>Upgrade:</b> gains 30 <b>Spell Power</b>";
                case 2: return "<b>Ataque: </b>Restaura <b>Salud</b> igual a <color=#B803FF>" + (AP) + "</color>% del daño infligido\n<b>Mejora:</b> obtiene 30 <b>Poder de Hechizos</b>";
                case 3: return "<b>Attaque: </b>Restaure <b>Santé</b> égale à <color=#B803FF>" + (AP) + "</color>% des dégâts infligés\n<b>Amélioration:</b> gagne 30 <b>Puissance des Sorts</b>";
                case 4: return "<b>Angriff: </b>Stellt <b>Gesundheit</b> gleich <color=#B803FF>" + (AP) + "</color>% des verursachten Schadens wieder her\n<b>Verbesserung:</b> erhält 30 <b>Zauberkraft</b>";
            }
        }
        else
        {
            switch (PauseMenu.Language)
            {
                case 0: return ("<b>Atak: </b>Przywraca sobie <b>Zdrowie</b> równe <color=#B803FF>" + (AP) + "</color>% zadanych obrażeń");
                case 1: return "<b>Attack: </b>Restores <b>Health</b> to self equal to <color=#B803FF>" + (AP) + "</color>% of damage dealt";
                case 2: return "<b>Ataque: </b>Restaura <b>Salud</b> igual a <color=#B803FF>" + (AP) + "</color>% del daño infligido";
                case 3: return "<b>Attaque: </b>Restaure <b>Santé</b> égale à <color=#B803FF>" + (AP) + "</color>% des dégâts infligés";
                case 4: return "<b>Angriff: </b>Stellt <b>Gesundheit</b> gleich <color=#B803FF>" + (AP) + "</color>% des verursachten Schadens wieder her";
            }
        }
        return null;
    }
    
    public override void Evolve()
    {
        GameObject newUnitObject = Instantiate(EvolveHeroes, gameObject.transform.position, Quaternion.identity);
        Heros newUnit = newUnitObject.GetComponent<Heros>();

        newUnit.Cost = Cost;
        newUnit.Initiative = Initiative;
        newUnit.Health = Health;
        newUnit.MaxHealth = MaxHealth;
        newUnit.Attack = Attack;
        newUnit.Defense = Defense;
        newUnit.AP = AP + 30;
        newUnit.MagicResist = MagicResist;

        newUnit.UpgradeLevel = 0;
        newUnit.UpgradeNeed = 0;
        newUnit.RealCost = 0;
        newUnit.Evolution = true;

        GetComponent<DragObject>().pole.unit = newUnitObject;
        GetComponent<DragObject>().pole.Start();
        Destroy(gameObject);
    }
}
