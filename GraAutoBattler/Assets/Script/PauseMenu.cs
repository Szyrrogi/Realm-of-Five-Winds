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

     [Header("Audio Mixers")]
    public AudioMixer  audioMixer;

    private Resolution[] resolutions;
    private bool isPaused = false;
    void Awake()
    {
        // Najpierw pobieramy dostępne rozdzielczości
        resolutions = Screen.resolutions
            .Where(r => Mathf.Approximately((float)r.width / r.height, 16f / 9f))
            .ToArray();

        // Potem wybieramy Full HD jeśli dostępne
        Resolution hd = resolutions.FirstOrDefault(r => r.width == 1920 && r.height == 1080);
        // if (hd.width != 0)
        // {
        //     Screen.SetResolution(hd.width, hd.height, Screen.fullScreen);
        // }
    }


    void Start()
    {
        // Filtrowanie tylko rozdzielczości 16:9
        resolutions = Screen.resolutions
            .Where(r => Mathf.Approximately((float)r.width / r.height, 16f / 9f))
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

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();

        fullscreenToggle.isOn = !Screen.fullScreen;

        pausePanel.SetActive(false);
        Time.timeScale = 1f;
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
        Debug.Log("  " + index);
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        Debug.Log(isFullscreen);
    }



    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }

    public void SelectLanguage(string langCode)
    {
        Debug.Log("Language set to: " + langCode);
        // Tu możesz podpiąć system lokalizacji np. używając Localization package
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // zmień nazwę sceny na własną
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
