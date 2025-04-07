using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synergy : MonoBehaviour
{
    public List<Unit> units;
    public int Id;
    
    public GameObject opis;
    public bool showOpis;
    public bool Enemy;

    public virtual IEnumerator BeforBattle()
    {
        Debug.Log("wykonano " + Id);
        yield return null;
    }

    public virtual void AfterBattle()
    {
        Debug.Log("wykonanoAfter " + Id);
    }

    public virtual bool CheckIsActive(int modify)
    {
        for (int i = 0; i < 3; i++)
        {
            Linia line = EventSystem.eventSystem.GetComponent<FightManager>().linie[i + modify];
            List<Unit> newUnits = new List<Unit>(units);
            foreach (var pole in line.pola)
            {
                foreach (Unit unit in units)
                {
                    if (pole.unit != null && unit != null && unit.Name == pole.unit.GetComponent<Unit>().Name)
                    {
                        Unit unitToRemove = newUnits.Find(u => u.Name == pole.unit.GetComponent<Unit>().Name);

                        if (unitToRemove != null)
                        {
                            newUnits.Remove(unitToRemove);
                        }
                    }
                }
                if (newUnits.Count == 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

     public void OnMouseEnter()
    {
        showOpis = true;
        StartCoroutine(WaitDescription());
    }

    public void OnMouseExit()
    {
        showOpis = false;
        opis.SetActive(false);
    }

    IEnumerator WaitDescription()
    {
        // Czekaj przez określony czas
        yield return new WaitForSeconds(0.1f);

        if (showOpis)
        {
            // Pobierz RectTransform, jeśli opis jest elementem UI
            RectTransform opisRectTransform = opis.GetComponent<RectTransform>();

            // Sprawdź pozycję X (używając anchoredPosition dla UI)
            if (Enemy && opisRectTransform.anchoredPosition.x > 0)
            {
                // Ustaw nową pozycję
                opisRectTransform.anchoredPosition = new Vector2(-opisRectTransform.anchoredPosition.x, opisRectTransform.anchoredPosition.y);
            }

            // Aktywuj opis
            opis.SetActive(true);
        }
    }
}