using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsManager : MonoBehaviour
{
    public static int life;
    public static int win;
    public static int Round;

    public Sprite RoundSprite;
    public Image RoundImage;

    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI winText;

    void Start()
    {
        life = 3;

        if(Multi.multi)
        {
            life = MultiOptions.Hearth;
        }
    
        win = 0;
        if(!RankedManager.Ranked)
        {
            RoundImage.sprite = RoundSprite;
        }
    }
    
    void Update()
    {
        lifeText.text = life.ToString();
        winText.text = win.ToString();
    }
}
