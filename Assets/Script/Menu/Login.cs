using System;
using System.Collections;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using System.Data.SqlClient;
using System.Threading.Tasks;

public class Login : MonoBehaviour
{
    #region LoginRequest
    private static readonly string apiURL = "https://api.m455yn.io/rofw/login";

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
        savePath = Application.dataPath + "/Save/player.json";

        

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

    public async void LoginUser()
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

        // Uruchamiamy asynchroniczne logowanie bezpośrednio z bazy
        await LoginUserAsync(username, password);
    }

    private async Task LoginUserAsync(string username, string password)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(DB.conStr))
            {
                await con.OpenAsync();

                // UWAGA: Zakładam nazwy kolumn na podstawie Twojej klasy LoginResponse.
                // Jeśli w bazie nazywają się inaczej (np. Username zamiast Name), musisz to tutaj podmienić!
                string query = @"
                    SELECT Id, Name, Email, LP, Face, Synergies, Achievements 
                    FROM Players 
                    WHERE Name = @Username AND Password = @Password";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Zabezpieczenie przed SQL Injection za pomocą parametrów
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password); 

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // Znaleziono użytkownika - przypisujemy dane
                            PlayerManager.Id = Convert.ToInt32(reader["Id"]);
                            PlayerManager.Name = reader["Name"].ToString();
                            
                            // Zabezpieczenie przed wartościami null w bazie
                            PlayerManager.LP = reader["LP"] != DBNull.Value ? Convert.ToInt32(reader["LP"]) : 0;
                            PlayerManager.PlayerFaceId = reader["Face"] != DBNull.Value ? Convert.ToInt32(reader["Face"]) : 0;

                            string synergies = reader["Synergies"] != DBNull.Value ? reader["Synergies"].ToString() : "";
                            string achievements = reader["Achievements"] != DBNull.Value ? reader["Achievements"].ToString() : "";

                            // Tworzenie folderu i zapis plików
                            string saveFolder = Application.dataPath + "/Save";
                            Directory.CreateDirectory(saveFolder);

                            string savePathSynergy = saveFolder + "/Synergy.txt";
                            string savePathAchievements = saveFolder + "/Achivments2.txt";

                            File.WriteAllText(savePathSynergy, synergies);
                            File.WriteAllText(savePathAchievements, achievements);

                            SaveLoginData(username, password);

                            login.SetActive(false);
                            menu.SetActive(true);
                            loggedP = true;
                            ranking.Start();
                        }
                        else
                        {
                            // Brak rekordów oznacza złe hasło lub login
                            feedbackText.text = "Nieprawidłowa nazwa użytkownika lub hasło.";
                            loggedP = false;
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            feedbackText.text = "Błąd bazy danych: Brak połączenia.";
            Debug.LogError("Błąd SQL: " + ex.Message);
            loggedP = false;
        }
        catch (Exception ex)
        {
            feedbackText.text = "Wystąpił błąd podczas logowania.";
            Debug.LogError("Nieoczekiwany błąd: " + ex.Message);
            loggedP = false;
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
    public static bool FirsAutoLog;

    private void AutoLogin(string json)
    {
        if (!FirsAutoLog)
        {
            FirsAutoLog = true;
            LoginRequest loginRequest = JsonUtility.FromJson<LoginRequest>(json);

            usernameInput.text = loginRequest.Username;
            passwordInput.text = loginRequest.Password;
            LoginUser();
        }
    }
    public bool IsLoggedP()
    {
        return loggedP;
    }
}