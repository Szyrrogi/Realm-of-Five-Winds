using UnityEngine;
using TMPro; // Dodaj przestrzeń nazw dla TextMeshPro
using System.Data.SqlClient;

public class Register : MonoBehaviour
{
    public TMP_InputField usernameInput; // InputField z TextMeshPro
    public TMP_InputField emailInput;    // InputField z TextMeshPro
    public TMP_InputField passwordInput; // InputField z TextMeshPro
    public TextMeshProUGUI feedbackText; // Tekst do wyświetlania informacji zwrotnej (TextMeshPro)

    public GameObject rejestracjaObiekt;
    public GameObject logowanieObiekt;
    public GameObject MenuObiekt;

    // Metoda wywoływana po kliknięciu przycisku rejestracji
    public void RegisterUser()
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

        // Połączenie z bazą danych i wykonanie zapytania SQL
        try
        {
            using (SqlConnection con = DB.Connect(DB.conStr))
            {
                string query = "INSERT INTO Players (Name, Email, Password, LP, Face, Synergies, Achievements) VALUES (@Username, @Email, @Password, 0, 0, @Sy, @Ach)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Sy", "S");
                cmd.Parameters.AddWithValue("@Ach", "S");

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
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
        catch (SqlException ex)
        {
            feedbackText.text = "Błąd bazy danych: " + ex.Message;
        }
        catch (System.Exception ex)
        {
            feedbackText.text = "Wystąpił nieoczekiwany błąd: " + ex.Message;
        }
    }
     public TMP_InputField[] inputFields; // Tablica pól InputField (TMP)
    private int currentIndex = 0;       // Indeks aktualnie aktywnego pola

    void Update()
    {
        if(rejestracjaObiekt.activeSelf)
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
                RegisterUser();
                Debug.Log("enter");
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                rejestracjaObiekt.SetActive(false);
                MenuObiekt.SetActive(true);
            }
        }
    }
}