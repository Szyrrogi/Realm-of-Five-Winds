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
        return "Atak Piaskowego Golema jest zawsze równy zdrowiu <b>Gdy zostanie zaatakowany:</b> przeciwnik otrzymuje <color=#B803FF>" + (AP) + "</color> obrażeń"; 
    }
}
