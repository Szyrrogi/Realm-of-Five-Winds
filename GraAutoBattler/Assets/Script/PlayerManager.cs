using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Data.SqlClient;

public class PlayerManager : MonoBehaviour
{
    public static string Version = "0.1.1";
    public TMP_InputField inputField;
    public static int PlayerFaceId;
    public static string Name = "Player";

    public Sprite[] spriteFace;

    public Image face;
    public TextMeshProUGUI textPlayer;


    public void SetPlayerFace(int player)
    {
        PlayerFaceId = player;
    }

    void Update()
    {
        face.sprite = spriteFace[PlayerFaceId];
        if(textPlayer != null)
            textPlayer.text = Name;
    }

    public void ReadInput()
{
    if (inputField.text.Length > 0)
    {
        Name = inputField.text;
    }

    // Przejście do sceny
    SceneManager.LoadScene(3);
}
    
}
