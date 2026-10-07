using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System;


public class ShopManager : MonoBehaviour
{
    public ShopObject[] character;
    public List<Pole> lawka;
    public static int levelUp = 1;
    public static int allRoll;

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
    public AudioSource audioSource;
    public AudioClip sellSound;
    public List<AudioClip> RollSound;


    void Awake()
    {
        if (Fraction.fractionList == null)
        {
            Fraction.fractionList = new List<Fraction.fractionType>();
        }
        if (Fraction.fractionList.Count == 0)
        {
            Fraction.fractionList.Add(Fraction.fractionType.Ludzie);
        }
    }

    public List<GameObject> poka;


    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            float randomVolume = audioSource.volume * UnityEngine.Random.Range(0.9f, 1f);
            audioSource.clip = clip;
            audioSource.volume = randomVolume;
            audioSource.Play();
        }
    }

    public void SellSound()
    {
        if (audioSource != null && sellSound != null)
        {
            float randomVolume = audioSource.volume * UnityEngine.Random.Range(0.9f, 1f);
            audioSource.clip = sellSound;
            audioSource.volume = randomVolume;
            audioSource.Play();
        }
    }

    void Update()
    {
        string[] freeRollLanguage = { "Darmowe odświeżenia", "Free Refreshes", "Refrescos Gratis", "Rafraîchissements Gratuits", "Kostenlose Aktualisierungen" };
        FreeRollText.text = FreeRoll == 0  ? "" : freeRollLanguage[PauseMenu.Language] + ": " + FreeRoll.ToString();
        RollCostText.text = (FreeRoll == 0 ? RollCost.ToString() : "0");
        
        if (DragObject.moveObject == null)
        {
            sklep.SetActive(true);
            sprzedarz.SetActive(false);
        }
        else
        {
            string[] sellLanguage = { "Sprzedaż", "	Sale", "Oferta", "Promotion", "Angebot" };
            sellText.text = sellLanguage[PauseMenu.Language] + " " + ((DragObject.moveObject.GetComponent<Unit>().RealCost != 0 ? DragObject.moveObject.GetComponent<Unit>().RealCost : DragObject.moveObject.GetComponent<Unit>().Cost) - 1) + " $";
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
    }

    public void ChangeUpgrade()
    {
        isLoockUpgrade =!isLoockUpgrade;
        loockUpgrade.sprite = loocks[isLoockUpgrade? 1 : 0];
    }

    public void FirstRoll()
    {
        FreeRoll++;
        Roll();
        foreach(ShopVisitor obj in shopVisitors)
        {
            obj.FirstRoll();
        } 
        foreach (ShopObject s in character) s.OdswiezCene();
    }
    public List<GameObject> filteredObjects;

    public void RollSoundVoid()
    {
        if ((MoneyManager.money >= RollCost || FreeRoll > 0) && (FightManager.IsFight == false && FightManager.IsOptions == false))
            PlaySound(RollSound[UnityEngine.Random.Range(0, RollSound.Count)]);
    }

    public void Roll()
    {
        if ((MoneyManager.money >= RollCost || FreeRoll > 0) && (FightManager.IsFight == false && FightManager.IsOptions == false))
        {
            if (FreeRoll > 0)
                FreeRoll--;
            else
                MoneyManager.money -= RollCost;
                
            CharacterManager characterManager = EventSystem.eventSystem.GetComponent<CharacterManager>();
            filteredObjects = FilterObjects(characterManager.characters);
            foreach (ShopVisitor obj in shopVisitors)
            {
                filteredObjects = obj.Filter(filteredObjects);
            }
            if (filteredObjects.Count == 0)
            {
                filteredObjects.Add(Chochil);
            }

            for (int i = 0; i < 5; i++)
            {
                int rng = UnityEngine.Random.Range(0, filteredObjects.Count);
                if (filteredObjects[rng].GetComponent<Building>() || filteredObjects[rng].GetComponent<Spell>())
                {
                    if (UnityEngine.Random.Range(0, 2) == 0)
                    {
                        rng = UnityEngine.Random.Range(0, filteredObjects.Count);
                    }
                }

                character[i].unit = filteredObjects[rng];
                filteredObjects[rng].GetComponent<Unit>().RealCost = 0;
                character[i].image.sprite = filteredObjects[rng].GetComponent<SpriteRenderer>().sprite;
                character[i].name.text = filteredObjects[rng].GetComponent<Unit>().Name[PauseMenu.Language];
                switch (filteredObjects[rng].GetComponent<Unit>().Star)
                {
                    case 0:
                        character[i].naText.color = Color.red;
                        break;
                    case 1:
                        character[i].naText.color = Color.gray;
                        break;
                    case 2:
                        character[i].naText.color = Color.blue;
                        break;
                    case 3:
                        character[i].naText.color = new Color(0.5f, 0f, 0.5f);
                        break;
                    case 4:
                        character[i].naText.color = Color.yellow;
                        break;
                    default:
                        character[i].naText.color = Color.red;
                        break;
                }
                character[i].price.text = character[i].Cena().ToString();
                
                if (character[i].unit.GetComponent<Heros>())
                    character[i].SetStats();
                else
                    character[i].stats.SetActive(false);
                    
                character[i].UpdateType();
            }
            allRoll++;
            foreach (ShopVisitor obj in shopVisitors)
            {
                obj.PostRoll();
            }
            foreach (ShopObject s in character) s.OdswiezCene(); // Budowlaniec / Darmowe Zaklęcie zmieniają cenę w PostRoll

            if (PlayerManager.Id != 0)
                SaveManager.Save(PlayerManager.Name, PlayerManager.PlayerFaceId, ShopManager.levelUp, BohaterManager.bohaterId);
        }
    }
    public GameObject MrocznaChata;

    protected List<GameObject> FilterObjects(List<GameObject> objects)
    {
        if (StoryManager.Active)
            return StoryManager.FilterShop(objects);

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

        if (WszystkieRzedyNaMax() && result.Contains(MrocznaChata))
        {
            result.Remove(MrocznaChata);
        }

        return result;
    }

    // Globalny Level Up usunięty – rzędy ulepsza się osobno (Linia.TryUpgrade).
    bool WszystkieRzedyNaMax()
    {
        List<Linia> linie = EventSystem.eventSystem.GetComponent<FightManager>().linie;
        for (int i = 0; i < 3; i++)
            if (linie[i].MoznaUlepszyc()) return false;
        return true;
    }

    void Start()
    {
        CharacterManager characterManager = EventSystem.eventSystem.GetComponent<CharacterManager>();
        
        allRoll = 0;
        if (PlayerManager.isSave)
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