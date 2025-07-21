using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public TextMeshProUGUI tagi;



    void Start()
    {
        opis = this.gameObject;
        if (SceneManager.GetActiveScene().buildIndex != 10)
            gameObject.SetActive(false);
    }

    private static readonly Dictionary<Unit.CreatureType, string[]> translations = new()
    {
        { Unit.CreatureType.Zwierzęta,      new[] { "Zwierzęta", "Animals", "Animales", "Animaux", "Tiere" } },
        { Unit.CreatureType.Wilki,          new[] { "Wilki", "Wolves", "Lobos", "Loups", "Wölfe" } },
        { Unit.CreatureType.Szkielety,      new[] { "Szkielety", "Skeletons", "Esqueletos", "Squelettes", "Skelette" } },
        { Unit.CreatureType.OddziałyLordów, new[] { "Oddziały Lordów", "Lord Squads", "Escuadrones de Señores", "Escouades des Seigneurs", "Lordtrupps" } },
        { Unit.CreatureType.Anioł,          new[] { "Anioł", "Angel", "Ángel", "Ange", "Engel" } },
        { Unit.CreatureType.Drzewo,         new[] { "Drzewo", "Tree", "Árbol", "Arbre", "Baum" } },
        { Unit.CreatureType.Wampir,         new[] { "Wampir", "Vampire", "Vampiro", "Vampire", "Vampir" } },
        { Unit.CreatureType.Szczur,         new[] { "Szczur", "Rat", "Rata", "Rat", "Ratte" } },
        { Unit.CreatureType.Duch,           new[] { "Duch", "Ghost", "Fantasma", "Fantôme", "Geist" } }
    };

    void Update()
    {
        if (unit != null)
        {
            int type = 0;
            if (unit.gameObject.GetComponent<Heros>() != null)
                type = 1;
            if (unit.gameObject.GetComponent<Building>() != null)
                type = 2;
            if (unit.gameObject.GetComponent<Spell>() != null)
                type = 3;

            showObject(type);



            text[0].text = unit.Name[PauseMenu.Language];
            if (!unit.attackAP)
            {
                if (unit.Range == 0)
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

                if (unit.Range == 0)
                    staffImage.sprite = sword;
                else
                    staffImage.sprite = bow;
                text[7].text = unit.Attack.ToString();
            }
            text[2].text = unit.Health.ToString();
            text[3].text = unit.Initiative.ToString();
            text[4].text = unit.Defense.ToString();
            if (type == 1)
                text[5].text = unit.GetComponent<Heros>().Evolution == true ? "Max" : (unit.UpgradeLevel + "/" + unit.UpgradeNeed);
            text[6].text = unit.Cost.ToString();

            text[8].text = unit.MagicResist.ToString();
            description.text = unit.DescriptionEdit();

            if (type == 3)
            {
                objects[10].GetComponent<Image>().sprite = unit.GetComponent<SpriteRenderer>().sprite;
            }

            tagi.text = "siema";
            PrzypiszTag(unit, tagi);

        }
    }

     public static void PrzypiszTag(Unit unit, TextMeshProUGUI tagi)
    {
        int lang = PauseMenu.Language;

        if (lang < 0 || lang > 4)
        {
            lang = 0; // Domyślnie polski
        }

        if (unit.Typy == null || unit.Typy.Count == 0)
        {
            tagi.text = "";
            return;
        }

        List<string> tagiList = new();

        foreach (var typ in unit.Typy)
        {
            if (translations.TryGetValue(typ, out var names))
            {
                tagiList.Add(names[lang]);
            }
        }

        tagi.text = string.Join(", ", tagiList);
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
        objects[11].SetActive(unit.Typy.Count != 0);
    }
}
