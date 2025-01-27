using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionManager : MonoBehaviour
{
    public Text[] text;
    public static GameObject opis;
    public Unit unit;
    public Image range;
    public Sprite bow;
    public Sprite sword;

    void Start()
    {
        opis = this.gameObject;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if(unit != null)
        {
            if(unit.Range == 0)
                range.sprite = sword;
            else
                range.sprite = bow;
            text[0].text = unit.Name;
            text[1].text = unit.Attack.ToString();
            text[2].text = unit.Health.ToString();
            text[3].text = unit.Initiative.ToString();
            text[4].text = unit.Defense.ToString();
            text[5].text = (unit.UpgradeLevel + "/" + unit.UpgradeNeed);
            text[6].text = unit.Cost.ToString();
            text[7].text = unit.AP.ToString();
            text[8].text = unit.MagicResist.ToString();
            text[9].text = unit.Description;
        }
    }
}
