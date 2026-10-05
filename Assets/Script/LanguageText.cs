using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LanguageText : MonoBehaviour
{
    public Text textUI;
    public TextMeshProUGUI textUIPro;


    public string[] text;

    void Update()
    {
        if(textUI != null)
            textUI.text = text[PauseMenu.Language];
        if(textUIPro != null)
            textUIPro.text = text[PauseMenu.Language];
    }
}
