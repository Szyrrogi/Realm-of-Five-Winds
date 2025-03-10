using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsManager : MonoBehaviour
{
    public static int life;
    public static int win = 9;
    public static int Round;

    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI winText;

    void Start()
    {
        life = 3;
        win = 0;
    }
    
    void Update()
    {
        lifeText.text = life.ToString();
        winText.text = win.ToString();
    }
}
