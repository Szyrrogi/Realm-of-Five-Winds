using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMagGuildd : Building
{
    public List<GameObject> spelle;
    public override void AfterBattle()
    {
        Attack += 1;
        Debug.Log(Attack);
        if (Attack == 3)
        {
            Debug.Log("Siema");
            Attack = 1;
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
                return;
            }
            Vector3 pos = poleDocelowe.gameObject.transform.position;
            pos.z -= 2f;
            List<GameObject> newSpelle = Filter(spelle);
            if(newSpelle.Count == 0)
                return;
            
            GameObject newUnit = Instantiate(newSpelle[Random.Range(0, newSpelle.Count)], pos, Quaternion.identity);
            newUnit.GetComponent<Unit>().RealCost = 1;
            poleDocelowe.unit = newUnit;
            newUnit.GetComponent<DragObject>().pole = poleDocelowe;
        }
    }
    
    public List<GameObject> Filter(List<GameObject> prev)
    {
        List<GameObject> newSpelle = new List<GameObject>();
        foreach (GameObject obj in prev)
        {
            if (obj.GetComponent<Unit>().Star <= (StatsManager.Round / 3) + 1)
                newSpelle.Add(obj);
        }
        return newSpelle;
    }
}
