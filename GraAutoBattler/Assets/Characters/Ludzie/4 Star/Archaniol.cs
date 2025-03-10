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
            if(pole.unit != null && pole.unit.GetComponent<Archaniol>() && unit.Enemy == pole.unit.GetComponent<Unit>().Enemy && pole.unit.GetComponent<Archaniol>().healCount > 0)
            {
                Debug.Log("weszlo");
                pole.unit.GetComponent<Archaniol>().healCount--;
                return pole.unit.GetComponent<Archaniol>().AP;
            }
        }
        return 0;
    }

    public override string DescriptionEdit()
    {
        if(Evolution)
            return "Pierwsze dwie jednostki, które zginą, zostają <b>Wskrzeszona</b> z <color=#B803FF>" + AP + "</color> zdrowia" ;
        return "Pierwsza jednostka, która zginie, zostaje <b>Wskrzeszona</b> z <color=#B803FF>" + AP + "</color> zdrowia" ;
    }
}
