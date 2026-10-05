using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MultiPlayerObject : MonoBehaviour
{
   // public TextMeshProUGUI HPCounter;
    public TextMeshProUGUI NameText;

    public void SetObject(int hp, string name, int image)
    {
        //HPCounter.text = hp.ToString();
        NameText.text = name.ToString();
        GetComponent<Image>().sprite = EventSystem.eventSystem.GetComponent<PlayerManager>().spriteFace[image];
    }
}