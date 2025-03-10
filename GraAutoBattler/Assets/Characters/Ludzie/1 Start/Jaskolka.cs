using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jaskolka : Heros
{
    public override IEnumerator OnBattleStart()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Unit>().Name != Name)
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
        return "<b>Początek Walki:</b> Zwiększ Moc Zaklęć wszystkich w rzędzie o <color=#B803FF>" + AP + "</color>" ;
    }
}
