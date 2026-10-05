using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiaskowyGolem : Heros
{
    public bool WielkiGolem;
    
    void Update()
    {
        Attack = Health;
        base.Update();
    }
    
    public override void AfterBuy()
    {
        if (WielkiGolem)
            Bestiariusz.AddAchivments(14);
    }
    public override int BeforDamage(GameObject enemy, int damage)
    {
        enemy.GetComponent<Unit>().Health -= AP;
        GameObject pop = Instantiate(PopUp, enemy.transform.position, Quaternion.identity);
        pop.GetComponent<PopUp>().SetText(AP.ToString(),  new Color(0.5f,0,1f));
        
        return damage;
    }

    public override string DescriptionEdit()
    {
        switch (PauseMenu.Language)
        {
            case 0: return ("<b>Siła</b> Piaskowego Golema jest równa  <color=#FF0000>" + (Health) + "</color>\n<b>Gdy zostanie zaatakowany:</b> przeciwnik otrzymuje <color=#B803FF>" + (AP) + "</color> obrażeń");
            case 1: return "<b>Strength</b> of the Sand Golem is equal to <color=#FF0000>" + (Health) + "</color>\n<b>When Attacked:</b> The opponent receives <color=#B803FF>" + (AP) + "</color> damage";
            case 2: return "<b>Fuerza</b> del Gólem de Arena es igual a <color=#FF0000>" + (Health) + "</color>\n<b>Cuando es Atacado:</b> El oponente recibe <color=#B803FF>" + (AP) + "</color> de daño";
            case 3: return "<b>Force</b> du Golem de Sable est égale à <color=#FF0000>" + (Health) + "</color>\n<b>Quand il est Attaqué:</b> L’adversaire reçoit <color=#B803FF>" + (AP) + "</color> dégâts";
            case 4: return "<b>Stärke</b> des Sandgolems entspricht <color=#FF0000>" + (Health) + "</color>\n<b>Wenn angegriffen:</b> Der Gegner erhält <color=#B803FF>" + (AP) + "</color> Schaden";
        }
        return null;
    }
}
