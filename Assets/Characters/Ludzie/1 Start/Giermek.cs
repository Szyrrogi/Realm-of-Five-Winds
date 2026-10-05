using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giermek : Heros
{
    public GameObject unit;
    public override void Evolve()
    {
        Pole poleDocelowe = null;
        foreach (Pole pole in EventSystem.eventSystem.GetComponent<ShopManager>().lawka)
        {
            if (pole.unit == null)
            {
                poleDocelowe = pole;
                break;
            }
        }
        if (poleDocelowe == null)
        {
            base.Evolve();
            return;
        }
        Vector3 pos = poleDocelowe.gameObject.transform.position;
        pos.z -= 2f;
        GameObject newUnit = Instantiate(unit, pos, Quaternion.identity);
        poleDocelowe.unit = newUnit;
        newUnit.GetComponent<DragObject>().pole = poleDocelowe;
        base.Evolve();
    }
    
    public override IEnumerator OnBattleStart()
    {
        if(findPole() != null && findPole().GetComponent<Pole>().unit != null && Enemy == findPole().GetComponent<Pole>().unit.GetComponent<Unit>().Enemy)
        {
            Unit friendlyUnit = findPole().GetComponent<Pole>().unit.GetComponent<Unit>();
            if (friendlyUnit.Typy.Contains(CreatureType.OddziałyLordów))
            {
                yield return new WaitForSeconds(0.4f);
                friendlyUnit.ShowPopUp("+5", new Color(0.5f, 0.5f, 0.5f));
                friendlyUnit.Defense += 5;
            }
        }
        if(PrefUnit() != null)
        {
            Unit friendlyUnit = PrefUnit();
            if (friendlyUnit.Typy.Contains(CreatureType.OddziałyLordów))
            {
                yield return new WaitForSeconds(0.4f);
                friendlyUnit.ShowPopUp("+5", new Color(0.5f, 0.5f, 0.5f));
                friendlyUnit.Defense += 5;
            }
        }
        yield return null;
    }
}
