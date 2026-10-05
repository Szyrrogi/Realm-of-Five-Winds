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
    public bool main;
    public GameObject strzela;
    public GameObject magia;
    public GameObject oba;

    public static DescriptionManager Instance;

    [Header("Epicki opis - animacja")]
    [Tooltip("RectTransform całego panelu opisu. Jeśli puste, użyty zostanie RectTransform tego obiektu.")]
    public RectTransform panelRect;

    [Tooltip("Portret bohatera (SpritePusty) wyświetlany na środku panelu.")]
    public Image portrait;

    [Tooltip("4 paski statystyk wysuwające się w lewo, w kolejności: Attack, Health, Initiative, Defense.")]
    public RectTransform[] leftBars;

    [Tooltip("4 paski statystyk wysuwające się w prawo, w kolejności: Upgrade, Cost, AP, MagicResist.")]
    public RectTransform[] rightBars;

    [Header("Timing")]
    public float hiddenOffsetY = 800f;
    public float panelMoveDuration = 0.35f;
    public float barMoveDuration = 0.3f;
    public float barStagger = 0.05f;

    [Tooltip("O ile niżej (w osi Y) paski startują względem swojej docelowej pozycji - tak by wyłaniały się z dołu, razem z panelem.")]
    public float barRiseDistance = 300f;

    [Tooltip("Współrzędna X, z której wyłaniają się WSZYSTKIE paski (i lewe, i prawe), zanim rozjadą się do swoich docelowych pozycji.")]
    public float barsHiddenX = -220f;

    Vector2 shownPos;
    Vector2 hiddenPos;
    Vector2[] leftTargets;
    Vector2[] leftHidden;
    Vector2[] rightTargets;
    Vector2[] rightHidden;
    Coroutine currentAnim;

    void Start()
    {
        if(main)
            opis = this.gameObject;

        if (panelRect == null)
            panelRect = GetComponent<RectTransform>();

        CacheAnimationPositions();

        if(main)
        {
            Instance = this;
            gameObject.SetActive(false);
        }
    }

    void CacheAnimationPositions()
    {
        shownPos = panelRect.anchoredPosition;
        hiddenPos = shownPos + new Vector2(0f, -hiddenOffsetY);

        if (leftBars != null)
        {
            leftTargets = new Vector2[leftBars.Length];
            leftHidden = new Vector2[leftBars.Length];
            for (int i = 0; i < leftBars.Length; i++)
            {
                if (leftBars[i] == null) continue;
                // zapamiętujemy pozycję, jaką pasek ma TERAZ w scenie - to jest jego cel
                leftTargets[i] = leftBars[i].anchoredPosition;
                // schowany: wspólne X dla wszystkich pasków, niżej niż cel - "wychodzi z dołu"
                leftHidden[i] = new Vector2(barsHiddenX, leftTargets[i].y - barRiseDistance);
                leftBars[i].anchoredPosition = leftHidden[i];
            }
        }

        if (rightBars != null)
        {
            rightTargets = new Vector2[rightBars.Length];
            rightHidden = new Vector2[rightBars.Length];
            for (int i = 0; i < rightBars.Length; i++)
            {
                if (rightBars[i] == null) continue;
                // zapamiętujemy pozycję, jaką pasek ma TERAZ w scenie - to jest jego cel
                rightTargets[i] = rightBars[i].anchoredPosition;
                // schowany: wspólne X dla wszystkich pasków, niżej niż cel - "wychodzi z dołu"
                rightHidden[i] = new Vector2(barsHiddenX, rightTargets[i].y - barRiseDistance);
                rightBars[i].anchoredPosition = rightHidden[i];
            }
        }
    }

    // Wywoływane zamiast DescriptionManager.opis.SetActive(true) + ustawienia unit
    public void Show(Unit u)
    {
        unit = u;
        gameObject.SetActive(true);

        if (currentAnim != null)
            StopCoroutine(currentAnim);
        currentAnim = StartCoroutine(AnimateIn());
    }

    // Wywoływane zamiast DescriptionManager.opis.SetActive(false)
    public void Hide()
    {
        if (!gameObject.activeSelf)
            return;

        if (currentAnim != null)
            StopCoroutine(currentAnim);
        currentAnim = StartCoroutine(AnimateOut());
    }

    void UpdatePortrait()
    {
        if (portrait == null || unit == null)
            return;

        Heros hero = unit.GetComponent<Heros>();
        if (hero != null && hero.SpritePusty != null)
        {
            portrait.sprite = hero.SpritePusty;
            portrait.gameObject.SetActive(true);
        }
        else
        {
            portrait.gameObject.SetActive(false);
        }
    }

    IEnumerator AnimateIn()
    {
        UpdatePortrait();
        HideBarsInstant();
        panelRect.anchoredPosition = hiddenPos;

        // panel i paski ruszają razem
        Coroutine panelMove = StartCoroutine(MoveRect(panelRect, hiddenPos, shownPos, panelMoveDuration));
        Coroutine barsMove = StartCoroutine(AnimateBars(true));

        yield return panelMove;
        yield return barsMove;

        currentAnim = null;
    }

    IEnumerator AnimateOut()
    {
        // paski i panel wracają razem
        Coroutine barsMove = StartCoroutine(AnimateBars(false));
        Coroutine panelMove = StartCoroutine(MoveRect(panelRect, shownPos, hiddenPos, panelMoveDuration));

        yield return barsMove;
        yield return panelMove;

        gameObject.SetActive(false);
        unit = null;
        currentAnim = null;
    }

    void HideBarsInstant()
    {
        if (leftBars != null)
            for (int i = 0; i < leftBars.Length; i++)
                if (leftBars[i] != null)
                    leftBars[i].anchoredPosition = leftHidden[i];

        if (rightBars != null)
            for (int i = 0; i < rightBars.Length; i++)
                if (rightBars[i] != null)
                    rightBars[i].anchoredPosition = rightHidden[i];
    }

    IEnumerator AnimateBars(bool show)
    {
        int count = Mathf.Max(
            leftBars != null ? leftBars.Length : 0,
            rightBars != null ? rightBars.Length : 0);

        List<Coroutine> running = new List<Coroutine>();

        for (int i = 0; i < count; i++)
        {
            if (leftBars != null && i < leftBars.Length && leftBars[i] != null)
            {
                Vector2 control = new Vector2(barsHiddenX, leftTargets[i].y);
                running.Add(StartCoroutine(MoveBarCurved(leftBars[i], leftHidden[i], control, leftTargets[i], show, barMoveDuration)));
            }
            if (rightBars != null && i < rightBars.Length && rightBars[i] != null)
            {
                Vector2 control = new Vector2(barsHiddenX, rightTargets[i].y);
                running.Add(StartCoroutine(MoveBarCurved(rightBars[i], rightHidden[i], control, rightTargets[i], show, barMoveDuration)));
            }

            yield return new WaitForSeconds(barStagger);
        }

        foreach (Coroutine c in running)
            yield return c;
    }

    // Pasek porusza się po krzywej Béziera: ze schowanej pozycji (wspólne barsHiddenX, niżej niż cel - "spod" panelu)
    // najpierw w górę na swoje Y, a potem na bok do swojego X. Chowanie to dokładnie ten sam tor pokonany od tyłu,
    // więc pasek zawsze trafia dokładnie w miejsce, z którego wyszedł.
    IEnumerator MoveBarCurved(RectTransform rect, Vector2 hidden, Vector2 control, Vector2 target, bool show, float duration)
    {
        if (rect == null)
            yield break;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / duration);
            float u = show ? p : 1f - p;
            rect.anchoredPosition = QuadraticBezier(hidden, control, target, u);
            yield return null;
        }
        rect.anchoredPosition = show ? target : hidden;
    }

    static Vector2 QuadraticBezier(Vector2 p0, Vector2 p1, Vector2 p2, float t)
    {
        float u = 1f - t;
        return (u * u * p0) + (2f * u * t * p1) + (t * t * p2);
    }

    IEnumerator MoveRect(RectTransform rect, Vector2 from, Vector2 to, float duration)
    {
        if (rect == null)
            yield break;

        float t = 0f;
        rect.anchoredPosition = from;
        while (t < duration)
        {
            t += Time.deltaTime;
            rect.anchoredPosition = Vector2.Lerp(from, to, t / duration);
            yield return null;
        }
        rect.anchoredPosition = to;
    }

    private static readonly Dictionary<Unit.CreatureType, string[]> translations = new()
    {
        { Unit.CreatureType.Zwierzęta,        new[] { "Zwierzę", "Animal", "Animaleus", "Animal", "Tier" } },
        { Unit.CreatureType.Wilki,           new[] { "Wilk", "Wolve", "Lobo", "Loup", "Wölf" } },
        { Unit.CreatureType.Szkielety,       new[] { "Szkielet", "Skeleton", "Esqueleto", "Squelette", "Skeletton" } },
        { Unit.CreatureType.OddziałyLordów,  new[] { "Oddział Lordów", "Lord Army", "Escuadrone de Señores", "Escouade des Seigneurs", "Lordstrupp" } },
        { Unit.CreatureType.Anioł,          new[] { "Anioł", "Angel", "Ángel", "Ange", "Engel" } },
        { Unit.CreatureType.Drzewo,         new[] { "Drzewo", "Tree", "Árbol", "Arbre", "Baum" } },
        { Unit.CreatureType.Wampir,         new[] { "Wampir", "Vampire", "Vampiro", "Vampire", "Vampir" } },
        { Unit.CreatureType.Szczur,         new[] { "Szczur", "Rat", "Rata", "Rat", "Ratte" } },
        { Unit.CreatureType.Duch,           new[] { "Duch", "Ghost", "Fantasma", "Fantôme", "Geist" } },
		{ Unit.CreatureType.Ozywieniec,		new[] { "Ożywieniec", "Animated Corpse", "Reanimado", "Revenant", "Wiedergänger" } },
		{ Unit.CreatureType.Nekromanta,		new[] { "Nekromanta", "Necromancer", "Nigromante", "Nécromancien", "Nekromant" } },
		{ Unit.CreatureType.Mag,				new[] { "Mag", "Mage", "Mago", "Magicien", "Zauberer" } },
		{ Unit.CreatureType.SlugiAnielskie,   new[] { "Sługa Anielski", "Angelic Servant", "Sirviente Angelical", "Serviteur Angélique", "Engelsdiener" } },
		{ Unit.CreatureType.Plemiennik,		new[] { "Plemiennik", "Tribesman", "Tribal", "Tribal", "Stammes" } },
		{ Unit.CreatureType.MagPustynny,		new[] { "Mag Pustynny", "Desert Magus", "Mago del Desierto", "Mage du Désert", "Wüstenmagier" } },
		{ Unit.CreatureType.Obronca,			new[] { "Obrońca", "Defender", "Defensor", "Défenseur", "Verteidiger" } },
		{ Unit.CreatureType.Wojownik,		new[] { "Wojownik", "Warrior", "Guerrero", "Guerrier", "Krieger" } },
		{ Unit.CreatureType.Inzynier,		new[] { "Inżynier", "Engineer", "Ingeniero", "Ingénieur", "Ingenieur" } },
		{ Unit.CreatureType.Obywatel,		new[] { "Obywatel", "Civilian", "Ciudadano", "Citoyen", "Bürger" } },
		{ Unit.CreatureType.StrazLasu,		new[] { "StrażLasu", "Forrest Guard", "Guardia Forestal", "Garde forestière", "Waldwache" } },
		{ Unit.CreatureType.OpiekunGaju,		new[] { "Opiekun Gaju", "Caretaker of the woods", "Guardián de la arboleda", "Gardien du Bosquet", "Wächter des Hains" } },
		{ Unit.CreatureType.Konstrukt,		new[] { "Konstrukt", "Construct", "Construir", "Construction", "Konstrukt" } }
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
            text[1].text = unit.Attack.ToString();
            // if (!unit.attackAP)
            // {
            //     if (unit.Range == 0)
            //         range.sprite = sword;
            //     else
            //         range.sprite = bow;
            //     text[1].text = unit.Attack.ToString();

            //     text[7].text = unit.AP.ToString();
            //     staffImage.sprite = staff;
            // }
            // else
            // {
            //     range.sprite = staff;
            //     text[1].text = unit.AP.ToString();

            //     if (unit.Range == 0)
            //         staffImage.sprite = sword;
            //     else
            //         staffImage.sprite = bow;
            //     text[7].text = unit.Attack.ToString();
            // }
            text[2].text = unit.Health.ToString();
            text[3].text = unit.Initiative.ToString();
            text[4].text = unit.Defense.ToString();
            if (type == 1)
                text[5].text = unit.GetComponent<Heros>().Evolution == true ? "Max" : (unit.UpgradeLevel + "/" + unit.UpgradeNeed);
            text[6].text = unit.Cost.ToString();
            text[7].text = unit.AP.ToString();
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
        objects[9].SetActive(true);//!(unit.Description[PauseMenu.Language].Length == 0));
        objects[10].SetActive(type == 3);
        objects[11].SetActive(unit.Typy.Count != 0);
        strzela.SetActive(unit.Range != 0);
        magia.SetActive(unit.attackAP);
        oba.SetActive(unit.Range != 0 || unit.attackAP);
    }
}