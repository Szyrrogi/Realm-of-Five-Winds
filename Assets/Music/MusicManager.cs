using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource musicSource; // Główny odtwarzacz muzyki
    public AudioSource SFXSource;

    [System.Serializable]
    public class FractionSoundtracks
    {
        public Fraction.fractionType fraction;
        public AudioClip shopMusic;
        public AudioClip battleMusic;
    }
    public List<AudioClip> Put;
    public AudioClip RemoveShild;
    public AudioClip ZeroDmg;
    public AudioClip SpellLearn;
    public List<FractionSoundtracks> soundtrackList;
    public Unit.Music SoundEffect;
    public static MusicManager musicManager;

    private Dictionary<Fraction.fractionType, FractionSoundtracks> soundtrackMap;
    private AudioClip currentClip;
    private bool inBattle = false;

    void Awake()
    {
        musicManager = GetComponent<MusicManager>();
        soundtrackMap = new Dictionary<Fraction.fractionType, FractionSoundtracks>();
        foreach (var track in soundtrackList)
        {
            soundtrackMap[track.fraction] = track;
        }
    }

    void Start()
    {
        if (Fraction.fractionList != null && Fraction.fractionList.Count > 0)
        {
            SetFraction(Fraction.fractionList[0]); // Domyślnie odtwarzaj muzykę frakcji nr 0
        }
    }

    public void SetFraction(Fraction.fractionType fraction)
    {
        var soundtracks = soundtrackMap[fraction];
        currentClip = inBattle ? soundtracks.battleMusic : soundtracks.shopMusic;
        musicSource.clip = currentClip;
        musicSource.Play();
    }

    public void ToggleMusicMode()
    {
        if (Fraction.fractionList == null || Fraction.fractionList.Count == 0)
        {
            Debug.LogWarning("Brak frakcji do odtworzenia muzyki.");
            return;
        }

        Fraction.fractionType currentFraction = Fraction.fractionList[0];
        var soundtracks = soundtrackMap[currentFraction];

        float currentTime = musicSource.time;
        AudioClip newClip = inBattle ? soundtracks.shopMusic : soundtracks.battleMusic;

        if (newClip != null && newClip != musicSource.clip)
        {
            musicSource.clip = newClip;
            musicSource.time = Mathf.Min(currentTime, newClip.length - 0.1f); // zapobiega przekroczeniu długości nowego utworu
            musicSource.Play();
        }

        inBattle = !inBattle;
    }
    public void SetToBattle()
    {
        if (!inBattle) ToggleMusicMode();
    }

    public void SetToShop()
    {
        if (inBattle) ToggleMusicMode();
    }
    
    public void PlaySound(List<AudioClip> list)
    {
        PlaySound(list[Random.Range(0, list.Count)]);
    }
    public void PlaySound(AudioClip clip)
    {
        if (SFXSource != null && clip != null)
        {
            // Losowanie głośności od 90% do 100% obecnego ustawienia
            float randomVolume = SFXSource.volume * UnityEngine.Random.Range(0.9f, 1f);

            SFXSource.clip = clip;
            SFXSource.volume = randomVolume;
            SFXSource.Play();
        }
    }
}
