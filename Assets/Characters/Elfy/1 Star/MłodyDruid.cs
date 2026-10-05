using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MłodyDruid : Heros
{
    public override IEnumerator OnBattleStart()
    {
        int ile = Drzewo.IleDrzew(Enemy);
        int buff = Evolution ? 20 : 10;
        AP += buff * ile;
        if(ile != 0)
            ShowPopUp("+" + (buff * ile), new Color(0.5f, 0, 1f));
        yield return null;
    }
}
