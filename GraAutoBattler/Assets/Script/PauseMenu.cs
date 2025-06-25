using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System;



public class PauseMenu : MonoBehaviour
{
     [Header("UI")]
    public GameObject pausePanel;
    public Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Image[] languageFlags;
    public static int language;

     [Header("Audio Mixers")]
    public AudioMixer  audioMixer;
    public static bool isFull = true;

    private Resolution[] resolutions;
    private bool isPaused = false;
    void Awake()
    {
        resolutions = Screen.resolutions
            .Where(r => Mathf.Approximately((float)r.width / r.height, 16f / 9f))
            .ToArray();

        int savedWidth = PlayerPrefs.GetInt("ResolutionWidth", Screen.currentResolution.width);
        int savedHeight = PlayerPrefs.GetInt("ResolutionHeight", Screen.currentResolution.height);
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        
        Screen.SetResolution(savedWidth, savedHeight, isFullscreen);
    }



    void Start()
    {


        // Filtrowanie tylko rozdzielczości 16:9
        resolutions = Screen.resolutions
            //.Where(r => Mathf.Approximately((float)r.width / r.height, 16f / 9f))
            .GroupBy(r => new { r.width, r.height }) // Grupujemy po szerokości i wysokości
            .Select(g => g.First()) // Wybieramy pierwszy element z każdej grupy
            .ToArray();

        resolutionDropdown.ClearOptions();
        int currentResIndex = 0;
        var options = new System.Collections.Generic.List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
            currentResIndex = Mathf.Clamp(savedIndex, 0, resolutions.Length - 1);

        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();

        // Ustawienie stanu Toggle na podstawie PlayerPrefs.
        fullscreenToggle.isOn = Screen.fullScreen;

        pausePanel.SetActive(false);
        Time.timeScale = 1f;

        // if (resolutions[i].width == Screen.currentResolution.width &&
        //     resolutions[i].height == Screen.currentResolution.height)
        // {
        //     currentResIndex = i;
        // }
            float masterVol = PlayerPrefs.GetFloat("MasterVolume", 1f); // domyślnie 1.0
            float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
            float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);

            masterVolumeSlider.value = masterVol;
            musicVolumeSlider.value = musicVol;
            sfxVolumeSlider.value = sfxVol;

            SetMasterVolume(masterVol);
            SetMusicVolume(musicVol);
            SetSFXVolume(sfxVol);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
        
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);

        PlayerPrefs.SetInt("ResolutionWidth", res.width);
        PlayerPrefs.SetInt("ResolutionHeight", res.height);
        PlayerPrefs.SetInt("ResolutionIndex", index);
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen; // Ustawienie trybu pełnoekranowego.
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0); // Zapisanie do PlayerPrefs (1 dla true, 0 dla false).
        PlayerPrefs.Save();
        fullscreenToggle.isOn = isFullscreen; // Upewnij się, że stan toggle'a jest spójny.
    }





public void SetMasterVolume(float volume)
{
    float newVolume = (volume == 0) ? -80f : Mathf.Log10(volume) * 20;
    audioMixer.SetFloat("Master", newVolume);
    PlayerPrefs.SetFloat("MasterVolume", volume);
    PlayerPrefs.Save();
}

public void SetMusicVolume(float volume)
{
    float newVolume = (volume == 0) ? -80f : Mathf.Log10(volume) * 20;
    audioMixer.SetFloat("MusicVolume", newVolume);
    PlayerPrefs.SetFloat("MusicVolume", volume);
    PlayerPrefs.Save();
}

public void SetSFXVolume(float volume)
{
    float newVolume = (volume == 0) ? -80f : Mathf.Log10(volume) * 20;
    audioMixer.SetFloat("SFXVolume", newVolume);
    PlayerPrefs.SetFloat("SFXVolume", volume);
    PlayerPrefs.Save();
}


    public void SelectLanguage(int newlanguage)
    {
        //Debug.Log("Language set to: " + langCode);
        // Tu możesz podpiąć system lokalizacji np. używając Localization package
        language = newlanguage;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // zmień nazwę sceny na własną
        StatsManager.Round = 0;
        StatsManager.win = 0;
        FightManager.IsFight = false;
        StatsManager.life = 3;
        RankedManager.Poddymka = false;
        ShopManager.nizka = 0;  
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
