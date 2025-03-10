using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventManager : MonoBehaviour
{
    public float TimeLeft;
    public Image image;
    public Image eventObject;
    public TextMeshProUGUI description;
    private bool isFading = false;
    private float delayTime = 1f; // Opóźnienie przed rozpoczęciem zanikania

    public void SetEvent(Sprite sprite, string Text)
    {
        TimeLeft = 2f; // Całkowity czas zanikania (2 sekundy)
        delayTime = 1f; // Resetowanie opóźnienia
        image.sprite = sprite;
        description.text = Text;

        // Przywróć pełną widoczność
        SetAlpha(image, 1f);
        SetAlpha(eventObject, 1f);
        SetAlpha(description, 1f);

        isFading = true;
    }

    void Update()
    {
        if (isFading)
        {
            if (delayTime > 0)
            {
                // Czekaj przez 1 sekundę przed rozpoczęciem zanikania
                delayTime -= Time.deltaTime;
            }
            else if (TimeLeft > 0)
            {
                // Rozpocznij zanikanie po upływie opóźnienia
                TimeLeft -= Time.deltaTime;

                // Zmniejsz przezroczystość w czasie
                float alpha = TimeLeft / 2f; // Zanikanie przez 2 sekundy
                SetAlpha(image, alpha);
                SetAlpha(eventObject, alpha);
                SetAlpha(description, alpha);
            }
            else
            {
                // Zakończ zanikanie
                isFading = false;
            }
        }
    }

    private void SetAlpha(Image img, float alpha)
    {
        Color color = img.color;
        color.a = alpha;
        img.color = color;
    }

    private void SetAlpha(TextMeshProUGUI text, float alpha)
    {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }
}