using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class FightManager : MonoBehaviour
{
    #region LoginRequest
    private static readonly string apiURL = "https://api.m455yn.dev/rofw/save-stats";

    [Serializable]
    public class SaveStatsRequest
    {
        public int PlayerId;
        public int Round;
        public bool IsWin;
        public string Comp;
        public int Win;
        public int Lose;
        public string Fraction;
    }
    #endregion

    public Transform targetPosition;
    public Camera mainCamera;

    public List<Unit> units;
    public List<Linia> linie;

    public GameObject shop;
    public GameObject fightUI;
    public static List<Vector2> Tomb = new List<Vector2>();

    public GameObject[] EndScreen;
    public static int Turn;
    public static float GameSpeed = 1f;
    public static bool IsFight;
    public TextMeshProUGUI GameSpeedText;

    public static bool isPaused;

    public TextMeshProUGUI waitingText;

    public EventObject fatyga;
    public GameObject ShildObject;
    //public Music music;
    private bool isBattleRunning = false;

    public void Start()
    {
        StartCoroutine(Sztart());
    }
    public IEnumerator Sztart()
    {
        yield return new WaitForSeconds(1f);
        SaveManager.Save(PlayerManager.Name, PlayerManager.PlayerFaceId, ShopManager.levelUp);

        EventSystem.eventSystem.GetComponent<SaveManager>().LoadActive(true);
    }


    public void ChangeSpeed(int speed)
    {
        switch (speed)
        {
            case 0:
                isPaused = !isPaused;
                if (isPaused)
                {
                    Time.timeScale = 0; // Zatrzymaj czas
                }
                else
                {
                    Time.timeScale = GameSpeed; // Wznów czas
                }
                break;
            case 1:
                if (GameSpeed < 20)
                    GameSpeed *= 2f;
                Time.timeScale = GameSpeed;
                break;
            case 2:
                if (GameSpeed > 0.3f)
                    GameSpeed /= 2f;
                Time.timeScale = GameSpeed;
                break;
        }
        GameSpeedText.text = Time.timeScale == 0 ? "0" : "x" + GameSpeed;
    }

    public void Test()
    {
        StatsManager.Round--;
        ActiveBattle();
    }


    public class AutoBattlerGameViewModel
    {
        public int Id { get; set; }
        public string Version { get; set; }
        public string Name { get; set; }
        public int FaceId { get; set; }
        public DateTime Date { get; set; }
        public int LP { get; set; }
        public int PlayerId { get; set; }
        public string Comp { get; set; }
        public int Round { get; set; }
    }


    public static List<AutoBattlerGameViewModel> LoadFeaturedGamesFromDatabase()
    {
        List<AutoBattlerGameViewModel> gamesList = new List<AutoBattlerGameViewModel>();

        SqlCommand cmd = new SqlCommand("SELECT * FROM AutoBattlerGame", DB.con);

        DataSet ds = DB.selectSQL(cmd);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            AutoBattlerGameViewModel game = new AutoBattlerGameViewModel
            {
                Id = Convert.ToInt32(dr["Id"]),
                Version = dr["Version"].ToString(),
                Name = dr["Name"].ToString(),
                FaceId = Convert.ToInt32(dr["FaceId"]),
                Date = Convert.ToDateTime(dr["Date"]),
                LP = Convert.ToInt32(dr["LP"]),
                PlayerId = Convert.ToInt32(dr["PlayerId"]),
                Comp = dr["Comp"].ToString(),
                Round = Convert.ToInt32(dr["Round"]),
            };
            gamesList.Add(game);
        }
        return gamesList;
    }


    public void ActiveBattle()
    {
        // Jeśli bitwa już trwa lub jesteśmy w trakcie walki, wyjdź
        if (isBattleRunning || IsFight)
            return;

        isBattleRunning = true;

        try
        {
            waitingText.text = "Poszukiwanie oponenta ...";
            Tomb = new List<Vector2>();
            StartCoroutine(ActiveBattle2());
        }
        finally
        {
            // Nie resetujemy tutaj isBattleRunning - powinno to zrobić się po zakończeniu ActiveBattle2
        }
    }

    public IEnumerator ActiveBattle2()
    {
        yield return new WaitForSeconds(0.03f);
        if (!Multi.multi)
        {
            SaveManager.Save(PlayerManager.Name, PlayerManager.PlayerFaceId, ShopManager.levelUp);

            EventSystem.eventSystem.GetComponent<SaveManager>().LoadActive(true);
        }

        waitingText.text = "";
        StartCoroutine(StartBattle());
        yield return null;
    }

    public Pole GetPole(int lineId, int poleId)
    {
        Linia linia = linie.Find(l => l.nr == lineId);
        return linia?.pola.Find(p => p.nr == poleId);
    }

    public IEnumerator ShowEndScreen(int nr)
    {
        if (nr == 3)
        {
            yield return new WaitForSeconds(2);
        }
        EndScreen[nr].SetActive(true);
        yield return new WaitForSeconds(2);
        EndScreen[nr].SetActive(false);
        IsFight = false;
    }


    public IEnumerator StartBattle()
    {
        if (IsFight == false)
        {
            //music.SetMusic(1);
            EventSystem.eventSystem.GetComponent<ShopManager>().FreeRoll = 0;
            shop.SetActive(false);
            fightUI.SetActive(true);

            if (!ShopManager.isLoock)
            {
                EventSystem.eventSystem.GetComponent<ShopManager>().FirstRoll();
            }
            else
            {
                GetComponent<ShopManager>().ChangeLoock();
            }

            yield return StartCoroutine(MoveAndZoomCamera(targetPosition.position, mainCamera.orthographicSize + 1f));
            setList();
            SortUnits();
            yield return (StartCoroutine(Battle()));
            Debug.Log("Zgery essa");
            foreach (var kvp in EventSystem.eventSystem.GetComponent<SynergyManager>().ActiveSynergyObjects)
            {
                GameObject gameObject = kvp.Value; // Get the GameObject from the dictionary
                Synergy synergy = gameObject.GetComponent<Synergy>(); // Get the Synergy component
                synergy.AfterBattle(); // Start the coroutine
            }
            for (int i = 0; i < 3; i++)
            {
                foreach (Pole pole in linie[i].pola)
                {
                    if (pole.unit != null)
                    {
                        try
                        {
                            pole.unit.GetComponent<Unit>().AfterBattle();
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
            }

            //music.SetMusic(0);
            SaveManager.Save(PlayerManager.Name, PlayerManager.PlayerFaceId, ShopManager.levelUp);
        }
        isBattleRunning = false;
    }

    public void Poddymka()
    {
        RankedManager.Poddymka = true;
        IsFight = false;
    }

    bool[] FirstBuffLine = new bool[3];

    public IEnumerator Battle()
    {

        for (int i = 0; i < FirstBuffLine.Length; i++)
        {
            FirstBuffLine[i] = false;
        }
        IsFight = true;
        EventSystem.eventSystem.GetComponent<SynergyManager>().UpdateEnemy();
        foreach (var kvp in EventSystem.eventSystem.GetComponent<SynergyManager>().ActiveSynergyObjects)
        {
            GameObject gameObject = kvp.Value; // Get the GameObject from the dictionary
            Synergy synergy = gameObject.GetComponent<Synergy>(); // Get the Synergy component
            yield return synergy.StartCoroutine(synergy.BeforBattle()); // Start the coroutine
        }
        foreach (var kvp in EventSystem.eventSystem.GetComponent<SynergyManager>().ActiveSynergyObjectsEnemy)
        {
            GameObject gameObject = kvp.Value; // Get the GameObject from the dictionary
            Synergy synergy = gameObject.GetComponent<Synergy>(); // Get the Synergy component
            yield return synergy.StartCoroutine(synergy.BeforBattle()); // Start the coroutine
        }
        // if(units.Count == 0)
        // {
        //     AddToBase();
        //     if(!RankedManager.Ranked)
        //         StatsManager.win++;
        //     StartCoroutine(ShowEndScreen(2));
        //     yield break;
        // }
        foreach (var unit in units)
        {

            if (unit != null && !unit.Skip)
            {
                unit.gameObject.transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
                yield return unit.StartCoroutine(unit.OnBattleStart());
                unit.gameObject.transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
            }
        }
        Turn = 0;
        //     while (!czyWygrana(new int[] { 2, 5 }) || 
        //    !czyWygrana(new int[] { 0, 3 }) || 
        //    !czyWygrana(new int[] { 1, 4 }))
        while (!linie[0].EndBattle || !linie[1].EndBattle || !linie[2].EndBattle)
        {
            if (units.Count == 0)
            {
                break;
            }
            SortUnits();
            Turn++;
            if (Turn > 10)
                EventSystem.eventSystem.GetComponent<EventManager>().SetEvent(fatyga.sprite, fatyga.description + (Turn - 10) + " obrażeń!");
            foreach (var unit in units)
            {
                StartCoroutine(czyWygrana(new int[] { 2, 5 }));
                StartCoroutine(czyWygrana(new int[] { 0, 3 }));
                StartCoroutine(czyWygrana(new int[] { 1, 4 }));
                yield return new WaitForSeconds(0.1f);
                if (unit != null && !unit.Skip && unit.gameObject != null)
                {
                    unit.gameObject.transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
                    if (unit.ReadyToJump)
                        yield return unit.StartCoroutine(unit.Jump());
                    yield return unit.StartCoroutine(unit.PreAction());
                    yield return unit.StartCoroutine(unit.Move());
                    yield return unit.StartCoroutine(unit.Action());
                    yield return unit.StartCoroutine(unit.Fight());
                    unit.gameObject.transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
                    if (unit != null && Turn > 10)
                        yield return StartCoroutine(unit.TakeDamage(unit, Turn - 10, TypeDamage.typeDamage.TrueDamage));
                    if (unit.Health <= 0)
                    {
                        StartCoroutine(unit.Death());
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        StatsManager.Round++;
        setList();
        foreach (var unit in units)
            if (unit != null)
            {
                unit.gameObject.transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
                yield return unit.StartCoroutine(unit.OnBattleEnd());
                unit.gameObject.transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
            }
        int[][] paryLinii = new int[][]
        {
            new int[] { 0, 3 },
            new int[] { 1, 4 },
            new int[] { 2, 5 }
        };
        sprawdzKtoWygralWParach(paryLinii);
        EndBattle();
        EventSystem.eventSystem.GetComponent<SaveManager>().LoadActive();
        yield return null;

    }

    void AddToBase()    //TEST
    {
        int rng = UnityEngine.Random.Range(0, 10);
        if (Login.loggedP && Tutorial.tutorial == false && rng <= StatsManager.Round && !Multi.multi && !PlayerManager.SI)
        {
            rng = UnityEngine.Random.Range(0, 2);
            if (rng == 0 || StatsManager.Round > 12)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(@"
                INSERT INTO AutoBattlerGame (Version, FaceId, Date, LP, PlayerId, Comp, Round)  
                VALUES (@Version, @PlayerFaceId, @DataNow, @LP, @Id, @Team, @Round);
                ", DB.con);

                    cmd.Parameters.AddWithValue("Version", PlayerManager.Version);
                    //cmd.Parameters.AddWithValue("Name", PlayerManager.Name);
                    cmd.Parameters.AddWithValue("PlayerFaceId", PlayerManager.PlayerFaceId);
                    cmd.Parameters.AddWithValue("DataNow", DateTime.Now);
                    cmd.Parameters.AddWithValue("Round", StatsManager.Round - 1);
                    if (RankedManager.Ranked)
                        cmd.Parameters.AddWithValue("LP", PlayerManager.LP);
                    else
                        cmd.Parameters.AddWithValue("LP", 0);
                    cmd.Parameters.AddWithValue("Id", PlayerManager.Id);

                    string filePath = Application.dataPath + "/Save/Zapis.json";
                    if (RankedManager.Ranked)
                    {
                        filePath = Application.dataPath + "/Save/ZapisR.json";
                    }
                    string jsonContent = File.ReadAllText(filePath);

                    cmd.Parameters.AddWithValue("Team", jsonContent);

                    DB.execSQL(cmd);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }
        }
    }

    void CheckAchivment()
    {
        if (StatsManager.win == 12)
        {
            if (Fraction.fractionList.Count == 3)
            {
                Bestiariusz.AddAchivments(5);
            }
            if (Fraction.fractionList.Count == 4)
            {
                Bestiariusz.AddAchivments(6);
            }
            if (Fraction.fractionList.Count == 5)
            {
                Bestiariusz.AddAchivments(7);
            }
            if (Fraction.fractionList.Count == 1)
            {
                if (Fraction.fractionList.Contains(Fraction.fractionType.Ludzie))
                    Bestiariusz.AddAchivments(0);
                if (Fraction.fractionList.Contains(Fraction.fractionType.Nekro))
                    Bestiariusz.AddAchivments(1);
                if (Fraction.fractionList.Contains(Fraction.fractionType.Nomadzi))
                    Bestiariusz.AddAchivments(2);
                if (Fraction.fractionList.Contains(Fraction.fractionType.Krasnoludy))
                    Bestiariusz.AddAchivments(3);
                if (Fraction.fractionList.Contains(Fraction.fractionType.Elfy))
                    Bestiariusz.AddAchivments(4);
            }
        }
    }

    void sprawdzKtoWygralWParach(int[][] paryLinii)
    {
        int graczWin = 0;
        int enemyWin = 0;
        for (int i = 0; i < 3; i++)
        {
            if (linie[i].KtoWygral == 1)
                graczWin++;
            if (linie[i].KtoWygral == 2)
                enemyWin++;
        }

        if (graczWin > enemyWin)
        {
            //if(StatsManager.win != 10)
            StartCoroutine(ShowEndScreen(0));
            AddToBase();
            //Debug.Log("WYGRANKO");
            StatsManager.win++;
            CheckAchivment();
            if (PlayerManager.SI && StatsManager.win == 12)
            {
                SceneManager.LoadScene(2);
            }
            if (((StatsManager.win == 10 && !RankedManager.Poddymka) || StatsManager.win == 12) && RankedManager.Ranked && !Multi.multi && !PlayerManager.SI)
            {
                StartCoroutine(ZapiszwWynil(true));
                if (!RankedManager.Ranked)
                {
                    SceneManager.LoadScene(2);
                    string savePath2 = Application.dataPath + "/Save/Save2.json";
                    if (File.Exists(savePath2))
                    {
                        File.Delete(savePath2);
                        Debug.Log("Plik Save2.json został usunięty.");
                    }
                    else
                    {
                        Debug.LogWarning("Plik Save2.json nie istnieje.");
                    }
                }
                else
                    EventSystem.eventSystem.GetComponent<RankedManager>().StartRank();
            }
        }
        else if (graczWin < enemyWin)
        {
            if (StatsManager.life != 0)
                StartCoroutine(ShowEndScreen(1));
            StatsManager.life--;
            if (Multi.multi)
            {
                GetComponent<PhotonView>().RPC("ZmniejszZdrowie", RpcTarget.All, MultiHP.ID);
            }
            if (!RankedManager.Ranked)
                StatsManager.win++;
            if (StatsManager.life <= 0)
            {
                if (Multi.multi)
                {
                    GetComponent<PhotonView>().RPC("NotReady", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber - 1);
                }
                else
                {
                    StartCoroutine(ZapiszwWynil(false));
                    if (!RankedManager.Ranked)
                    {
                        SceneManager.LoadScene(1);
                        string savePath2 = Application.dataPath + "/Save/Save2.json";
                        if (File.Exists(savePath2))
                        {
                            File.Delete(savePath2);
                            Debug.Log("Plik Save2.json został usunięty.");
                        }
                        else
                        {
                            Debug.LogWarning("Plik Save2.json nie istnieje.");
                        }
                    }
                    else
                        EventSystem.eventSystem.GetComponent<RankedManager>().StartRank();
                }

            }

            AddToBase();
            // Debug.Log("PRZEGRANA");
        }
        else
        {
            AddToBase();
            if (!RankedManager.Ranked)
                StatsManager.win++;
            StartCoroutine(ShowEndScreen(2));
            //Debug.Log("REMIS");
        }
    }
    [PunRPC]
    void NotReady(int id)
    {
        Multi.Death[id] = true;
    }

    IEnumerator ZapiszwWynil(bool isWinning)
    {
        string frakcja = "";

        foreach (Fraction.fractionType type in Enum.GetValues(typeof(Fraction.fractionType)))
        {
            frakcja += Fraction.fractionList.Contains(type) ? "1" : "0";
        }

        string filePath = Application.dataPath + "/Save/Zapis.json";
        if (RankedManager.Ranked)
        {
            filePath = Application.dataPath + "/Save/ZapisR.json";
        }

        string jsonContent = File.ReadAllText(filePath);

        SaveStatsRequest request = new SaveStatsRequest
        {
            PlayerId = PlayerManager.Id,
            Round = StatsManager.Round,
            IsWin = isWinning,
            Comp = jsonContent,
            Win = StatsManager.win,
            Lose = StatsManager.life,
            Fraction = frakcja
        };

        string json = JsonUtility.ToJson(request);
        using (UnityWebRequest www = new UnityWebRequest(apiURL, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Błąd wysyłania: " + www.error);
            }
            else
            {
                Debug.Log("Statystyki zapisane: " + www.downloadHandler.text);
            }
        }
    }

    IEnumerator czyWygrana(int[] indeksyLinii)
    {
        int wygrywa = 0;
        foreach (int indeks in indeksyLinii)
        {
            foreach (Pole pole in linie[indeks].pola)
            {
                if (pole.unit != null && pole.unit.GetComponent<Heros>())
                {
                    if (wygrywa == 0)
                    {
                        wygrywa = pole.unit.GetComponent<Unit>().Enemy ? 2 : 1;
                        linie[indeksyLinii[0]].KtoWygral = wygrywa;
                    }
                    else
                    {
                        bool jestEnemy = pole.unit.GetComponent<Unit>().Enemy;
                        if ((jestEnemy && wygrywa == 1) || (!jestEnemy && wygrywa == 2))
                            yield break;
                    }
                }
            }
        }
        if (linie[indeksyLinii[0]].EndBattle == false)
        {
            switch (wygrywa)
            {
                case 0:
                    foreach (int indeks in indeksyLinii)
                    {
                        linie[indeks].gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                        linie[indeks].EndBattle = true;
                    }
                    break;
                case 1:
                    foreach (int indeks in indeksyLinii)
                    {
                        linie[indeks].gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                        linie[indeks].EndBattle = true;
                        if (FirstBuffLine[indeksyLinii[0]] == false)
                        {
                            FirstBuffLine[indeksyLinii[0]] = true;
                            foreach (Linia line in linie)
                            {
                                if (!indeksyLinii.Contains(line.nr))
                                {
                                    foreach (Pole pole in line.pola)
                                    {
                                        if (pole.unit != null && !pole.unit.GetComponent<Unit>().Enemy)
                                        {
                                            pole.unit.GetComponent<Unit>().Morale();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case 2:
                    foreach (int indeks in indeksyLinii)
                    {
                        linie[indeks].gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                        linie[indeks].EndBattle = true;
                        if (FirstBuffLine[indeksyLinii[0]] == false)
                        {
                            FirstBuffLine[indeksyLinii[0]] = true;
                            foreach (Linia line in linie)
                            {
                                if (!indeksyLinii.Contains(line.nr))
                                {
                                    foreach (Pole pole in line.pola)
                                    {
                                        if (pole.unit != null && pole.unit.GetComponent<Unit>().Enemy)
                                        {
                                            pole.unit.GetComponent<Unit>().Morale();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
        }
        foreach (int indeks in indeksyLinii)
        {
            foreach (Pole pole in linie[indeks].pola)
            {
                if (pole.unit != null)
                {
                    if (pole.unit.GetComponent<Unit>().CanJump != true)
                        pole.unit.GetComponent<Unit>().Skip = true;
                    else
                        pole.unit.GetComponent<Unit>().ReadyToJump = true;
                }

                //units.Remove(pole.unit.GetComponent<Unit>());
            }
        }
        yield return null;
    }

    public void EndBattle()
    {
        GameSpeed = 1f;
        GameSpeedText.text = "x" + GameSpeed;
        Time.timeScale = GameSpeed;
        MoneyManager.ActiveIncom();
        shop.SetActive(true);
        fightUI.SetActive(false);
        StartCoroutine(MoveAndZoomCamera(new Vector3(0, 0, -20f), mainCamera.orthographicSize - 1f));
        EventSystem.eventSystem.GetComponent<SynergyManager>().ClearEnemySynergies();
        
        if((StatsManager.Round == 2 || StatsManager.Round == 7) && StatsManager.life != 3 && !Multi.multi)
        {
            StartCoroutine(ShowEndScreen(3));
            StatsManager.life++;
        }
        if (StatsManager.Round == 2 && StatsManager.life != MultiOptions.Hearth && Multi.multi)
        {
            StartCoroutine(ShowEndScreen(3));
            StatsManager.life++;
            if (Multi.multi)
            {
                GetComponent<PhotonView>().RPC("UpZdrowie", RpcTarget.All, MultiHP.ID);
            }
        }
    }

    private IEnumerator MoveAndZoomCamera(Vector3 endPosition, float endSize)
    {
        Vector3 startPosition = mainCamera.transform.position;
        float startSize = mainCamera.orthographicSize;
        float duration = 1f; // Czas przesunięcia i zmiany rozmiaru kamery (w sekundach)
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Interpolacja pozycji kamery i jej rozmiaru
            mainCamera.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            mainCamera.orthographicSize = Mathf.Lerp(startSize, endSize, t);

            yield return null; // Poczekaj do następnej klatki
        }
        foreach (Linia linia in linie)
        {
            linia.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            linia.EndBattle = false;
        }
        // Ustawienie końcowych wartości
        mainCamera.transform.position = endPosition;
        mainCamera.orthographicSize = endSize;
    }

    public void SortUnits()
    {
        units = units
            .OrderBy(unit => unit.Initiative)       // Sortowanie po Initiative rosnąco
            .ThenBy(unit => unit.Cost)              // W przypadku równej Initiative, sortowanie po Cost
            .ThenBy(unit => unit.Health)            // W przypadku równej Initiative i Cost, sortowanie po Health
           // .ThenBy(unit => unit.Name)              // Na końcu sortowanie alfabetyczne po Name
            .ThenBy(unit => unit.CPU)
            .ToList();
        units.Reverse();
    }

    public void setList()
    {
        units = new List<Unit>();
        foreach (Linia linia in linie)
        {
            foreach (Pole pole in linia.pola)
            {
                if (pole.unit != null)
                {
                    units.Add(pole.unit.GetComponent<Unit>());
                }
            }
        }

    }
}
