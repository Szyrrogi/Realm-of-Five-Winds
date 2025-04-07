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
        photonView.RPC("WlaczFight", RpcTarget.All, playerPairs[PhotonNetwork.LocalPlayer.ActorNumber], PhotonNetwork.LocalPlayer.ActorNumber);
    }

    [PunRPC]
    public void WlaczFight(int id, int powrot)
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == id)
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
            
            photonView.RPC("WlaczFightStart", RpcTarget.All, powrot, jsonContent);

            
        }
    }

    [PunRPC]
    public void WlaczFightStart(int id, string comp)
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == id)
        {
            EventSystem.eventSystem.GetComponent<EnemyManager>().Comps[StatsManager.Round] = comp;
            EventSystem.eventSystem.GetComponent<SaveManager>().LoadActive(true);
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
        SaveManager.Save(PlayerManager.Name, PlayerManager.PlayerFaceId, ShopManager.levelUp);
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

    // Lista jeszcze niesparowanych graczy
    var unpairedPlayers = new List<int>(activePlayers);

    while (unpairedPlayers.Count > 1)
    {
        int player1 = unpairedPlayers[0];
        int player2 = unpairedPlayers[1];
        
        // Dodaj pary w obie strony
        playerPairs[player1] = player2;
        playerPairs[player2] = player1;
        
        Debug.Log($"Para: {player1} vs {player2}");
        
        // Usuń sparowanych graczy z listy
        unpairedPlayers.RemoveAt(0);
        unpairedPlayers.RemoveAt(0);
    }

    // Jeśli został 1 gracz (nieparzysta liczba), sparuj go z losowym już sparowanym graczem
    if (unpairedPlayers.Count == 1)
    {
        int lastPlayer = unpairedPlayers[0];
        var possibleOpponents = playerPairs.Keys.Where(p => p != lastPlayer).ToList();
        
        if (possibleOpponents.Count > 0)
        {
            int opponent = possibleOpponents[Random.Range(0, possibleOpponents.Count)];
            
            // Aktualizujemy pary
            playerPairs[lastPlayer] = opponent;
            //playerPairs[opponent] = lastPlayer;
            
            Debug.Log($"Dodatkowa para (nieparzysta liczba): {lastPlayer} vs {opponent}");
        }
        else
        {
            Debug.Log($"Nie można znaleźć przeciwnika dla ostatniego gracza: {lastPlayer}");
        }
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