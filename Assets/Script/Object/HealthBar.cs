using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Referencje")]
    public Heros hero; // Dodana referencja do Twojego Herosa

    [Header("Slidery")]
    public Slider mainSlider;
    public Slider easeSlider;

    [Header("Transformy Paska (Szerokość)")]
    public RectTransform firstRect;
    public RectTransform secondRect;

    [Header("Ustawienia Animacji")]
    public float lerpSpeed = 5f;

    // Funkcja Update wywoływana co klatkę, aby stale monitorować zdrowie Herosa
    void Update()
    {
        if (hero != null)
        {
            UpdateBar(hero.Health, hero.MaxHealth);
        }
    }

    public void UpdateBar(float currentValue, float maxValue)
    {
        // 1. Aktualizacja maksymalnej wartości i szerokości paska
        if (mainSlider.maxValue != maxValue)
        {
            mainSlider.maxValue = maxValue;
            easeSlider.maxValue = maxValue;

            // Obliczanie nowej szerokości na bazie max HP
            float newWidth = 20f + Mathf.Sqrt(maxValue);

            // Aktualizacja pierwszego RectTransform
            if (firstRect != null)
            {
                firstRect.sizeDelta = new Vector2(newWidth, firstRect.sizeDelta.y);
            }
            
            // Aktualizacja drugiego RectTransform
            if (secondRect != null)
            {
                secondRect.sizeDelta = new Vector2(newWidth, secondRect.sizeDelta.y);
            }
        }

        // 2. Główny pasek skacze natychmiast
        if (mainSlider.value != currentValue)
        {
            mainSlider.value = currentValue;
        }

        // 3. Pasek pod spodem płynnie opada
        if (mainSlider.value != easeSlider.value)
        {
            easeSlider.value = Mathf.Lerp(easeSlider.value, currentValue, lerpSpeed * Time.deltaTime);
        }
    }
}