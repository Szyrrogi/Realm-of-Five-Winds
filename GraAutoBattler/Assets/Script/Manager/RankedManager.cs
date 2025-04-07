using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Data.SqlClient;
using System.Data;
using System;
using System.IO;

public class RankedManager : MonoBehaviour
{
    public int[] LPToGet;
    public static bool Ranked = true;
    public GameObject RankedShow;

    public GameObject WinSymbol; // Prefab z RectTransform (np. Image lub Text)
    public GameObject rankedParent; // Obiekt z RectTransform (np. pusty GameObject z Canvas jako rodzicem)
    public Vector2 rankedPosition; // Początkowa pozycja
    public int win;
    public TextMeshProUGUI NewLp;
    public TextMeshProUGUI LP;
    public TextMeshProUGUI RankNumber;
    public Image RankImage;
    public Sprite[] ranksSprite;

    public GameObject exit;
    public GameObject poddym;
    public GameObject punkty;
    public static bool Poddymka;

    public void StartRank()
    {
        win = StatsManager.win;
        RankedShow.SetActive(true);
        SetRange(PlayerManager.LP);
        StartCoroutine(SpawnObjectsWithDelay());
    }

    IEnumerator SpawnObjectsWithDelay()
    {
        NewLp.text = "";
        RectTransform parentRectTransform = rankedParent.GetComponent<RectTransform>();
        if (parentRectTransform == null)
        {
            Debug.LogError("rankedParent musi mieć komponent RectTransform!");
            yield break;
        }

        if(!Poddymka)
    {
        for (int i = 0; i < win; i++)
        {
            GameObject newObj = Instantiate(WinSymbol);

            newObj.transform.SetParent(rankedParent.transform, false);

            RectTransform rectTransform = newObj.GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                Debug.LogError("WinSymbol musi mieć komponent RectTransform!");
                yield break;
            }

            rectTransform.anchoredPosition = rankedPosition;
            rankedPosition.x += 170;

            // Efekt "pop" - skalowanie obiektu
            StartCoroutine(PopEffect(newObj));

            // Czekaj 0.7 sekundy przed stworzeniem kolejnego obiektu
            yield return new WaitForSeconds(0.4f);
        }
    }

        int sum = sumPoint();

        if (sum < 0)
        {
            NewLp.color = Color.red; // Czerwony kolor dla ujemnych wartości
        }
        else
        {
            NewLp.color = Color.green; // Zielony kolor dla dodatnich wartości
        }

