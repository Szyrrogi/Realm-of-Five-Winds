using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Heros
{
    public bool WielkiGolem;
    public override void AfterBuy()
    {
        if(WielkiGolem)
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
            case 0: return ("<b>Gdy zostanie zaatakowany:</b> przeciwnik otrzymuje <color=#B803FF>" + (AP) + "</color> obrażeń");
            case 1: return "<b>When Attacked:</b> The opponent receives <color=#B803FF>" + (AP) + "</color> damage";
            case 2: return "<b>Cuando es Atacado:</b> El oponente recibe <color=#B803FF>" + (AP) + "</color> de daño";
            case 3: return "<b>Quand il est Attaqué:</b> L’adversaire reçoit <color=#B803FF>" + (AP) + "</color> dégâts";
            case 4: return "<b>Wenn angegriffen:</b> Der Gegner erhält <color=#B803FF>" + (AP) + "</color> Schaden";
        }
        return null;
    }
}
