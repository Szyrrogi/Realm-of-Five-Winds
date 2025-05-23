using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Menu : MonoBehaviour
{
    public GameObject login;
    public GameObject logout;
    public GameObject Load;

    // Update is called once per frame
    void Update()
    {
        if(Login.loggedP)
        {
            login.SetActive(true);
            logout.SetActive(false);
        }
        else
        {
            login.SetActive(false);
            logout.SetActive(true);
        }
        string savePath2 = Application.dataPath + "/Save/Save2.json";
        if (File.Exists(savePath2))
        {
            string json = File.ReadAllText(savePath2);
            SaveManager.SaveData2 data = JsonUtility.FromJson<SaveManager.SaveData2>(json);

            if (data.playerId == PlayerManager.Id)
            {
                Load.SetActive(true);
            }
            else
            {
                Load.SetActive(false);
            }
        }
        else
        {
            Load.SetActive(false);
        }
    }
}