        // Animacja dla dodatnich lub ujemnych wartości
        if (sum >= 0)
        {
            for (int i = 0; i <= sum; i++)
            {
                NewLp.text = "+" + i;
                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {
            for (int i = 0; i >= sum; i--)
            {
                NewLp.text = i.ToString();
                yield return new WaitForSeconds(0.05f);
            }
        }
        if(StatsManager.win != 10 || Poddymka)
        {
            yield return new WaitForSeconds(1f);

            // Zmiana wartości LP w zależności od sum
            if (sum >= 0)
            {
                for (int i = 0; i <= sum; i++)
                {
                    NewLp.text = "+" + (sum - i);
                    PlayerManager.LP++;
                    SetRange(PlayerManager.LP);
                    yield return new WaitForSeconds(0.05f);
                }
            }
            else
            {
                int min = PlayerManager.LP % 300;
                min = PlayerManager.LP - min;
                for (int i = 0; i >= sum; i--)
                {
                    Debug.Log(-sum + ">" + min);
                    if(PlayerManager.LP > min)
                    {
                        NewLp.text = (sum - i).ToString();
                        PlayerManager.LP--;
                        SetRange(PlayerManager.LP);
                        yield return new WaitForSeconds(0.05f);
                    }
                }
            }
            UpdateLPIdInDatabase(PlayerManager.Id, PlayerManager.LP);
            exit.SetActive(true);
            string savePath2 = Application.dataPath + "/Save/Save2R.json";
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
        {
             punkty.SetActive(true);
        }
        if(StatsManager.win == 10 && !Poddymka)
        {
            poddym.SetActive(true);
        }
    }

    public void punktyyy()
    {
        string savePath2 = Application.dataPath + "/Save/Save2R.json";
        if (File.Exists(savePath2))
        {
            File.Delete(savePath2);
            Debug.Log("Plik Save2.json został usunięty.");
        }
        else
        {
            Debug.LogWarning("Plik Save2.json nie istnieje.");
        }
        StartCoroutine(GivePuzniej());
    }

    IEnumerator GivePuzniej()
    {
        int sum = sumPoint();
         if (sum >= 0)
            {
                for (int i = 0; i <= sum; i++)
                {
                    NewLp.text = "+" + (sum - i);
                    PlayerManager.LP++;
                    SetRange(PlayerManager.LP);
                    yield return new WaitForSeconds(0.05f);
                }
            }
            else
            {
                int min = sum % 300;
                min = sum - min;
                for (int i = 0; i >= sum; i--)
                {
                    if(sum > min)
                    {
                        NewLp.text = (sum - i).ToString();
                        PlayerManager.LP--;
                        SetRange(PlayerManager.LP);
                        yield return new WaitForSeconds(0.05f);
                    }
                }
            }
            UpdateLPIdInDatabase(PlayerManager.Id, PlayerManager.LP);
            exit.SetActive(true);
    }

    void SetRange(int sum)
    {
        RankImage.sprite = ranksSprite[sum / 300];
        RankNumber.text = ((sum % 300) / 100 + 1).ToString();
        LP.text = (PlayerManager.LP % 100) + " LP";
    }

    int sumPoint()
    {
        int modyfikator = PlayerManager.LP / 300 + 2;
        if(modyfikator > 4)
        {
            modyfikator = 4;
        }
        float zwrot = 0;
        int puchary = win;
        if(puchary > 10)
        {
            puchary = 10;
        }
        if(puchary >= modyfikator)
            zwrot = LPToGet[puchary - modyfikator];
        else
            zwrot = -(LPToGet[modyfikator - 1 - puchary]);
        if(win >= 10)
            zwrot *= 1.5f;
        if(win == 12)
            zwrot *= 2;
        else
        {
            if(Poddymka)
            {
                zwrot *= 0.5f;
            }
        }
        if(zwrot > 0)
        {
            zwrot += zwrot / 30 * Fraction.fractionList.Count;
        }
        if(PlayerManager.LP > 900 && zwrot > 0)
        {
            zwrot *= 0.9f;
        }
        if(PlayerManager.LP > 900 && zwrot < 0)
        {
            zwrot *= 1.1f;
        }
        if(PlayerManager.LP > 1200 && zwrot > 0)
        {
            zwrot *= 0.9f;
        }
        if(PlayerManager.LP > 1200 && zwrot < 0)
        {
            zwrot *= 1.1f;
        }
        if(PlayerManager.LP > 1500 && zwrot > 0)
        {
            zwrot *= 0.7f;
        }
        if(PlayerManager.LP > 1500 && zwrot < 0)
        {
            zwrot *= 1.3f;
        }
        return (int)zwrot;
    }

    IEnumerator PopEffect(GameObject obj)
    {
        // Początkowa skala obiektu
        obj.transform.localScale = Vector3.zero;

        // Animacja skalowania do normalnej wielkości
        float duration = 0.3f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            obj.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Upewnij się, że obiekt ma dokładnie skalę 1 na końcu animacji
        obj.transform.localScale = Vector3.one;
    }

    private void UpdateLPIdInDatabase(int playerId, int newLPId)
    {
        try
        {
            using (SqlConnection con = DB.Connect(DB.conStr))
            {
                // Zapytanie SQL do aktualizacji FaceId dla gracza o danym Id
                string query = "UPDATE Players SET LP = @NewLPId WHERE Id = @PlayerId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@NewLPId", newLPId);
                cmd.Parameters.AddWithValue("@PlayerId", playerId);
                // Wykonaj zapytanie
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Debug.Log("FaceId został pomyślnie zaktualizowany w bazie danych.");
                }
                else
                {
                    Debug.LogWarning("Nie znaleziono gracza o podanym Id.");
                }
            }
        }
        catch (SqlException ex)
        {
            Debug.LogError("Błąd bazy danych podczas aktualizacji FaceId: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Wystąpił nieoczekiwany błąd: " + ex.Message);
        }
    }
}