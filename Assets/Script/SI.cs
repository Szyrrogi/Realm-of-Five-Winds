using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SI : MonoBehaviour
{
    public List<string> LudzieEasy;
    public List<string> LudzieNormal;
    public List<string> LudzieHard;
    public List<string> NekroEasy;
    public List<string> NekroNormal;
    public List<string> NekroHard;
    public List<string> NomadziEasy;
    public List<string> NomadziNormal;
    public List<string> NomadziHard;
    public List<string> KrasnaleEasy;
    public List<string> KrasnaleNormal;
    public List< string> KrasnaleHard;
    public List<string> ElfyEasy;
    public List<string> ElfyNormal;
    public List<string> ElfyHard;
    public static List<string> Official;

    public void Start()
    {
        Official = new List<string>();
    }

    public void StartBattle(int poziom)
    {
        switch(poziom)
        {
            case 0:
                for(int i = 0; i < 13; i++)
                {
                    Debug.Log(i);
                    int rng = Random.Range(0, 3);
                    switch(rng)
                    {
                    case 0:
                        Official.Add(LudzieEasy[i]); break;
                    case 1:
                        Official.Add(NekroEasy[i]); break;
                    case 2:
                        Official.Add(NomadziEasy[i]); break;
                    }
                }
                break;
            case 1:
                for(int i = 0; i < 12; i++)
                {
                    int rng = Random.Range(0, 3);
                    switch(rng)
                    {
                    case 0:
                        Official.Add(LudzieNormal[i]); break;
                    case 1:
                        Official.Add(NekroNormal[i]); break;
                    case 2:
                        Official.Add(NomadziNormal[i]); break;
                    }
                }
                break;
            case 2:
            for(int i = 0; i < 12; i++)
                {
                    int rng = Random.Range(0, 2);
                    switch(rng)
                    {
                    case 0:
                        Official.Add(LudzieHard[i]); break;
                    case 1:
                        Official.Add(NekroHard[i]); break;
                    }
                }
                break;
        }
        SceneManager.LoadScene(4);
    }

}
