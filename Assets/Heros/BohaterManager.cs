using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class BohaterManager : MonoBehaviour
{
    public List<BohaterData> AllBohater;
    public List<Bohater> BohaterObject;
    public GameObject PickObject;

    public BohaterData ChoseBohater;
    public BohaterInGame bohaterInGame;
    public static int bohaterId;

    public void Start()
    {
        //FightManager.IsFight = true;
        Roll();
        if (PlayerManager.isSave)
        {
            Pick();
        }
    }

    
    public void Roll()
    {
        var losoweDwa = AllBohater
            .OrderBy(x => UnityEngine.Random.value)
            .Take(2)
            .ToList();

        for (int i = 0; i < 2; i++)
        {
            BohaterObject[i].data = losoweDwa[i];
            BohaterObject[i].Start();
        }
    }

    public void Pick()
    {
        PickObject.SetActive(false);
        FightManager.IsFight = false;
        bohaterInGame.gameObject.SetActive(true);
        if (ChoseBohater != null)
        {
            bohaterInGame.Show(ChoseBohater.Image);
            bohaterId = ChoseBohater.bohaterId;
        }
    }
}
