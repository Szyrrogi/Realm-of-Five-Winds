using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Heros
{
    public override int BeforDamage(GameObject enemy, int damage)
    {
        enemy.GetComponent<Unit>().Health -= AP;
        GameObject pop = Instantiate(PopUp, enemy.transform.position, Quaternion.identity);
        pop.GetComponent<PopUp>().SetText(AP.ToString(),  new Color(0.5f,0,1f));
        
        return damage;
    }

    public override string DescriptionEdit()
    {
        return "<b>Gdy zostanie zaatakowany:</b> przeciwnik otrzymuje <color=#B803FF>" + (AP) + "</color> obrażeń"; 
    }
}
