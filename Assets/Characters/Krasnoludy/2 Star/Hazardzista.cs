using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hazardzista : Heros
{
    public override IEnumerator OnBattleStart()
    {
        int RNG = UnityEngine.Random.Range(0, 2); 
        if (Evolution)
        {
            RNG = UnityEngine.Random.Range(0, 100);
            if (RNG > AP)
                RNG = 0;
        }
        if (!Enemy)
            {
                if (RNG == 0)
                    MoneyManager.money -= 1;
                else
                    MoneyManager.money += 2;
            }
        GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
        if (RNG == 0)
            pop.GetComponent<PopUp>().SetText("-1", Color.red);
        else
            pop.GetComponent<PopUp>().SetText("2", Color.yellow);
        yield return new WaitForSeconds(0.7f);
    }

    public override string DescriptionEdit()
    {
        if (Evolution)
            switch (PauseMenu.Language)
            {
                case 0: return ("<b>Początek Walki:</b> Zyskaj 2 sztuki złota lub strać 1 sztukę złota (<color=#B803FF>" + (Math.Min(100, AP)) + "</color>% szan na zwycięstwo)");
                case 1: return "<b>Start of Battle:</b> Gain 2 gold or lose 1 gold (<color=#B803FF>" + (Math.Min(100, AP)) + "</color>% chance to succeed)";
                case 2: return "<b>Inicio de la Batalla:</b> Gana 2 de oro o pierde 1 de oro (<color=#B803FF>" + (Math.Min(100, AP)) + "</color>% de probabilidad de éxito)";
                case 3: return "<b>Début du Combat:</b> Gagnez 2 or ou perdez 1 or (<color=#B803FF>" + (Math.Min(100, AP)) + "</color>% szan na zwycięstwo)";
                case 4: return "<b>Kampfbeginn:</b> Erhalte 2 Gold oder verliere 1 Gold (<color=#B803FF>" + (Math.Min(100, AP)) + "</color>% Chance auf Erfolg)";
            }
        else
            return Description[PauseMenu.Language];
        return null;
    }
}
