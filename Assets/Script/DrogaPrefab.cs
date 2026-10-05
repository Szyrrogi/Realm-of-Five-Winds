using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DrogaPrefab : MonoBehaviour
{
    [Header("Przeciągnij tutaj pola w odpowiedniej kolejności (0, 1, 2...)")]
    public List<Pole> polaNaDrodze;

    [Header("Przycisk ulepszenia (opcjonalny, bez Canvas/UI)")]
    // Zwykły SpriteRenderer (np. strzałka/ikonka) umieszczony w scenie/prefabie jako zwykły obiekt.
    // Kliknięcie wykrywane jest w Linia.cs po dystansie myszy od tego obiektu — bez Canvas i Buttona.
    // Zostaw puste na prefabie najwyższego poziomu (Lvl4) — nie ma czego dalej ulepszać.
    public SpriteRenderer przyciskSprite;
    // Opcjonalny tekst 3D (komponent TextMeshPro, NIE TextMeshProUGUI) z kosztem następnego ulepszenia.
    public TextMeshPro tekstKosztu;
}