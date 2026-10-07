using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Linia : MonoBehaviour
{
    public List<Pole> pola; // Ta lista teraz zaktualizuje się sama z prefaba!
    public bool enemyLine;
    public int nr;
    public bool EndBattle;
    public int KtoWygral;

    public Linia LineNext; // Zostaje bezpieczne i nietknięte!
    
    [Header("Prefaby dróg (Kolejno: Lvl1, Lvl2, Lvl3, Lvl4)")]
    public GameObject[] roadPrefabs;

    [Header("Osobne ulepszanie tej linii")]
    // Aktualny poziom TEJ linii: 1 = poziom startowy (roadPrefabs[0], zrobiony w Start()).
    // Po ulepszeniu do poziomu 2 -> roadPrefabs[1] jest aktywny, itd.
    public int poziom = 1;
    // Koszt kolejnych ulepszeń: kosztyUlepszen[0] = koszt ulepszenia z poziomu 1 na 2,
    // kosztyUlepszen[1] = koszt z 2 na 3, kosztyUlepszen[2] = koszt z 3 na 4.
    public int[] kosztyUlepszen = new int[] { 2, 3, 3 };
    // Promień (w jednostkach świata), w którym klik liczy się jako trafienie w przycisk ulepszenia.
    public float promienKlikniecia = 0.5f;

    private GameObject currentRoadInstance;
    private bool isInitialized = false;
    private Camera mainCamera;

    // Przycisk ulepszenia jako zwykły obiekt świata (bez Canvas/UI) — tak jak reszta gry (patrz Pole.cs).
    private SpriteRenderer aktualnyPrzyciskSprite;
    private TextMeshPro aktualnyTekstKosztu;
    private Color kolorAktywny = Color.white;
    private Color kolorNieaktywny = new Color(1f, 1f, 1f, 0.35f);

    // Kategoria pola — decyduje o tym, jakiego typu jednostka może na nim stać.
    // Ustalana na podstawie flag onlyHeros / onlyBuilding na komponencie Pole.
    private enum PoleType { Jednostka, Budynek, Ogolne }

    private PoleType GetPoleType(Pole p)
    {
        if (p.onlyBuilding) return PoleType.Budynek;
        if (p.onlyHeros) return PoleType.Jednostka;
        return PoleType.Ogolne;
    }

    public static bool Pasuje(GameObject unit, Pole pole)
    {
        bool hero = unit.GetComponent<Heros>() != null || unit.GetComponent<Spell>() != null;
        bool building = unit.GetComponent<Building>() != null;
        if (pole.onlyHeros && !hero) return false;
        if (pole.onlyBuilding && !building) return false;
        return true;
    }

    private void PrzypiszJednostkeDoPola(GameObject unit, Pole docelowe)
    {
        docelowe.unit = unit;

        DragObject dragObj = unit.GetComponent<DragObject>();
        if (dragObj != null) dragObj.pole = docelowe;

        // Odświeżenie pozycji modelu na scenie
        Vector3 newPosition = docelowe.transform.position;
        newPosition.z -= 2f;
        unit.transform.position = newPosition;
    }

    public void Start()
    {
        // Blokada zabezpieczająca przed wielokrotnym odpaleniem
        if (!isInitialized)
        {
            SpawnRoad(0);
            isInitialized = true;
        }
    }

    void Update()
    {
        OdswiezPrzycisk();

        if (aktualnyPrzyciskSprite != null && !enemyLine && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = GetMouseWorldPosition();
            float dystans = Vector3.Distance(mousePos, aktualnyPrzyciskSprite.transform.position);
            if (dystans <= promienKlikniecia)
            {
                TryUpgrade();
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = mainCamera.WorldToScreenPoint(aktualnyPrzyciskSprite.transform.position).z;
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }

    // Koszt NASTĘPNEGO ulepszenia tej linii, albo -1 jeśli linia jest już na max. poziomie.
    public int KosztNastepnegoUlepszenia()
    {
        int indeks = poziom - 1;
        if (kosztyUlepszen == null || indeks < 0 || indeks >= kosztyUlepszen.Length) return -1;
        return Mathf.Max(0, kosztyUlepszen[indeks] - (enemyLine ? 0 : ShopManager.nizka));
    }

    public bool MoznaUlepszyc()
    {
        if (enemyLine) return false; // linii przeciwnika nie ulepszamy z poziomu UI gracza
        if (roadPrefabs == null || poziom >= roadPrefabs.Length) return false;
        return KosztNastepnegoUlepszenia() >= 0;
    }

    // Wywoływane po kliknięciu w promieniu "promienKlikniecia" od ikonki przycisku (patrz Update())
    public void TryUpgrade()
    {
        if (FightManager.IsFight || FightManager.IsOptions) return;
        if (!MoznaUlepszyc()) return;

        int koszt = KosztNastepnegoUlepszenia();
        if (koszt < 0 || MoneyManager.money < koszt) return;

        MoneyManager.money -= koszt;
        ShopManager.nizka = 0; // zniżka z Mrocznej chaty zużyta
        Upgrade(poziom); // poziom to aktualnie obowiązujący indeks w roadPrefabs
        poziom++;

        if (PlayerManager.Id != 0)
            SaveManager.Save(PlayerManager.Name, PlayerManager.PlayerFaceId, ShopManager.levelUp, BohaterManager.bohaterId);
    }

    // Pełny reset stanu tej linii — niszczy aktualną drogę (i jednostki na niej stojące)
    // i zeruje poziom do 0, żeby najbliższe SetLevel()/Upgrade() na pewno odpaliło SpawnRoad
    // od nowa. Używane przez SaveManager przed wczytaniem zapisu (Clear()).
    public void ResetLinia()
    {
        if (pola != null)
        {
            foreach (Pole p in pola)
            {
                if (p != null && p.unit != null)
                {
                    Destroy(p.unit);
                }
            }
        }

        if (currentRoadInstance != null)
        {
            Destroy(currentRoadInstance);
            currentRoadInstance = null;
        }

        pola = new List<Pole>();
        poziom = 0; // 0 = brak zainicjalizowanej drogi, wymusza pełny respawn w SetLevel()
    }

    // Ustawia linię na konkretny poziom (1..roadPrefabs.Length) bez pobierania opłat —
    // używane wyłącznie przy wczytywaniu zapisu, żeby odtworzyć stan planszy.
    public void SetLevel(int docelowyPoziom)
    {
        int maxPoziom = (roadPrefabs != null && roadPrefabs.Length > 0) ? roadPrefabs.Length : 1;
        docelowyPoziom = Mathf.Clamp(docelowyPoziom, 1, maxPoziom);

        if (poziom >= 1 && docelowyPoziom < poziom)
        {
            Debug.LogWarning($"SetLevel: próba zmniejszenia rzędu {nr} z {poziom} do {docelowyPoziom} – pomijam.");
            return;
        }

        if (poziom < 1)
        {
            // Brak zainicjalizowanej drogi (np. po ResetLinia() przy wczytywaniu zapisu) —
            // najpierw trzeba zrobić poziom bazowy, zanim zacznie się dalsze ulepszanie.
            SpawnRoad(0);
            poziom = 1;
        }

        while (poziom < docelowyPoziom)
        {
            Upgrade(poziom);
            poziom++;
        }
    }

    private void OdswiezPrzycisk()
    {
        int koszt = KosztNastepnegoUlepszenia();
        if (aktualnyPrzyciskSprite != null)
        {
            bool moze = MoznaUlepszyc() && koszt >= 0 && MoneyManager.money >= koszt
                && !FightManager.IsFight && !FightManager.IsOptions;
            aktualnyPrzyciskSprite.color = moze ? kolorAktywny : kolorNieaktywny;
        }
        if (aktualnyTekstKosztu != null)
        {
            aktualnyTekstKosztu.text = koszt >= 0 ? koszt + " $" : "MAX";
        }
    }

    private void Upgrade(int level)
    {
        if (level < roadPrefabs.Length)
        {
            SpawnRoad(level);
        }
    }

    private void SpawnRoad(int levelIndex)
    {
        if (roadPrefabs == null || levelIndex < 0 || levelIndex >= roadPrefabs.Length || roadPrefabs[levelIndex] == null)
        {
            Debug.LogWarning($"SpawnRoad: brak prefaba drogi dla poziomu {levelIndex} na linii {nr}.");
            return;
        }

        // 1. Zapisanie jednostek ze starych pól wraz z ich TYPEM (nie samym indeksem!),
        // żeby dało się je potem dopasować do pól tego samego typu na nowej drodze,
        // nawet jeśli układ pól (jednostka/budynek/ogólne) zmienił się między poziomami.
        List<(PoleType typ, GameObject unit)> starePrzypisania = new List<(PoleType, GameObject)>();
        if (pola != null)
        {
            foreach (Pole stare in pola)
            {
                if (stare != null && stare.unit != null)
                {
                    starePrzypisania.Add((GetPoleType(stare), stare.unit));
                }
            }
        }

        // 2. Usunięcie STAREGO modelu drogi (usuwamy tylko wizualizację, Linia.cs zostaje!)
        if (currentRoadInstance != null)
        {
            Destroy(currentRoadInstance);
        }

        // 3. Stworzenie NOWEGO prefaba drogi jako "Dziecka" tej linii.
        // WAŻNE: instancjonujemy od razu z parentem i zerujemy transform LOKALNIE,
        // zamiast tworzyć w Quaternion.identity w świecie i dopiero potem robić SetParent.
        // Dla linii przeciwnika (enemyLine) dokładamy odbicie względem osi Y (flip X),
        // bo prefab drogi jest zaprojektowany raz, z perspektywy gracza.
        currentRoadInstance = Instantiate(roadPrefabs[levelIndex], this.transform);
        currentRoadInstance.transform.localPosition = Vector3.zero;
        currentRoadInstance.transform.localRotation = Quaternion.identity;
        currentRoadInstance.transform.localScale = enemyLine ? new Vector3(-1f, 1f, 1f) : Vector3.one;

        // 4. Pobranie ręcznie ułożonych pól z nowego skryptu DrogaPrefab
        DrogaPrefab nowaDroga = currentRoadInstance.GetComponent<DrogaPrefab>();
        if (nowaDroga != null)
        {
            // Kopia listy, żeby Linia nie dzieliła referencji z komponentem prefaba
            this.pola = new List<Pole>(nowaDroga.polaNaDrodze);

            // Podpięcie (opcjonalnego) przycisku ulepszenia z nowego prefaba drogi.
            // Bez Canvas/UI — zwykły SpriteRenderer + klik wykrywany po dystansie w Update().
            aktualnyPrzyciskSprite = nowaDroga.przyciskSprite;
            aktualnyTekstKosztu = nowaDroga.tekstKosztu;
            if (aktualnyPrzyciskSprite != null)
            {
                aktualnyPrzyciskSprite.gameObject.SetActive(!enemyLine);
            }
            OdswiezPrzycisk();
        }
        else
        {
            Debug.LogError("Prefab drogi nie ma przypiętego skryptu DrogaPrefab!");
            return;
        }

        // 5. Skonfigurowanie nowych pól (nadanie numerów i referencji)
        for (int i = 0; i < pola.Count; i++)
        {
            if (pola[i] == null)
            {
                Debug.LogWarning($"SpawnRoad: pole o indeksie {i} w prefabie '{roadPrefabs[levelIndex].name}' nie jest podpięte w DrogaPrefab.");
                continue;
            }
            pola[i].nr = i;
            pola[i].line = this;
        }

        // 6. Przygotowanie kolejek wolnych pól na nowej drodze, osobno dla każdego typu.
        // Kolejność w kolejce = kolejność w liście polaNaDrodze, czyli "od przodu do tyłu"
        // danego typu — więc dopasowanie zachowuje pierwsze N jednostek na miejscu,
        // a nowe pola (dodane dla danego typu) po prostu zostają puste.
        Dictionary<PoleType, Queue<Pole>> wolnePolaWgTypu = new Dictionary<PoleType, Queue<Pole>>();
        foreach (PoleType t in System.Enum.GetValues(typeof(PoleType)))
            wolnePolaWgTypu[t] = new Queue<Pole>();

        foreach (Pole nowe in pola)
        {
            if (nowe != null)
                wolnePolaWgTypu[GetPoleType(nowe)].Enqueue(nowe);
        }

        // 7. Przypisanie jednostek do pól TEGO SAMEGO TYPU co miały wcześniej
        List<(PoleType typ, GameObject unit)> nieprzydzielone = new List<(PoleType, GameObject)>();
        foreach (var (typ, unit) in starePrzypisania)
        {
            if (wolnePolaWgTypu[typ].Count > 0)
            {
                PrzypiszJednostkeDoPola(unit, wolnePolaWgTypu[typ].Dequeue());
            }
            else
            {
                nieprzydzielone.Add((typ, unit));
            }
        }

        // 8. Awaryjnie: jeśli dla jakiegoś typu zabrakło pól (np. downgrade albo błąd
        // w układzie prefaba), spróbuj wstawić jednostkę na dowolne wolne, zgodne pole,
        // zamiast bezpowrotnie ją zgubić.
        foreach (var (typ, unit) in nieprzydzielone)
        {
            Pole docelowe = null;
            foreach (PoleType kandydatTyp in new[] { PoleType.Ogolne, PoleType.Jednostka, PoleType.Budynek })
            {
                if (wolnePolaWgTypu[kandydatTyp].Count > 0 && Pasuje(unit, wolnePolaWgTypu[kandydatTyp].Peek()))
                {
                    docelowe = wolnePolaWgTypu[kandydatTyp].Dequeue();
                    break;
                }
            }

            if (docelowe != null)
            {
                PrzypiszJednostkeDoPola(unit, docelowe);
            }
            else
            {
                // Nigdy nie zostawiamy jednostki bez pola: gracz -> ławka, wróg -> usunięty.
                Pole wolna = enemyLine ? null : EventSystem.eventSystem.GetComponent<ShopManager>().lawka.Find(p => p.unit == null);
                if (wolna != null) { wolna.unit = unit; wolna.Start(); }
                else Destroy(unit);
                Debug.LogWarning($"Brak pola dla '{unit.name}' po podmianie drogi na linii {nr}.");
            }
        }
    }
}