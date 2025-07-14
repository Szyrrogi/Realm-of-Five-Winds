using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public ShopObject[] character;
    public List<Pole> lawka;
    public static int levelUp = 1;

    public GameObject sklep;
    public GameObject sprzedarz;

    public GameObject dol;
    public GameObject gora;
    public TextMeshProUGUI RollCostText;

    public static bool isLoock;
    public Image loock;
    public Sprite[] loocks;

    public static bool isLoockUpgrade;
    public Image loockUpgrade;

    public List<ShopVisitor> shopVisitors;

    public int RollCost;
    public int FreeRoll;

    public int LevelUpCost;
    public TextMeshProUGUI LevelUpText;

    public TextMeshProUGUI sellText;
    public TextMeshProUGUI FreeRollText;
    public GameObject Chochil;
    public static int nizka;


    void Awake()
    {
        if(Fraction.fractionList == null)
        {
            Fraction.fractionList = new List<Fraction.fractionType>();
        }
        if(Fraction.fractionList.Count == 0)
        {
            Fraction.fractionList.Add(Fraction.fractionType.Ludzie);
        }
    }
    void Update()
    {
        if(nizka > LevelUpCost)
        {
            nizka = LevelUpCost;
        }
        LevelUpText.text = (LevelUpCost - nizka).ToString();
        FreeRollText.text = FreeRoll == 0  ? "" : "Darmowe odświeżenia: " + FreeRoll.ToString();
        RollCostText.text = (FreeRoll == 0 ? RollCost.ToString() : "0");
        if(DragObject.moveObject == null)
        {
            sklep.SetActive(true);
            sprzedarz.SetActive(false);
        }
        else
        {
            sellText.text = "Sprzedaż " + ((DragObject.moveObject.GetComponent<Unit>().RealCost != 0 ? DragObject.moveObject.GetComponent<Unit>().RealCost : DragObject.moveObject.GetComponent<Unit>().Cost) - 1) + " $";
            sprzedarz.SetActive(true); 
        }
        foreach(ShopVisitor shopVis in shopVisitors)
        {
            if(shopVis == null)
            {
                shopVisitors.Remove(shopVis);
                break;
            }
        }
    }

    public void ChangeLoock()
    {
        isLoock =!isLoock;
        loock.sprite = loocks[isLoock? 1 : 0];
        //Debug.Log(isLoock);
    }

    public void ChangeUpgrade()
    {
        isLoockUpgrade =!isLoockUpgrade;
        loockUpgrade.sprite = loocks[isLoockUpgrade? 1 : 0];
        //Debug.Log(isLoockUpgrade);
    }

    public void FirstRoll()
    {
        
        FreeRoll++;
        Roll();
        foreach(ShopVisitor obj in shopVisitors)
        {
            obj.FirstRoll();
        } 
    }
    public List<GameObject> filteredObjects;

    public void Roll()
    {
        if(MoneyManager.money >= RollCost || FreeRoll > 0)
        {
            if(FreeRoll > 0)
                FreeRoll--;
            else
                MoneyManager.money -= RollCost;
            CharacterManager characterManager = EventSystem.eventSystem.GetComponent<CharacterManager>();
            filteredObjects = FilterObjects(characterManager.characters);
            foreach(ShopVisitor obj in shopVisitors)
            {
                filteredObjects = obj.Filter(filteredObjects);
            }
            if(filteredObjects.Count == 0)
            {
                filteredObjects.Add(Chochil);
            }

            for(int i = 0; i < 5; i++)
            {
                int rng = Random.Range(0, filteredObjects.Count);
                if(filteredObjects[rng].GetComponent<Building>() || filteredObjects[rng].GetComponent<Spell>())
                {
                    if(Random.Range(0, 2) == 0)
                    {
                        rng = Random.Range(0, filteredObjects.Count);
                    }
                }

                character[i].unit = filteredObjects[rng];

                filteredObjects[rng].GetComponent<Unit>().RealCost = 0;
                character[i].image.sprite = filteredObjects[rng].GetComponent<SpriteRenderer>().sprite;
                character[i].name.text = filteredObjects[rng].GetComponent<Unit>().Name[PauseMenu.Language];
                character[i].price.text = 
                (filteredObjects[rng].GetComponent<Unit>().RealCost == 0 ? filteredObjects[rng].GetComponent<Unit>().Cost.ToString() : filteredObjects[rng].GetComponent<Unit>().RealCost.ToString());
                if(character[i].unit.GetComponent<Heros>())
                    character[i].SetStats();
                else
                    character[i].stats.SetActive(false);
                character[i].UpdateType();

            
            }
            
            foreach(ShopVisitor obj in shopVisitors)
            {
                obj.PostRoll();
            } 
        }
    }
    protected List<GameObject> FilterObjects(List<GameObject> objects)
    {
        List<GameObject> result = new List<GameObject>();

        foreach (var obj in objects)
        {
            Heros hero = obj.GetComponent<Heros>();

            if ((!hero || !hero.Evolution) && obj.GetComponent<Unit>().Star <= (StatsManager.Round / 3) + 1 && (obj.GetComponent<Unit>().Star != 0) 
            && (Fraction.fractionList == null || Fraction.fractionList.Contains(obj.GetComponent<Unit>().fraction)))
            {
                result.Add(obj);
            }
        }

        return result;
    }
    public void LevelUp()
    {
        if(MoneyManager.money >= LevelUpCost - nizka && levelUp < 4)
        {
            MoneyManager.money -= LevelUpCost - nizka;
            for(int i = 0; i < 3; i++)
            {
                EventSystem.eventSystem.GetComponent<FightManager>().linie[i].Upgrade(levelUp == 3 ? 1 : levelUp);
            }
            nizka = 0;
            LevelUpCost++;
            levelUp++;
            if(levelUp == 4)
            {
                LevelUpCost = 0;
            }   
        }
    }

    void Start()
    {
        if(PlayerManager.isSave)
        {
            FreeRoll = 1;
            
        }
        else
        {
            levelUp = 1;
            FreeRoll = Fraction.fractionList == null ? 3 : Fraction.fractionList.Count + 1;
        }
        Roll();
    }
}
