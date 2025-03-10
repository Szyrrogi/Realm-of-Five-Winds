using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Data.SqlClient;
using System;
using System.IO;
using System.Data;
using TMPro;


public class FightManager : MonoBehaviour
{
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
                if(GameSpeed < 10)
                    GameSpeed *= 2f;
                Time.timeScale = GameSpeed;
                break;
            case 2:
                if(GameSpeed > 0.3f)
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
        //StatsManager.Round--;
        //////  TEST

        if(IsFight == false)
        {
            waitingText.text = "Poszukiwanie oponenta ...";
            Debug.Log("Poszukiwanie oponenta ...");

            Tomb = new List<Vector2>();
            StartCoroutine(ActiveBattle2());
        }
    }

    public IEnumerator ActiveBattle2()
    {
        yield return new WaitForSeconds(0.03f);
        SaveManager.Save(PlayerManager.Name, PlayerManager.PlayerFaceId, ShopManager.levelUp);
        
        EventSystem.eventSystem.GetComponent<SaveManager>().LoadActive(true);

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
        if(nr == 3)
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
        if(IsFight == false)
        {
            EventSystem.eventSystem.GetComponent<ShopManager>().FreeRoll = 0;
            shop.SetActive(false);
            fightUI.SetActive(true);

            if(!ShopManager.isLoock)
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

            foreach (var kvp in EventSystem.eventSystem.GetComponent<SynergyManager>().ActiveSynergyObjects)
            {
                GameObject gameObject = kvp.Value; // Get the GameObject from the dictionary
                Synergy synergy = gameObject.GetComponent<Synergy>(); // Get the Synergy component
                synergy.AfterBattle(); // Start the coroutine
            }
            for(int i = 0; i < 3; i++)
            {
                foreach(Pole pole in linie[i].pola)
                {
                    if(pole.unit != null)
                    {
                        pole.unit.GetComponent<Unit>().AfterBattle();
                    }
                }
            }
            // if(!ShopManager.isLoock)
            // {
            //     EventSystem.eventSystem.GetComponent<ShopManager>().FirstRoll();
            // }
            // else
            // {
            //     GetComponent<ShopManager>().ChangeLoock();
            // }
            SaveManager.Save(PlayerManager.Name, PlayerManager.PlayerFaceId, ShopManager.levelUp);
        }
        
    }

    public void Poddymka()
    {
        RankedManager.Poddymka = true;
        IsFight = false;
    }

    bool[] FirstBuffLine = new bool[3];

    public IEnumerator Battle()
    {
        for(int i = 0; i < FirstBuffLine.Length; i++)
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

        foreach (var unit in units)
            if(unit != null && !unit.Skip)
            {
                unit.gameObject.transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
                yield return unit.StartCoroutine(unit.OnBattleStart());
                unit.gameObject.transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
            }
        Turn = 0;
    //     while (!czyWygrana(new int[] { 2, 5 }) || 
    //    !czyWygrana(new int[] { 0, 3 }) || 
    //    !czyWygrana(new int[] { 1, 4 }))
        while(!linie[0].EndBattle || !linie[1].EndBattle || !linie[2].EndBattle)
        {
            SortUnits();
            Turn++;
            if(Turn > 10)
                EventSystem.eventSystem.GetComponent<EventManager>().SetEvent(fatyga.sprite, fatyga.description + (Turn - 10 ) + " obrażeń!");
            foreach (var unit in units)
            {
                StartCoroutine(czyWygrana(new int[] { 2, 5 }));
                StartCoroutine(czyWygrana(new int[] { 0, 3 })); 
                StartCoroutine(czyWygrana(new int[] { 1, 4 }));
                yield return new WaitForSeconds(0.1f);
                if(unit != null && !unit.Skip && unit.gameObject != null)
                {
                    unit.gameObject.transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
                    if(unit.ReadyToJump)
                        yield return unit.StartCoroutine(unit.Jump());
                    yield return unit.StartCoroutine(unit.PreAction());
                    yield return unit.StartCoroutine(unit.Move());
                    yield return unit.StartCoroutine(unit.Action());
                    yield return unit.StartCoroutine(unit.Fight());
                    unit.gameObject.transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
                    if (unit != null && Turn > 10)
                        yield return StartCoroutine(unit.TakeDamage(unit, Turn - 10, TypeDamage.typeDamage.TrueDamage));
                    if(unit.Health <= 0)
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
            if(unit != null)
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
        if(Login.zalogowano)
        {
            try{
                SqlCommand cmd = new SqlCommand(@"
                INSERT INTO AutoBattlerGame (Version, Name, FaceId, Date, LP, PlayerId, Comp, Round)  
                VALUES (@Version, @Name, @PlayerFaceId, @DataNow, @LP, @Id, @Team, @Round);
                ", DB.con);

                cmd.Parameters.AddWithValue("Version", PlayerManager.Version);
                cmd.Parameters.AddWithValue("Name", PlayerManager.Name);
                cmd.Parameters.AddWithValue("PlayerFaceId", PlayerManager.PlayerFaceId);
                cmd.Parameters.AddWithValue("DataNow", DateTime.Now);
                cmd.Parameters.AddWithValue("Round", StatsManager.Round - 1);
                if(RankedManager.Ranked)
                    cmd.Parameters.AddWithValue("LP", PlayerManager.LP);
                else
                    cmd.Parameters.AddWithValue("LP", 0);
                cmd.Parameters.AddWithValue("Id", PlayerManager.Id);

                string filePath = Application.dataPath + "/Save/Zapis.json";
                if(RankedManager.Ranked)
                {
                    filePath = Application.dataPath + "/Save/ZapisR.json";
                }
                string jsonContent = File.ReadAllText(filePath);

                cmd.Parameters.AddWithValue("Team", jsonContent);
            
                DB.execSQL(cmd);
            }
            catch(Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
    }

   void sprawdzKtoWygralWParach(int[][] paryLinii)
    {
        int graczWin = 0;
        int enemyWin = 0;
        for(int i = 0; i < 3; i++)
        {
            if(linie[i].KtoWygral == 1)
                graczWin++; 
            if(linie[i].KtoWygral == 2)
                enemyWin++;
        }
        // foreach (int[] para in paryLinii)
        // {
        //     int graczJednostki = 0;
        //     int enemyJednostki = 0;

        //     foreach (int indeks in para)
        //     {
        //         foreach (Pole pole in linie[indeks].pola)
        //         {
        //             if (pole.unit != null && pole.unit.GetComponent<Heros>())
        //             {
        //                 if (pole.unit.GetComponent<Unit>().Enemy)
        //                     enemyJednostki++;
        //                 else
        //                     graczJednostki++;
        //             }
        //         }
        //     }

        //     // Sprawdź wynik dla danej pary linii
        //     if (graczJednostki > 0 && enemyJednostki == 0)
        //     {
        //         graczWin++;
        //     }
        //     else if (enemyJednostki > 0 && graczJednostki == 0)
        //     {
        //         enemyWin++;
        //     }
        // }
        if (graczWin > enemyWin)
        {
            if(StatsManager.win != 10)
                StartCoroutine(ShowEndScreen(0));
            AddToBase();
            //Debug.Log("WYGRANKO");
            StatsManager.win++;
            if((StatsManager.win == 10 && !RankedManager.Poddymka) || StatsManager.win == 12)
            {
                ZapiszwWynil(true);
                if(!RankedManager.Ranked)
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
            if(StatsManager.life != 0)
                StartCoroutine(ShowEndScreen(1));
            StatsManager.life--;
            if(StatsManager.life == 0)
            {
                ZapiszwWynil(false);
                if(!RankedManager.Ranked)
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
            
            AddToBase();
           // Debug.Log("PRZEGRANA");
        }
        else
        {
            AddToBase();
            StartCoroutine(ShowEndScreen(2));
            //Debug.Log("REMIS");
        }
    }

    void ZapiszwWynil(bool isWinning)
    {
        try
        {
            using (SqlConnection con = DB.Connect(DB.conStr))
            {
                string query = "INSERT INTO Stats (PlayerId, Round, IsWin, Comp, Win, Lose, Fraction) VALUES (@PlayerId, @Round, @IsWin, @Comp, @Win, @Lose, @fraction)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@PlayerId", PlayerManager.Id);
                cmd.Parameters.AddWithValue("@Round", StatsManager.Round);
                cmd.Parameters.AddWithValue("@IsWin", isWinning);
                cmd.Parameters.AddWithValue("@Win", StatsManager.win);
                cmd.Parameters.AddWithValue("@Lose", StatsManager.life);

                string frakcja = "";

                // Iteracja przez wszystkie wartości enuma
                foreach (Fraction.fractionType type in Enum.GetValues(typeof(Fraction.fractionType)))
                {
                    if (Fraction.fractionList.Contains(type))
                    {
                        frakcja += "1";
                    }
                    else
                    {
                        frakcja += "0";
                    }
                }
                cmd.Parameters.AddWithValue("@Fraction", frakcja);

                string filePath = Application.dataPath + "/Save/Zapis.json";
                if(RankedManager.Ranked)
                {
                    filePath = Application.dataPath + "/Save/ZapisR.json";
                }
                string jsonContent = File.ReadAllText(filePath);

                cmd.Parameters.AddWithValue("@Comp", jsonContent);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    Debug.Log("Rejestracja zakończona pomyślnie!");
                }
                else
                {
                    Debug.Log("Wystąpił błąd podczas rejestracji.");
                }
            }
        }
        catch (SqlException ex)
        {
            Debug.Log("Błąd bazy danych: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            Debug.Log("Wystąpił nieoczekiwany błąd: " + ex.Message);
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
        if(linie[indeksyLinii[0]].EndBattle == false)
        {
            switch(wygrywa)
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
                    if(FirstBuffLine[indeksyLinii[0]] == false)
                    {
                        FirstBuffLine[indeksyLinii[0]] = true;
                        foreach(Linia line in linie)
                        {
                            if(!indeksyLinii.Contains(line.nr))
                            {
                                foreach(Pole pole in line.pola)
                                {
                                    if(pole.unit != null && !pole.unit.GetComponent<Unit>().Enemy)
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
                    if(FirstBuffLine[indeksyLinii[0]] == false)
                    {
                        FirstBuffLine[indeksyLinii[0]] = true;
                        foreach(Linia line in linie)
                        {
                            if(!indeksyLinii.Contains(line.nr))
                            {
                                foreach(Pole pole in line.pola)
                                {
                                    if(pole.unit != null && pole.unit.GetComponent<Unit>().Enemy)
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
                foreach(Pole pole in linie[indeks].pola)
                {
                    if(pole.unit != null)
                    {
                        if(pole.unit.GetComponent<Unit>().CanJump != true)
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
        
        if(StatsManager.Round == 2 && StatsManager.life != 3)
        {
            StartCoroutine(ShowEndScreen(3));
            StatsManager.life++;
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
        foreach(Linia linia in linie)
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
            .ThenBy(unit => unit.Name)              // Na końcu sortowanie alfabetyczne po Name
            .ThenBy(unit => unit.CPU)
            .ToList();
        units.Reverse();
    }

    public void setList()
    {
        units = new List<Unit>();
        foreach(Linia linia in linie)
        {
            foreach(Pole pole in linia.pola)
            {
                if(pole.unit!= null)
                {
                    units.Add(pole.unit.GetComponent<Unit>());
                }
            }
        }
        
    }
}
