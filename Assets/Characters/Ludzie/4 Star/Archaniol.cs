using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archaniol : Heros
{
    public int healCount;

    public static int IsArchaniol(Unit unit)
    {
        foreach(Pole pole in unit.gameObject.GetComponent<DragObject>().pole.line.pola)
        {
            if (pole.unit != null && pole.unit.GetComponent<Archaniol>() && unit.Enemy == pole.unit.GetComponent<Unit>().Enemy && pole.unit.GetComponent<Archaniol>().healCount > 0)
            {
                Debug.Log("weszlo");
                pole.unit.GetComponent<Archaniol>().healCount--;
                pole.unit.GetComponent<Archaniol>().PlaySound(pole.unit.GetComponent<Archaniol>().SoundEffect.specialEffect);
                return pole.unit.GetComponent<Archaniol>().AP;
                
            }
        }
        foreach(Pole pole in unit.gameObject.GetComponent<DragObject>().pole.line.LineNext.pola)
        {
            if (pole.unit != null && pole.unit.GetComponent<Archaniol>() && unit.Enemy == pole.unit.GetComponent<Unit>().Enemy && pole.unit.GetComponent<Archaniol>().healCount > 0)
            {
                Debug.Log("weszlo");
                pole.unit.GetComponent<Archaniol>().healCount--;
                pole.unit.GetComponent<Archaniol>().PlaySound(pole.unit.GetComponent<Archaniol>().SoundEffect.specialEffect);
                return pole.unit.GetComponent<Archaniol>().AP;
                
            }
        }
        return 0;
    }

    public override string DescriptionEdit()
    {
        if (!Evolution)
        {
            switch (PauseMenu.Language)
            {
                case 0: return ("Przywraca do życia pierwszą jednostkę, która zginie w tym rzędzie z <color=#B803FF>" + AP + "</color> <b>Zdrowia</b>");
                case 1: return "Revives the first unit that dies in this row with <color=#B803FF>" + AP + "</color> <b>Health</b>";
                case 2: return "Revive a la primera unidad que muera en esta fila con <color=#B803FF>" + AP + "</color> <b>Salud</b>";
                case 3: return "Réanime la première unité qui meurt dans cette rangée avec <color=#B803FF>" + AP + "</color> <b>Santé</b>";
                case 4: return "Belebt die erste Einheit wieder, die in dieser Reihe stirbt, mit <color=#B803FF>" + AP + "</color> <b>Gesundheit</b>";
            }
        }
        else
        {
            switch (PauseMenu.Language)
            {
                case 0: return ("Przywraca do życia pierwsze dwie jednostki, które zginą w tym rzędzie z <color=#B803FF>" + AP + "</color> <b>Zdrowia</b>");
                case 1: return "Revives the first two units that die in this row with <color=#B803FF>" + AP + "</color> <b>Health</b>";
                case 2: return "Revive a las dos primeras unidades que mueran en esta fila con <color=#B803FF>" + AP + "</color> <b>Salud</b>";
                case 3: return "Réanime les deux premières unités qui meurent dans cette rangée avec <color=#B803FF>" + AP + "</color> <b>Santé</b>";
                case 4: return "Belebt die ersten beiden Einheiten, die in dieser Reihe sterben, mit <color=#B803FF>" + AP + "</color> <b>Gesundheit</b>";
            }
        }
        return null;
    }
}
