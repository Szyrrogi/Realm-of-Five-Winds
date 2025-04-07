using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BackToMenu : MonoBehaviour
{
    public void Back()
    {
        SceneManager.LoadScene(0);
        StatsManager.Round = 0;
        StatsManager.win = 0;
        FightManager.IsFight = false;
        StatsManager.life = 3;
        RankedManager.Poddymka = false;
        ShopManager.nizka = 0;            
    }
}
