using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static int Step;
    public GameObject[] Porady;
    public GameObject Rollka;
    public GameObject level;
    public Pole pole;
    public Pole poleWalka;
    public Pole poleWalkaDwa;
    public GameObject look;
    public GameObject Fight;
    public GameObject life;
    public GameObject[] unit;
    public GameObject[] liniaDwa;
    public GameObject shop;
    public GameObject koniec;
    public static bool tutorial;
    
    public void tutorialZmiana()
    {
        tutorial = false;
    }

    void Start()
    {
        Step = 0;
    }

    void Update()
    {
        if(StatsManager.Round > 2)
        {
            StatsManager.Round = 2;
            StatsManager.win = 2;
        }
        if(Step == 9 && ShopManager.isLoock)
            Fight.SetActive(true);
        if(Step == 9 && !ShopManager.isLoock)
            Fight.SetActive(false);
        Debug.Log(Step);
        if(Step == 2 && pole.unit != null)
        {
            ShowPorada(4);
            Step++;
        }
        if(Step == 4 && poleWalka.unit != null)
        {
            ShowPorada(5);
            Step++;
        }
        if(Step == 8 && poleWalka.unit != null)
        {
            ShowPorada(9);
            Step++;
        }
        if(StatsManager.Round == 1 && Step == 9)
        {
            Rollka.SetActive(false);
            Step++;
            ShowPorada(10);
        }
        if(Step == 12 && MoneyManager.money == 5)
        {
            Fight.SetActive(true);
        }
        if(StatsManager.Round == 2 && Step == 12)
        {
            Rollka.SetActive(false);
            Step++;
            life.SetActive(true);
            Fight.SetActive(false);
            shop.SetActive(false);
            ShowPorada(12);
        }
    }
    public void ShowPorada(int i)
    {
        if(i >= 0)
            Porady[i].SetActive(true);
        if(i == -3)
        {
            Step++;
        }
        if(i == -2)
        {
            Step++;
            EventSystem.eventSystem.GetComponent<ShopTutorial>().Start();
        }
        if(i == -4)
        {
            Step++;
            Rollka.gameObject.SetActive(true);
        }
        if(i == -5)
        {
            Step++;
            look.SetActive(true);
        }
        if(i == -6)
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
                return;
            }
            Vector3 pos = poleDocelowe.gameObject.transform.position;
            pos.z -= 2f;
            GameObject newUnit = Instantiate(unit[0], pos, Quaternion.identity);
            poleDocelowe.unit = newUnit;
            newUnit.GetComponent<DragObject>().pole = poleDocelowe;
        }
        if(i == -7)
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
                return;
            }
            Vector3 pos = poleDocelowe.gameObject.transform.position;
            pos.z -= 2f;
            GameObject newUnit = Instantiate(unit[1], pos, Quaternion.identity);
            poleDocelowe.unit = newUnit;
            newUnit.GetComponent<DragObject>().pole = poleDocelowe;
        }
        if(i == -8)
        {
            level.SetActive(true);
        }
        if(i == -10)
        {
            for(int J = 0; J < 2; J ++){ 
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
                return;
            }
            Vector3 pos = poleDocelowe.gameObject.transform.position;
            pos.z -= 2f;
            GameObject newUnit = Instantiate(unit[2], pos, Quaternion.identity);
            poleDocelowe.unit = newUnit;
            newUnit.GetComponent<DragObject>().pole = poleDocelowe;
            }
        }
        if(i == -11)
        {
            for(int j = 0; j < 4; j++)
            {
                liniaDwa[j].SetActive(true);
                liniaDwa[j].GetComponent<Linia>().Start();
            }
        }
        if(i == -12)
        {
            koniec.SetActive(true);
            shop.SetActive(true);
            Fight.SetActive(true);
            Rollka.SetActive(true);
        }
    }
}
