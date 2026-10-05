using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpancerzonyRycerz : Heros
{
    public override int BeforDamage(GameObject enemy, int damage)
    {
        enemy.GetComponent<Unit>().Health -= Defense;
        GameObject pop = Instantiate(PopUp, enemy.transform.position, Quaternion.identity);
        pop.GetComponent<PopUp>().SetText(Defense.ToString(), Color.red);

        return damage;
    }

    public override string DescriptionEdit()
    {
        if (!Evolution)
        {
            switch (PauseMenu.Language)
            {
                case 0: return ("<b>Gdy zostanie zaatakowany:</b> przeciwnik otrzymuje <color=#5B5B5B> " + Defense.ToString() + "</color> obrażeń. \n<b>Ulepszenie:</b> otrzymuje 15 <b>Obrony</b>");
                case 1: return "<b>When Attacked:</b> the opponent takes <color=#5B5B5B>"  + Defense.ToString() + "</color> damage.\n<b>Upgrade:</b> gains 15 <b>Defense</b>";
                case 2: return "<b>Cuando es Atacado:</b> el oponente recibe <color=#5B5B5B>"  + Defense.ToString() + "</color> de daño.\n<b>Mejora:</b> obtiene 15 de Defensa";
                case 3: return "<b>Quand il est Attaqué:</b> l’adversaire subit <color=#5B5B5B>"  + Defense.ToString() + "</color> dégâts.\n<b>Amélioration:</b> gagne 15 Défense";
                case 4: return "<b>Wenn angegriffen:</b> erleidet der Gegner <color=#5B5B5B>"  + Defense.ToString() + "</color> Schaden.\n<b>Verbesserung:</b> erhält 15 Verteidigung";
            }
        }
        else
        {
            switch (PauseMenu.Language)
            {
                case 0: return ("<b>Gdy zostanie zaatakowany:</b> przeciwnik otrzymuje <color=#5B5B5B> " + Defense.ToString() + "</color> obrażeń.");
                case 1: return "<b>When Attacked:</b> the opponent takes <color=#5B5B5B>"  + Defense.ToString() + "</color> damage.";
                case 2: return "<b>Cuando es Atacado:</b> el oponente recibe <color=#5B5B5B>"  + Defense.ToString() + "</color> de daño.";
                case 3: return "<b>Quand il est Attaqué:</b> l’adversaire subit <color=#5B5B5B>"  + Defense.ToString() + "</color> dégâts.";
                case 4: return "<b>Wenn angegriffen:</b> erleidet der Gegner <color=#5B5B5B>"  + Defense.ToString() + "</color> Schaden.";
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
        newUnit.Attack = Attack ;
        newUnit.Defense = Defense + 15;
        newUnit.AP = AP;
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
