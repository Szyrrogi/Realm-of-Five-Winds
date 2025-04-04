using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    public static bool MusicPlay;
    public Sprite MusicOff;
    public Sprite MusicOn;
    public Image image;
    public AudioSource audioSource;
    public GameObject Settings;

    public AudioClip[] music;


    void Start()
    {
        if (!MusicPlay)
        {
            MusicPlay = true;
            ChangeMusic();
        }
    }
    public void SetMusic(int nr)
    {
        audioSource.clip = music[nr];
        audioSource.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Settings.SetActive(!Settings.activeSelf);
        }
    }

    public void ChangeMusic()
    {
        MusicPlay = !MusicPlay; 
        if(MusicPlay)
        {
            image.sprite = MusicOn;
        }
        if(!MusicPlay)
        {
            image.sprite = MusicOff;
        }
        ToggleMusic();
    }

    public void ToggleMusic()
    {
        audioSource.mute = !audioSource.mute;
    }
}
