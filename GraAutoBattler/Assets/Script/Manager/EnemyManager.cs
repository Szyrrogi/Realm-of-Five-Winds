using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public List<Sprite> FaceIcon;
    public Image FaceImage;

    public void SetPlayer(string name,int nr)
    {
        Name.text = name;
        FaceImage.sprite = FaceIcon[nr];
    }
}
