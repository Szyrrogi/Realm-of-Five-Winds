using UnityEngine;
using TMPro;
using System.Data.SqlClient;
using System.IO;
using System;

public class Login : MonoBehaviour
{
    public TMP_InputField usernameInput; // InputField z TextMeshPro dla nazwy użytkownika
    public TMP_InputField passwordInput; // InputField z TextMeshPro dla hasła
    public TextMeshProUGUI feedbackText; // Tekst do wyświetlania informacji zwrotnej (TextMeshPro)

    public static bool zalogowano = false; // Flaga informująca, czy użytkownik jest zalogowany

    public GameObject login;
    public GameObject menu;

    private string savePath; // Ścieżka do pliku player.json

    public Ranking ranking;
    public TMP_InputField[] inputFields; // Tablica pól InputField (TMP)
    private int currentIndex = 0;  

     void Update()
    {
        if(login.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                // Przełącz na następne pole
                currentIndex++;
                if (currentIndex >= inputFields.Length)
                {
                    currentIndex = 0; // Zawróć do pierwszego pola, jeśli osiągnięto koniec
                }

                // Ustaw fokus na następnym polu
                inputFields[currentIndex].Select();
                inputFields[currentIndex].ActivateInputField();
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                LoginUser();
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                login.SetActive(false);
                menu.SetActive(true);
            }
        }
    }

    private void Start()
    {
        // Ustaw ścieżkę do pliku player.json w folderze Save
        savePath = Path.Combine(Application.persistentDataPath, "Save/player.json");

        // Sprawdź, czy plik istnieje i nie jest pusty
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            if (!string.IsNullOrEmpty(json))
            {
                // Próbuj automatycznie zalogować użytkownika
                AutoLogin(json);
            }
        }
    }

    // Metoda wywoływana po kliknięciu przycisku logowania
    public void LoginUser()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        // Walidacja danych
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

        // Połączenie z bazą danych i sprawdzenie poprawności danych logowania
        try
        {
            using (SqlConnection con = DB.Connect(DB.conStr))
            {
                string query = "SELECT Id, Name, Email, Password, LP, Face FROM Players WHERE Name = @Username AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        // Użytkownik został znaleziony, logowanie udane
                        while (reader.Read())
                        {
                            PlayerManager.Name = reader["Name"].ToString();
                            string email = reader["Email"].ToString();
                            string userPassword = reader["Password"].ToString();
                            PlayerManager.LP = reader.GetInt32(reader.GetOrdinal("LP"));
                            PlayerManager.PlayerFaceId = reader.GetInt32(reader.GetOrdinal("Face"));
                            PlayerManager.Id = reader.GetInt32(reader.GetOrdinal("Id"));

                            // Zapisz dane logowania do pliku
                            SaveLoginData(username, password);

                            // Ustaw flagę zalogowano na true
                            login.SetActive(false);
                            menu.SetActive(true);
                            zalogowano = true;
                            ranking.Start();
                        }
                    }
                    else
                    {
                        // Użytkownik nie został znaleziony, logowanie nieudane
                        feedbackText.text = "Nieprawidłowa nazwa użytkownika lub hasło.";
                        zalogowano = false;
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            feedbackText.text = "Błąd bazy danych: " + ex.Message;
            zalogowano = false;
        }
        catch (System.Exception ex)
        {
            feedbackText.text = "Wystąpił nieoczekiwany błąd: " + ex.Message;
            zalogowano = false;
        }
    }

    // Metoda do zapisywania danych logowania do pliku JSON
    private void SaveLoginData(string username, string password)
    {
        // Utwórz obiekt z danymi logowania
        LoginData loginData = new LoginData
        {
            Username = username,
            Password = password
        };

        // Serializuj obiekt do JSON
        string json = JsonUtility.ToJson(loginData, true);

        // Upewnij się, że folder Save istnieje
        string saveDirectory = Path.GetDirectoryName(savePath);
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }

        // Zapisz JSON do pliku
        File.WriteAllText(savePath, json);
        Debug.Log(Application.persistentDataPath);
    }

    // Metoda do automatycznego logowania
    private void AutoLogin(string json)
    {
        // Deserializuj JSON do obiektu LoginData
        LoginData loginData = JsonUtility.FromJson<LoginData>(json);

        // Ustaw wartości w polach InputField
        usernameInput.text = loginData.Username;
        passwordInput.text = loginData.Password;

        // Spróbuj zalogować użytkownika
        LoginUser();
    }

    // Metoda do sprawdzenia, czy użytkownik jest zalogowany
    public bool CzyZalogowano()
    {
        return zalogowano;
    }

    // Klasa do przechowywania danych logowania
    [System.Serializable]
    private class LoginData
    {
        public string Username;
        public string Password;
    }
}