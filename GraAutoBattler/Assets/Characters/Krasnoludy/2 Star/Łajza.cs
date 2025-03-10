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
        if(Evolution)
            return "<b>Akcja: </b>Zadaj dwukrotnie <color=#B803FF>" + (AP) + "</color> obrażeń jednostce za sobą"; 
        else
            return "<b>Akcja: </b>Zadaj <color=#B803FF>" + (AP) + "</color> obrażeń jednostce za sobą"; 
    }
}
