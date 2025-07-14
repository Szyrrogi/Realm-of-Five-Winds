using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using UnityEngine.Networking;

public class PlayerManager : MonoBehaviour
{
    [Serializable]
    private class UpdateFaceRequest
    {
        public int PlayerId;
        public int NewFaceId;
    }

    private static readonly string apiURL = "https://api.m455yn.dev/rofw/update-face";

    public static string Version = "0.8.1";
    public static int PlayerFaceId;
    public static string Name = "Player";
    public static int Id = 0;
    public static int LP;

    public Sprite[] spriteFace;

    public Image[] face;
    public TextMeshProUGUI[] textPlayer;
    public GameObject faces;
    public Ranking ranking;

    public TextMeshProUGUI Uwagi;
    public static bool SI;

    public void ShowFace()
    {
        faces.SetActive(!faces.activeSelf);
    }

    public void SetPlayerFace(int player)
    {
        PlayerFaceId = player;
        StartCoroutine(UpdateFaceIdInDatabase(Id, player));
    }

    private IEnumerator UpdateFaceIdInDatabase(int playerId, int newFaceId)
    {
        string json = JsonUtility.ToJson(new UpdateFaceRequest
        {
            PlayerId = playerId,
            NewFaceId = newFaceId
        });

        using UnityWebRequest request = new UnityWebRequest(apiURL, "POST")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json)),
            downloadHandler = new DownloadHandlerBuffer()
        };
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        switch (request.result)
        {
            case UnityWebRequest.Result.Success:
                //Debug.Log("FaceId został pomyślnie zaktualizowany przez API.");
                break;
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.ProtocolError:
              //  Debug.LogError($"Błąd API: {request.responseCode} - {request.error}");
                break;
        }
    }

    public void Logout()
    {
        Login.loggedP = false;
        Name = "Player";
        Id = 0;
        LP = 0;
        ranking.Start();
    }

    public Image RankImage;
    public TextMeshProUGUI LPText;
    public TextMeshProUGUI RankNumber;

    void Update()
    {
        if (LPText != null)
        {
            if (!RankedManager.Ranked)
            {
                RankImage.gameObject.SetActive(false);
                LPText.gameObject.SetActive(false);
                RankNumber.gameObject.SetActive(false);
            }
            else
            {
                RankImage.gameObject.SetActive(true);
                LPText.gameObject.SetActive(true);
                RankNumber.gameObject.SetActive(true);
                int sum = LP;
                if (sum < 1500)
                {
                    RankImage.sprite = EventSystem.eventSystem.GetComponent<RankedManager>().ranksSprite[(int)sum / 300];
                    RankNumber.text = (((int)sum % 300) / 100 + 1).ToString();
                    LPText.text = ((int)sum % 100) + " LP";
                }
                else
                {
                    RankImage.sprite = EventSystem.eventSystem.GetComponent<RankedManager>().ranksSprite[5];
                    RankNumber.text = "";
                    LPText.text = (sum - 1500) + " LP";
                }
            }
        }
        for (int i = 0; i < face.Length; i++)
        {
            face[i].sprite = spriteFace[PlayerFaceId];
            if (textPlayer[i] != null)
                textPlayer[i].text = Name;
        }
    }
    public static bool isSave;
    public void ReadInput(bool save)
    {
        isSave = save;
        if (Login.loggedP || save)
        {
            RankedManager.Ranked = false; //TEST
            if (save || Fraction.fractionList.Count != 0)
            {
                Multi.multi = false;
                Tutorial.tutorial = false;
                SceneManager.LoadScene(4);
            }
            else
            {
                Uwagi.text = "Kliknij na mapę aby wybrać frakcję";
            }
        }
        else
        {
            Uwagi.text = "Zaloguj się";
        }
    }

    public void SIFight()
    {
        Multi.multi = false;
        RankedManager.Ranked = false;
        Tutorial.tutorial = false;
        SI = true;
        SceneManager.LoadScene(9);
    }

    public void Best()
    {
        Multi.multi = false;
        RankedManager.Ranked = false;
        Tutorial.tutorial = false;
        SI = false;
        SceneManager.LoadScene(10);
    }

    public void ReadInputMulti()
    {
        if (Login.loggedP)
        {
            RankedManager.Ranked = false; //TEST
            if (Fraction.fractionList.Count != 0)
            {
                Multi.multi = true;
                SI = false;
                Tutorial.tutorial = false;
                SceneManager.LoadScene(6);
            }
            else
            {
                Uwagi.text = "Kliknij na mapę aby wybrać frakcję";
            }
        }
        else
        {
            Uwagi.text = "Zaloguj się";
        }
    }

    public void StartTutorial()
    {
        Multi.multi = false;
        RankedManager.Ranked = false;
        SI = false;
        Tutorial.tutorial = true;
        SceneManager.LoadScene(5);
    }
    public void StartRanked()
    {
        if (Login.loggedP)
        {
            RankedManager.Ranked = true;
            string savePath2 = Application.dataPath + "/Save/Save2R.json";
            if (File.Exists(savePath2))
            {
                string json = File.ReadAllText(savePath2);
                SaveManager.SaveData2 data = JsonUtility.FromJson<SaveManager.SaveData2>(json);

                if (data.playerId == PlayerManager.Id)
                {
                    isSave = true;
                }
                else
                {
                    isSave = false;
                }
            }
            else
            {
                isSave = false;
            }
            if (isSave || Fraction.fractionList.Count != 0)
            {
                Multi.multi = false;
                Tutorial.tutorial = false;
                SI = false;
                SceneManager.LoadScene(4);
            }
            else
            {
                Uwagi.text = "Kliknij na mapę aby wybrać frakcję";
            }
        }
        else
        {
            Uwagi.text = "Zaloguj się";
        }
    }
}

