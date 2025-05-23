using System;
using System.Collections;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Login : MonoBehaviour
{
    #region LoginRequest
    private static readonly string apiURL = "https://api.m455yn.dev/rofw/login";

    [Serializable]
    public class LoginRequest
    {
        public string Username;
        public string Password;
    }

    [Serializable]
    public class LoginResponse
    {
        public int id;
        public string name;
        public string email;
        public string password;
        public int lp;
        public int face;
        public string synergies;
        public string achievements;
    }
    #endregion

    public TMP_InputField usernameInput; // InputField z TextMeshPro dla nazwy użytkownika
    public TMP_InputField passwordInput; // InputField z TextMeshPro dla hasła
    public TextMeshProUGUI feedbackText; // Tekst do wyświetlania informacji zwrotnej (TextMeshPro)

    public static bool loggedP = false; // Flaga informująca, czy użytkownik jest zalogowany

    public GameObject login;
    public GameObject menu;

    private string savePath; // Ścieżka do pliku player.json

    public Ranking ranking;
    public TMP_InputField[] inputFields; // Tablica pól InputField (TMP)
    private int currentIndex = 0;

    private void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, "Save/player.json");

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            if (!string.IsNullOrEmpty(json))
                AutoLogin(json);
        }
    }
    private void Update()
    {
        if (login.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                currentIndex++;
                if (currentIndex >= inputFields.Length)
                    currentIndex = 0;

                inputFields[currentIndex].Select();
                inputFields[currentIndex].ActivateInputField();
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                LoginUser();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                login.SetActive(false);
                menu.SetActive(true);
            }
        }
    }

    public void LoginUser()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username))
        {
            feedbackText.text = "Proszę podać nazwę użytkownika.";
            return;
        }

        if (string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Proszę podać hasło.";
            return;
        }

        StartCoroutine(LoginUserCoroutine(username, password));
    }
    private IEnumerator LoginUserCoroutine(string username, string password)
    {
        LoginRequest loginRequest = new LoginRequest
        {
            Username = username,
            Password = password
        };

        string json = JsonUtility.ToJson(loginRequest);

        using (UnityWebRequest request = new UnityWebRequest(apiURL, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                feedbackText.text = "Błąd połączenia: " + request.error;
                loggedP = false;
            }
            else if (request.responseCode == 401)
            {
                feedbackText.text = "Nieprawidłowa nazwa użytkownika lub hasło.";
                loggedP = false;
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(jsonResponse);

                PlayerManager.Name = response.name;
                PlayerManager.LP = response.lp;
                PlayerManager.PlayerFaceId = response.face;
                PlayerManager.Id = response.id;

                string savePathSynergy = Application.dataPath + "/Save/Synergy.txt";
                File.WriteAllText(savePathSynergy, response.synergies);

                string savePathAchievements = Application.dataPath + "/Save/Achivments.txt";
                File.WriteAllText(savePathAchievements, response.achievements);

                SaveLoginData(username, password);

                login.SetActive(false);
                menu.SetActive(true);
                loggedP = true;
                ranking.Start();
            }
        }
    }
    private void SaveLoginData(string username, string password)
    {
        LoginRequest loginData = new LoginRequest
        {
            Username = username,
            Password = password
        };

        string json = JsonUtility.ToJson(loginData, true);
        string saveDirectory = Path.GetDirectoryName(savePath);

        if (!Directory.Exists(saveDirectory))
            Directory.CreateDirectory(saveDirectory);

        File.WriteAllText(savePath, json);
    }
    private void AutoLogin(string json)
    {
        LoginRequest loginRequest = JsonUtility.FromJson<LoginRequest>(json);

        usernameInput.text = loginRequest.Username;
        passwordInput.text = loginRequest.Password;
        LoginUser();
    }
    public bool IsLoggedP()
    {
        return loggedP;
    }
}