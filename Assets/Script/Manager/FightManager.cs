using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class FightManager : MonoBehaviour
{
    public Transform targetPosition; 
    public Camera mainCamera; 

    public List<Unit> units;
    public List<Linia> linie;

    public GameObject shop;
    public static List<Vector2> Tomb = new List<Vector2>();

    public GameObject[] EndScreen;

    public void ActiveBattle()
    {
        Tomb = new List<Vector2>();
        SaveManager.Save("Bejdżej", 7, ShopManager.levelUp);
        EventSystem.eventSystem.GetComponent<SaveManager>().LoadActive(true);
        StartCoroutine(StartBattle());
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
            yield return ShowEndScreen(2);
        }
        EndScreen[nr].SetActive(true);
        yield return new WaitForSeconds(2);
        EndScreen[nr].SetActive(false);
    }


    public IEnumerator StartBattle()
    {
        shop.SetActive(false);

        yield return StartCoroutine(MoveAndZoomCamera(targetPosition.position, mainCamera.orthographicSize + 0.8f));
        setList();
        SortUnits();
        StartCoroutine(Battle());
    }

    public IEnumerator Battle()
    {
        foreach (var unit in units)
            if(unit != null)
            {
                unit.gameObject.transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
                yield return unit.StartCoroutine(unit.OnBattleStart());
                unit.gameObject.transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
            }

        while (!czyWygrana(new int[] { 2, 5 }) || 
       !czyWygrana(new int[] { 0, 3 }) || 
       !czyWygrana(new int[] { 1, 4 }))
        {
            //Debug.Log("WALKA");
            foreach (var unit in units)
            {
                if(unit != null)
                {
                    unit.gameObject.transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
                    yield return unit.StartCoroutine(unit.PreAction());
                    yield return unit.StartCoroutine(unit.Move());
                    yield return unit.StartCoroutine(unit.Action());
                    yield return unit.StartCoroutine(unit.Fight());
                    unit.gameObject.transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);

                }
            }
        }
        StatsManager.Round++;
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

   void sprawdzKtoWygralWParach(int[][] paryLinii)
    {
        int graczWin = 0;
        int enemyWin = 0;
        foreach (int[] para in paryLinii)
        {
            int graczJednostki = 0;
            int enemyJednostki = 0;

            foreach (int indeks in para)
            {
                foreach (Pole pole in linie[indeks].pola)
                {
                    if (pole.unit != null && pole.unit.GetComponent<Heros>())
                    {
                        if (pole.unit.GetComponent<Unit>().Enemy)
                            enemyJednostki++;
                        else
                            graczJednostki++;
                    }
                }
            }

            // Sprawdź wynik dla danej pary linii
            if (graczJednostki > 0 && enemyJednostki == 0)
            {
                graczWin++;
            }
            else if (enemyJednostki > 0 && graczJednostki == 0)
            {
                enemyWin++;
            }
        }
        if (graczWin > enemyWin)
        {
            StartCoroutine(ShowEndScreen(0));
            Debug.Log("WYGRANKO");
            StatsManager.win++;
            if(StatsManager.win == 10)
            {
                SceneManager.LoadScene(2);
            }
        }
        else if (graczWin < enemyWin)
        {
            StartCoroutine(ShowEndScreen(1));
            StatsManager.life--;
            if(StatsManager.life == 0)
            {
                SceneManager.LoadScene(1);
            }
            Debug.Log("PRZEGRANA");
        }
        else
        {
            StartCoroutine(ShowEndScreen(2));
            Debug.Log("REMIS");
        }
    }


    bool czyWygrana(int[] indeksyLinii)
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
                    }
                    else
                    {
                        bool jestEnemy = pole.unit.GetComponent<Unit>().Enemy;
                        if ((jestEnemy && wygrywa == 1) || (!jestEnemy && wygrywa == 2))
                            return false;
                    }
                }
            }
        }
        return true;
    }

    public void EndBattle()
    {
        MoneyManager.ActiveIncom();
        shop.SetActive(true);
        StartCoroutine(MoveAndZoomCamera(new Vector3(0, 0, -20f), mainCamera.orthographicSize - 0.8f));
        if(!ShopManager.isLoock)
        {
            EventSystem.eventSystem.GetComponent<ShopManager>().FirstRoll();
        }
        else
        {
            GetComponent<ShopManager>().ChangeLoock();
        }
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
