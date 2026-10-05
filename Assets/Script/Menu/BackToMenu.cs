using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics; // Add this for Stopwatch functionality
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class BackToMenu : MonoBehaviour
{
    public static bool Wygrana;
    public AudioSource musicSource;
    public AudioClip WygranaSound;
    public AudioClip LoseSound;

    public TextMeshProUGUI Time;
    public TextMeshProUGUI Runda;
    public TextMeshProUGUI IloscZyc;
    public TextMeshProUGUI Roll;
    public TextMeshProUGUI MostPopular;
    public TextMeshProUGUI MostDamage;
    public Image MostPopularImage;
    public Image MostDamageImage;
    public TextMeshProUGUI DamageSum;

    public CharacterManager characterManager;

    public void Back()
    {
        SceneManager.LoadScene(0);
        StatsManager.Round = 0;
        StatsManager.win = 0;
        FightManager.IsFight = false;
        StatsManager.life = 3;
        RankedManager.Poddymka = false;
        ShopManager.nizka = 0;
        PlayerManager.Time = 0;
        // PlayerManager.stopwatch;
        PlayerManager.Popular = new Dictionary<int, int>();
        PlayerManager.Strong = new Dictionary<int, int>();
    }


    void Start()
    {
        if (Time != null)
        {
            if(Wygrana)
                musicSource.clip = WygranaSound;
            else
                musicSource.clip = LoseSound;
            musicSource.Play();

            PlayerManager.StopAndLogTime();
            float totalTime = PlayerManager.Time; // Assuming this is where you store the time

            int minutes = (int)(totalTime / 60);
            int seconds = (int)(totalTime % 60);
            UnityEngine.Debug.Log($"Czas wykonania: {minutes}:{seconds:D2}");

            Time.text = $"Czas podejścia: {minutes}:{seconds:D2}";
            Runda.text = $"Runda: {StatsManager.win}";
            IloscZyc.text = $"Pozostałe życia: {StatsManager.life}";
            Roll.text = $"Odświeżenia sklepu: {ShopManager.allRoll}";

            if (PlayerManager.Popular == null || PlayerManager.Popular.Count == 0)
            {
                MostPopular.text = "Zakup jednostek, zdecydownie ułatwi przyszłe zwycięstwa! Pomyśl o tym.";
            }
            else
            {

                var pair = PlayerManager.Popular.Aggregate((l, r) => l.Value > r.Value ? l : r); // dla .NET < 6 i Unity[1][5][7][11]
                var pairStrong = PlayerManager.Strong.Aggregate((l, r) => l.Value > r.Value ? l : r); // dla .NET < 6 i Unity[1][5][7][11]

            

                MostPopular.text = $"Najwięcej, bo aż {pair.Value} razy została zakupiona jednostka {characterManager.characters[pair.Key].GetComponent<Unit>().Name[PauseMenu.Language]}";
                MostPopularImage.sprite = characterManager.characters[pair.Key].GetComponent<SpriteRenderer>().sprite;

                MostDamage.text = $"Spośród wszystkich jednostek najwięcej, bo aż {pairStrong.Value} zadała łącznie {characterManager.characters[pairStrong.Key].GetComponent<Unit>().Name[PauseMenu.Language]}  ";
                MostDamageImage.sprite = characterManager.characters[pairStrong.Key].GetComponent<SpriteRenderer>().sprite;
                int suma = PlayerManager.Strong.Values.Sum();

                DamageSum.text = $"Łącznie zadane obrażenia: {suma}";
            }
        }
    }
}

