using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DescriptionManager : MonoBehaviour
{
    public Text[] text;
    public GameObject[] objects;
    public static GameObject opis;
    public Unit unit;
    public Image range;
    public Image staffImage;
    public Sprite bow;
    public Sprite sword;

    public Sprite staff;

    public TextMeshProUGUI description;



    void Start()
    {
        opis = this.gameObject;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if(unit != null)
        {
            int type = 0;
            if(unit.gameObject.GetComponent<Heros>() != null)
                type = 1;
            if(unit.gameObject.GetComponent<Building>() != null)
                type = 2;
            if(unit.gameObject.GetComponent<Spell>() != null)
                type = 3;

            showObject(type);

            

            text[0].text = unit.Name[PauseMenu.Language];
            if(!unit.attackAP)
            {
                if(unit.Range == 0)
                    range.sprite = sword;
                else
                    range.sprite = bow;
                text[1].text = unit.Attack.ToString();

                text[7].text = unit.AP.ToString();
                staffImage.sprite = staff;
            }
            else
            {
                range.sprite = staff;
                text[1].text = unit.AP.ToString();
                
                if(unit.Range == 0)
                    staffImage.sprite = sword;
                else
                    staffImage.sprite = bow;
                text[7].text = unit.Attack.ToString();
            }
            text[2].text = unit.Health.ToString();
            text[3].text = unit.Initiative.ToString();
            text[4].text = unit.Defense.ToString();
            if(type == 1)
                text[5].text = unit.GetComponent<Heros>().Evolution == true ? "Max" : (unit.UpgradeLevel + "/" + unit.UpgradeNeed);
            text[6].text = unit.Cost.ToString();
            
            text[8].text = unit.MagicResist.ToString();
            description.text = unit.DescriptionEdit();

            if(type == 3)
            {
                objects[10].GetComponent<Image>().sprite = unit.GetComponent<SpriteRenderer>().sprite;
            }

        }
    }

    void showObject(int type)
    {
        objects[1].SetActive(type == 1);
        objects[2].SetActive(type != 3);
        objects[3].SetActive(type != 3);
        objects[4].SetActive(type != 3);
        objects[5].SetActive(type == 1);
        objects[6].SetActive(type != 3);
        objects[7].SetActive(type == 1);
        objects[8].SetActive(type == 1);
        objects[9].SetActive(!(unit.Description.Length == 0));
        objects[10].SetActive(type == 3);
    }
}
