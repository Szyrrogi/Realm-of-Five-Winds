using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jaskolka : Heros
{
    public override IEnumerator OnBattleStart()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Unit>().Name[0] != Name[0])
            {
                pole.unit.GetComponent<Unit>().AP += AP;
                pole.unit.GetComponent<Unit>().ShowPopUp(AP.ToString(), new Color(0.5f, 0f, 1f));
                yield return new WaitForSeconds(0.5f );
            }
        }
        yield return null;
    }

    public override void Evolve()
    {
        GameObject newUnitObject = Instantiate(EvolveHeroes, gameObject.transform.position, Quaternion.identity);
        Heros newUnit = newUnitObject.GetComponent<Heros>();

        newUnit.Cost = Cost;

        GetComponent<DragObject>().pole.unit = newUnitObject;
        GetComponent<DragObject>().pole.Start();
        Destroy(gameObject);
    }

    public override string DescriptionEdit()
    {
        switch (PauseMenu.Language)
        {
            case 0: return ("<b>Początek Walki:</b> Zwiększ <b>Moc Zaklęć</b> wszystkich jednostek w rzędzie o <color=#B803FF>" + AP + "</color>\n <b>Ulepszenie:</b> Zamienia się w Anioła");
            case 1: return "<b>Start of Battle:</b> Increase <b>Spell Power</b> of all units in the row by <color=#B803FF>" + AP + "</color>\n <b>Upgrade:</b> Transforms into an Angel";
            case 2: return "<b>Inicio de la Batalla:</b> Aumenta el <b>Poder de Hechizos</b> de todas las unidades en la fila en <color=#B803FF>" + AP + "</color>\n <b>Mejora:</b> Se transforma en un Ángel";
            case 3: return "<b>Début du Combat:</b> Augmente la <b>Puissance des Sorts</b> de toutes les unités de la rangée de <color=#B803FF>" + AP + "</color>\n <b>Amélioration:</b> Se transforme en Ange";
            case 4: return "<b>Kampfbeginn:</b> Erhöht die <b>Zauberkraft</b> aller Einheiten in der Reihe um <color=#B803FF>" + AP + "</color>\n <b>Verbesserung:</b> Verwandelt sich in einen Engel";
        }
        return null;
    }
}
