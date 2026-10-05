using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saper : Heros
{
    public GameObject unit;
    public override void AfterBuy()
    {
        Pole poleDocelowe = null;
        foreach(Pole pole in EventSystem.eventSystem.GetComponent<ShopManager>().lawka)
        {
            if(pole.unit == null)
            {
                poleDocelowe = pole;
                break;
            }
        }
        if(poleDocelowe == null)
        {
            base.Evolve();
            return;
        }
        Vector3 pos = poleDocelowe.gameObject.transform.position;
        pos.z -= 2f;
        GameObject newUnit = Instantiate(unit, pos, Quaternion.identity);
        poleDocelowe.unit = newUnit;
        newUnit.GetComponent<DragObject>().pole = poleDocelowe;
    }
    public override void Evolve()
    {
        Pole poleDocelowe = null;
        foreach(Pole pole in EventSystem.eventSystem.GetComponent<ShopManager>().lawka)
        {
            if(pole.unit == null)
            {
                poleDocelowe = pole;
                break;
            }
        }
        if(poleDocelowe == null)
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
    public override void UpgradeHeros(Unit newUnit)
    {
        Health += (int)(newUnit.Health * 0.4f);
        MaxHealth += (int)(newUnit.MaxHealth * 0.4f);
        Attack += (int)(newUnit.Attack * 0.4f);
        AP += (int)(newUnit.AP);
        Cost += (int)(newUnit.Cost);
        RealCost += (int)(newUnit.RealCost);
        Initiative += (int)(newUnit.Initiative * 0.1f);
        UpgradeLevel += newUnit.UpgradeLevel;
        GetComponent<DragObject>().pole.unit = gameObject;

        if(GetComponent<Wizard>())
        {
            if(newUnit.gameObject.GetComponent<Wizard>().spell.Cost > GetComponent<Wizard>().spell.Cost)
            {
                Debug.Log(GetComponent<Wizard>().spell.gameObject.name);
                GetComponent<Wizard>().AddSpell(newUnit.gameObject.GetComponent<Wizard>().spell);
                Debug.Log("podmianka");
                Debug.Log(GetComponent<Wizard>().spell.gameObject.name);
            }
        }

        Destroy(newUnit.gameObject);

        if(!Evolution)
        {
            GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
            pop.GetComponent<PopUp>().SetText(UpgradeLevel + "/" + UpgradeNeed,  Color.white);
            
            if(UpgradeLevel >= UpgradeNeed)
            {
                Evolve();
            }
        }
    }
}
