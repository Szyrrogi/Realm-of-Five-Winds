using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine;
using System.Linq; 
using System.IO;
public class Multi : MonoBehaviour
{
    public static bool multi;
    public static string comp;
    public GameObject StartGame;
    public GameObject ReadyButton;
    public TextMeshProUGUI timerText; // Dodaj referencję do TextMeshProUGUI w Inspectorze

    private float timeRemaining = 50f; // Czas startowy: 60 sekund
    private bool isTimerRunning = false;
    PhotonView photonView;
    public bool[] Ready;
    public static bool[] Death;

    void Start()
    {
        if (multi)
        {
            photonView = GetComponent<PhotonView>();
            Ready = new bool[PhotonNetwork.CurrentRoom.PlayerCount];
            Death = new bool[PhotonNetwork.CurrentRoom.PlayerCount];
            StartGame.SetActive(false);
            timerText.gameObject.SetActive(true);
            ReadyButton.gameObject.SetActive(true);
        }
    }

    public void ReadyOn()
    {
        ReadyButton.SetActive(false);
        photonView.RPC("ReadyMulti", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
    }

    [PunRPC]
    void ReadyMulti(int playerNumber)
    {
        Ready[playerNumber - 1] = true;
    }

    void Update()
{
    if (multi && !isTimerRunning)
    {
        StartCoroutine(CountdownTimer());
        isTimerRunning = true;
    }

    if(multi)
    {
        if(Death[PhotonNetwork.LocalPlayer.ActorNumber - 1])
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene(1);
            
        }
        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene(2);
        }
        if(IsReady() && timeRemaining > 5f)
        {
            StopAllCoroutines(); // Zatrzymaj obecny timer
            timeRemaining = 5f;
            UpdateTimerDisplay(timeRemaining);
            StartCoroutine(CountdownTimer()); // Uruchom nowy timer z 5 sekundami
        }
        // if(IsLastAlivePlayer())
        // {
        //     photonView.RPC("EndGame", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
        // }
    }
}

