using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Linia : MonoBehaviour
{
    public List<Pole> pola;
    public bool enemyLine;
    public int nr;

    public Linia LineNext;
    public GameObject[] poleType;

    void Start()
    {
        Porzadek();
    }

    void Porzadek()
    {
        if(enemyLine)
                pola.Reverse();
        for (int i = 0; i < pola.Count; i++)
        {
            float x = -(pola.Count * 0.125f + 0.125f) + ((pola.Count * 0.125f + 0.125f)*2)/(pola.Count-1) * (i);
            Vector3 pos = new Vector3(x, 0f, -0.5f);
            pola[i].gameObject.transform.localPosition = pos;
            
        }
        if(!enemyLine)
                pola.Reverse();
        for (int i = 0; i < pola.Count; i++)
        {
            pola[i].GetComponent<Pole>().nr = i;
            pola[i].GetComponent<Pole>().line = this;
            pola[i].GetComponent<Pole>().Start();
        }
    }

    public void Upgrade(int level)
    {
        switch(level)
        {
            case 0:  
                Porzadek();
                Vector3 newPosition = new Vector3(5, 0, 10);
                GameObject nowePole = Instantiate(poleType[1], newPosition, Quaternion.identity);
                nowePole.transform.SetParent(this.transform);
                pola.Add(nowePole.GetComponent<Pole>());
                nowePole = Instantiate(poleType[0], newPosition, Quaternion.identity);
                nowePole.transform.SetParent(this.transform);
                pola.Add(nowePole.GetComponent<Pole>());
                nowePole = Instantiate(poleType[0], newPosition, Quaternion.identity);
                nowePole.transform.SetParent(this.transform);
                pola.Add(nowePole.GetComponent<Pole>());
                Porzadek();
                break;
            case 1:  
                Porzadek();
                nowePole = Instantiate(poleType[0], pola[0].transform.position, Quaternion.identity);
                nowePole.transform.SetParent(this.transform);
                pola.Add(nowePole.GetComponent<Pole>());
                Porzadek();
                break;
            case 2:  
                nowePole = Instantiate(poleType[1], pola[0].transform.position, Quaternion.identity);
                nowePole.transform.SetParent(this.transform);
                pola.Add(nowePole.GetComponent<Pole>());
                nowePole = Instantiate(poleType[2], pola[0].transform.position, Quaternion.identity);
                nowePole.transform.SetParent(this.transform);
                Pole help = pola[3];
                pola[3] = nowePole.GetComponent<Pole>();
                pola.Add(help);
                Porzadek();
                Porzadek(); 
                break;
        }
    }
}
