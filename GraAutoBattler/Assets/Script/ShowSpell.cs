using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSpell : MonoBehaviour
{
    bool showOpis;
    public void OnMouseEnter()
    {
        showOpis = true;
        StartCoroutine(WaitDescription());
    }

    public void OnMouseExit()
    {
        showOpis = false;
        DescriptionManager.opis.SetActive(false);
    }

    IEnumerator WaitDescription()
    {
        yield return new WaitForSeconds(0.2f / FightManager.GameSpeed);
        if(showOpis && DragObject.moveObject == null)
        {
            GameObject grandparent = transform.parent?.parent?.gameObject;
            //Debug.Log(grandparent.name);
            DescriptionManager.opis.SetActive(true);
            DescriptionManager.opis.GetComponent<DescriptionManager>().unit = grandparent.GetComponent<Wizard>().spell;
        }
    }   
}
