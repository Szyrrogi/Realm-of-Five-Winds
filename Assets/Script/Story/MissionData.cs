using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Jedna misja trybu fabularnego. Tworzysz przez Create → Fabuła → Misja.</summary>
[CreateAssetMenu(fileName = "Misja", menuName = "Fabuła/Misja")]
public class MissionData : ScriptableObject
{
    [Tooltip("Unikalny identyfikator, np. ludzie_1. Po nim zapisywany jest postęp – nie zmieniaj go po wydaniu.")]
    public string missionId;
    public string title;
    [TextArea(2, 6)] public string description;

    [Header("Gracz")]
    [Tooltip("Jednostki i budynki, które mogą pojawić się w sklepie (prefaby z listy CharacterManager.characters).")]
    public List<GameObject> playerPool = new List<GameObject>();
    [Tooltip("true = cała pula od pierwszej rundy, bez limitu gwiazdek.")]
    public bool ignoreStarLimit;
    public int startMoney = 5;
    public int startIncome = 6;
    public int lives = 3;
    [Tooltip("Puste = misja bez bohatera.")]
    public BohaterData fixedHero;
    [Range(1, 3)] public int fixedHeroLevel = 1;

    [Header("Fale (kolejne walki)")]
    public List<MissionWave> waves = new List<MissionWave>();

    [Header("Koniec misji")]
    public List<DialogueLine> onVictory = new List<DialogueLine>();
    public List<DialogueLine> onDefeat = new List<DialogueLine>();
}

[Serializable]
public class MissionWave
{
    public string enemyName = "Przeciwnik";
    public int enemyFace;
    [Tooltip("Skład przeciwnika: plik .json w formacie Zapis.json (np. z Assets/Wave) albo tekst ~2...")]
    public TextAsset comp;
    [Tooltip("Losuj kolejność rzędów przed walką (wampiry nie zawsze na dole).")]
    public bool shuffleRows = true;
    public List<DialogueLine> dialogueBefore = new List<DialogueLine>();
    public List<DialogueLine> dialogueAfterWin = new List<DialogueLine>();
}

[Serializable]
public class DialogueLine
{
    public string speaker;
    public Sprite portrait;
    [Tooltip("Portret po prawej stronie (zwykle przeciwnik).")]
    public bool rightSide;
    [TextArea(2, 5)] public string text;
}
