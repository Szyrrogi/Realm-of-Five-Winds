using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUp : MonoBehaviour
{
    public TextMeshProUGUI popText;

    public void SetText(string text, Color color)
    {
        popText.color = color;
        popText.text = text;
    }

    void Start()
    {
        Destroy(gameObject, 3f);
    }
}
