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
        Load.SetActive(SaveService.HasRunFor("", PlayerManager.Id));
    }
}
