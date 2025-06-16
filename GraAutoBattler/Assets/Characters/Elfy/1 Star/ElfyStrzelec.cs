using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfyStrzelec : Heros
{
    public override IEnumerator OnBattleStart()
    {
        int ile = Drzewo.IleDrzew(Enemy);
        int buff = Evolution ? 16 : 8;
        Attack += buff * ile;
        Health += ((buff * ile)/2);
        MaxHealth += ((buff * ile)/2);
        if(ile != 0)
            ShowPopUp("+" + (buff * ile) + "/" + ((buff * ile)/2), Color.green);
        yield return null;
    }
}
