using Discord;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class DiscordManager : MonoBehaviour
{
    public static DiscordManager Instance { get; private set; }

    [SerializeField]
    private Discord.Discord discord;

    [Header("Rich Presence Settings")]

    [SerializeField]
    private string _details;
    public string Details
    {
        get => _details;
        set
        {
            _details = value;
            UpdateRichPresence(details: _details);
        }
    }

    [SerializeField]
    private string _state;

    public string State
    {
        get => _state;
        set
        {
            _state = value;
            UpdateRichPresence(state: _state);
        }
    }

    [SerializeField]
    private string largeImage;

    [SerializeField]
    private string largeText;

    [SerializeField]
    private string smallImage;

    [SerializeField]
    private string smallText;

    [SerializeField]
    private int currentPartySize;

    [SerializeField]
    private int maxPartySize;

    [SerializeField]
    private long timestamp;

    [SerializeField]
    private long endTimestamp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make sure the instance is persistent
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (discord != null)
        {
            discord.Dispose();
            discord = null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update the rich presence data based on the new scene
        //UpdateRichPresence(details: $"W {scene.name}", updateTimestamp: true);
    }

    void Start()
    {
        if (!IsDiscordRunning())
        {
            UnityEngine.Debug.LogWarning("Discord is not running. Rich presence will not be initialized.");
            return;
        }

        try
        {
            discord = new Discord.Discord(1268613606937985056, (long)CreateFlags.Default);
            UpdateRichPresence(updateTimestamp: true);
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"Failed to initialize Discord: {ex.Message}");
        }
    }

    void UpdateRichPresence(string details = null, string state = null, string largeImage = null, string largeText = null, string smallImage = null, string smallText = null, int currentPartySize = -1, int maxPartySize = -1, bool updateTimestamp = false, long endTimestamp = -1)
    {
        if (discord == null) return;

        this._details = details ?? this._details;
        this._state = state ?? this._state;
        this.largeImage = largeImage ?? this.largeImage;
        this.largeText = largeText ?? this.largeText;
        this.smallImage = smallImage ?? this.smallImage;
        this.smallText = smallText ?? this.smallText;
        this.currentPartySize = currentPartySize != -1 ? currentPartySize : this.currentPartySize;
        this.maxPartySize = maxPartySize != -1 ? maxPartySize : this.maxPartySize;
        this.timestamp = updateTimestamp ? new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() : this.timestamp;
        this.endTimestamp = endTimestamp != -1 ? endTimestamp : this.endTimestamp;

        if (discord != null)
        {
            ActivityManager activityManager = discord.GetActivityManager();
            Activity activity = new()
            {
                Details = this._details,
                State = this._state,
                Assets = { LargeImage = this.largeImage, LargeText = this.largeText, SmallImage = this.smallImage, SmallText = this.smallText },
                Timestamps = { Start = this.timestamp, End = this.endTimestamp },
                Party = { Size = { CurrentSize = this.currentPartySize, MaxSize = this.maxPartySize } }
            };
            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res == Result.Ok)
                {
                    Debug.Log("Rich presence updated successfully.");
                }
                else
                {
                    Debug.LogError("Failed to update rich presence.");
                }
            });
        }
    }

    void Update()
    {
        if (discord != null)
        {
            discord.RunCallbacks();
        }
    }

    private bool IsDiscordRunning()
    {
        Process[] processes = Process.GetProcessesByName("Discord");
        return processes.Length > 0;
    }
}
