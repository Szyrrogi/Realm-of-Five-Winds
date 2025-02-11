using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mumia : Heros
{
    public override IEnumerator Action()
    {
        yield return(StartCoroutine(Heal(Evolution ? AP : 10)));
        yield return null;
    }

    public override string DescriptionEdit()
    {
        if(Evolution)
            return "Co ture odnawia sobie <color=#B803FF>" + (AP) + "</color> zdrowia"  ;
        else
            return Description; 
    }
}
