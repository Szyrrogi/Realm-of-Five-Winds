using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Okno dialogu (UI). Klik albo dowolny klawisz = następna kwestia.</summary>
public class DialogueBox : MonoBehaviour
{
    public static bool Showing; // blokuje start walki

    public GameObject panel;
    public Image portraitLeft;
    public Image portraitRight;
    public TextMeshProUGUI speaker;
    public TextMeshProUGUI text;

    void Awake()
    {
        panel.SetActive(false);
    }

    public IEnumerator Play(List<DialogueLine> lines)
    {
        if (lines == null || lines.Count == 0) yield break;

        Showing = true;
        panel.SetActive(true);
        foreach (DialogueLine line in lines)
        {
            speaker.text = line.speaker;
            text.text = line.text;
            portraitLeft.gameObject.SetActive(!line.rightSide && line.portrait != null);
            portraitRight.gameObject.SetActive(line.rightSide && line.portrait != null);
            if (line.rightSide) portraitRight.sprite = line.portrait;
            else portraitLeft.sprite = line.portrait;

            yield return null; // nie łap kliknięcia, które zamknęło poprzednią kwestię
            while (!Input.GetMouseButtonDown(0) && !Input.anyKeyDown) yield return null;
        }
        panel.SetActive(false);
        Showing = false;
    }
}