IEnumerator CountdownTimer()
{
    float startTime = Time.unscaledTime;
    float endTime = startTime + timeRemaining;
    
    while (Time.unscaledTime < endTime && multi)
    {
        timeRemaining = endTime - Time.unscaledTime;
        UpdateTimerDisplay(timeRemaining);
        yield return null;
    }

    if (timeRemaining <= 1)
    {
        TimerEnded();
    }
}







    // Sprawdza czy tylko ten gracz jest żywy
    // public bool IsLastAlivePlayer()
    // {
    //     if (Death == null || Death.Length == 0)
    //         return false;

    //     // Pobierz ActorNumber aktualnego gracza
    //     int currentPlayerIndex = photonView.Owner.ActorNumber - 1;

    //     // Sprawdź czy indeks jest w zakresie
    //     if (currentPlayerIndex < 0 || currentPlayerIndex >= Death.Length)
    //         return false;

    //     int aliveCount = 0;
    //     bool isCurrentPlayerAlive = false;

    //     for (int i = 0; i < Death.Length; i++)
    //     {
    //         if (!Death[i]) // Jeśli gracz żyje
    //         {
    //             aliveCount++;
    //             if (i == currentPlayerIndex)
    //             {
    //                 isCurrentPlayerAlive = true;
    //             }
    //         }
    //     }

    //     // Zwróć true tylko jeśli jest dokładnie 1 żywy gracz i to jest aktualny gracz
    //     return aliveCount == 1 && isCurrentPlayerAlive;
    // }

    public bool IsReady()
    {
        for (int i = 0; i < Ready.Length; i++)
        {
            // Jeśli gracz jest żywy (Death[i] == false) i nie jest gotowy
            if (!Death[i] && !Ready[i])
            {
                return false;
            }
            // Martwi gracze (Death[i] == true) są automatycznie uznawani za gotowych
        }
        
        return true;
    }


    void UpdateTimerDisplay(float currentTime)
    {
        // Formatujemy czas jako MM:SS
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    [PunRPC]
    public void UstawEnemy(Dictionary<int, int> playerPairs)
    {
        SaveManager.Save(PlayerManager.Name, PlayerManager.PlayerFaceId, ShopManager.levelUp);
        string jsonContent = "";
        string filePath = Application.dataPath + "/Save/Zapis3.json";
        
        if (File.Exists(filePath))
        {
            jsonContent = File.ReadAllText(filePath);
           // Debug.Log("Zawartość pliku zapis3.json:\n" + jsonContent);
        }
        else
        {
            Debug.LogError("Plik zapis3.json nie istnieje w ścieżce: " + filePath);
        }
        photonView.RPC("WlaczFight", RpcTarget.All, playerPairs[PhotonNetwork.LocalPlayer.ActorNumber], jsonContent, PhotonNetwork.LocalPlayer.ActorNumber);
    }

    [PunRPC]
    public void WlaczFight(int id, string comp, int powrot)
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == id)
        {
            EventSystem.eventSystem.GetComponent<EnemyManager>().Comps[StatsManager.Round] = comp;
            EventSystem.eventSystem.GetComponent<SaveManager>().LoadActive(true);

            photonView.RPC("WlaczFightStart", RpcTarget.All, powrot);

            
        }
    }

    [PunRPC]
    public void WlaczFightStart(int id)
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == id)
        {
            ResetTimer();
            for (int i = 0; i < Ready.Length; i++)
            {
                Ready[i] = false;
            }
            ReadyButton.SetActive(true);
            EventSystem.eventSystem.GetComponent<FightManager>().ActiveBattle();
        }
    }

    void TimerEnded()
    {
        timerText.text = "00:00";
        EventSystem.eventSystem.GetComponent<FightManager>().ActiveBattle();
        var pairs = CreatePlayerPairs();
        if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            photonView.RPC("UstawEnemy", RpcTarget.All, pairs);
        }

    }

   public Dictionary<int, int> CreatePlayerPairs()
{
    Dictionary<int, int> playerPairs = new Dictionary<int, int>();
    
    // Pobierz aktywnych (żywych) graczy
    List<int> activePlayers = new List<int>();
    foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
    {
        int playerIndex = player.ActorNumber - 1;
        if (playerIndex >= 0 && playerIndex < Death.Length && !Death[playerIndex])
        {
            activePlayers.Add(player.ActorNumber);
        }
    }

    int activePlayerCount = activePlayers.Count;

    if (activePlayerCount == 0)
    {
        Debug.LogWarning("Brak aktywnych graczy.");
        return playerPairs;
    }

    // Jeśli tylko 1 aktywny gracz - para z samym sobą
    if (activePlayerCount == 1)
    {
        playerPairs[activePlayers[0]] = activePlayers[0];
        Debug.Log($"Tylko jeden aktywny gracz: {activePlayers[0]} vs sam siebie");
        return playerPairs;
    }

    // Potasuj listę aktywnych graczy
    ShuffleList(activePlayers);

    // Sparuj graczy po kolei
    for (int i = 0; i < activePlayerCount - 1; i += 2)
    {
        int player1 = activePlayers[i];
        int player2 = activePlayers[i + 1];
        
        playerPairs[player1] = player2;
        playerPairs[player2] = player1;
        
        Debug.Log($"Para: {player1} vs {player2}");
    }

    // Jeśli została nieparzysta liczba graczy, sparuj ostatniego z przedostatnim
    if (activePlayerCount % 2 != 0)
    {
        int lastPlayer = activePlayers[activePlayerCount - 1];
        int opponent = activePlayers[activePlayerCount - 2];
        
        // Dodaj nową parę
        playerPairs[lastPlayer] = opponent;
        // Nadpisz istniejącą parę dla przeciwnika
        playerPairs[opponent] = lastPlayer;
        
        Debug.Log($"Dodatkowa para (nieparzysta liczba): {lastPlayer} vs {opponent}");
    }

    return playerPairs;
}

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    // Dodatkowe metody do kontroli timera
    public void ResetTimer()
    {
        timeRemaining = 50f + (float)((StatsManager.Round + 1) * 5);
        isTimerRunning = false;
        UpdateTimerDisplay(timeRemaining);
    }

    public void StopTimer()
    {
        isTimerRunning = false;
        StopAllCoroutines();
    }
}