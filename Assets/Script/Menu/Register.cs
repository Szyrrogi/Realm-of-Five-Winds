using UnityEngine;
using TMPro;
using System.Data.SqlClient;
using System.Threading.Tasks; // Dodano obsługę asynchroniczności

public class Register : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI feedbackText;

    public GameObject rejestracjaObiekt;
    public GameObject logowanieObiekt;
    public GameObject MenuObiekt;

    // Metoda asynchroniczna wywoływana po kliknięciu przycisku rejestracji
    public async void RegisterUser()
    {
        string username = usernameInput.text;
        string email = emailInput.text;
        string password = passwordInput.text;

        // Walidacja danych
        if (string.IsNullOrEmpty(username) || username.Length > 13)
        {
            feedbackText.text = "Nazwa użytkownika musi mieć maksymalnie 13 znaków.";
            return;
        }

        if (string.IsNullOrEmpty(email) || !email.Contains("@"))
        {
            feedbackText.text = "Proszę podać poprawny adres e-mail.";
            return;
        }

        if (string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Proszę podać hasło.";
            return;
        }

        // Asynchroniczne logowanie do bazy danych
        try
        {
            using (SqlConnection con = new SqlConnection(DB.conStr))
            {
                await con.OpenAsync(); // Otwieramy połączenie w tle (nie ścina gry)

                string query = "INSERT INTO Players (Name, Email, Password, LP, Face, Synergies, Achievements) VALUES (@Username, @Email, @Password, 0, 0, @Sy, @Ach)";
                
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Sy", "S");
                    cmd.Parameters.AddWithValue("@Ach", "S");

                    // Wykonujemy zapytanie w tle
                    int result = await cmd.ExecuteNonQueryAsync();

                    if (result > 0)
                    {
                        // Zapisz dane logowania do pliku player.json
                        SaveLoginData(username, password);
                        
                        rejestracjaObiekt.SetActive(false);
                        logowanieObiekt.SetActive(true);
                        feedbackText.text = "Rejestracja zakończona pomyślnie!";
                    }
                    else
                    {
                        feedbackText.text = "Wystąpił błąd podczas rejestracji.";
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            // Dobrą praktyką jest unikanie wyświetlania surowych błędów SQL graczowi, ale dla testów jest OK
            feedbackText.text = "Błąd bazy danych: Brak połączenia z serwerem.";
            Debug.LogError("Błąd SQL podczas rejestracji: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            feedbackText.text = "Wystąpił nieoczekiwany błąd.";
            Debug.LogError("Nieoczekiwany błąd rejestracji: " + ex.Message);
        }
    }

    private void SaveLoginData(string username, string password)
    {
        var loginData = new Login.LoginRequest
        {
            Username = username,
            Password = password
        };

        string json = JsonUtility.ToJson(loginData, true);
        string savePath = Application.dataPath + "/Save/player.json";
        string saveDirectory = System.IO.Path.GetDirectoryName(savePath);

        if (!System.IO.Directory.Exists(saveDirectory))
            System.IO.Directory.CreateDirectory(saveDirectory);

        System.IO.File.WriteAllText(savePath, json);
    }

    public TMP_InputField[] inputFields; // Tablica pól InputField (TMP)
    private int currentIndex = 0;       // Indeks aktualnie aktywnego pola

    void Update()
    {
        if (rejestracjaObiekt.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                // Przełącz na następne pole
                currentIndex++;
                if (currentIndex >= inputFields.Length)
                {
                    currentIndex = 0; // Zawróć do pierwszego pola
                }

                // Ustaw fokus na następnym polu
                inputFields[currentIndex].Select();
                inputFields[currentIndex].ActivateInputField();
            }
            
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                RegisterUser();
                Debug.Log("Wciśnięto enter podczas rejestracji");
            }
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                rejestracjaObiekt.SetActive(false);
                MenuObiekt.SetActive(true);
            }
        }
    }
}