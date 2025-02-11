using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyManager : MonoBehaviour
{
    public List<Synergy> Synergie;
    public GameObject parentObject;
    public Dictionary<int, GameObject> ActiveSynergyObjects = new Dictionary<int, GameObject>();

    public GameObject parentObjectEnemy;
    public Dictionary<int, GameObject> ActiveSynergyObjectsEnemy = new Dictionary<int, GameObject>();

    void Update()
    {
        if(!FightManager.IsFight)
        {
            foreach (Synergy synergy in Synergie)
            {
                int synergyId = synergy.Id; // Zakładam, że klasa Synergy ma pole Id

                if (synergy.CheckIsActive(0))
                {
                    // Sprawdź, czy obiekt już istnieje
                    if (!ActiveSynergyObjects.ContainsKey(synergyId) || ActiveSynergyObjects[synergyId] == null)
                    {
                        // Jeśli nie istnieje, utwórz nowy obiekt
                        GameObject newSynergyObject = Instantiate(synergy.gameObject, new Vector3(0, 0, 0), Quaternion.identity);
                        newSynergyObject.transform.SetParent(parentObject.transform);
                        if (ActiveSynergyObjects.ContainsKey(synergyId))
                        {
                            ActiveSynergyObjects[synergyId] = newSynergyObject; // Aktualizuj istniejący wpis
                        }
                        else
                        {
                            ActiveSynergyObjects.Add(synergyId, newSynergyObject); // Dodaj nowy wpis
                        }
                    }
                }
                else
                {
                    // Jeśli obiekt istnieje i zmienna isActive jest false, usuń obiekt
                    if (ActiveSynergyObjects.ContainsKey(synergyId) && ActiveSynergyObjects[synergyId] != null)
                    {
                        Destroy(ActiveSynergyObjects[synergyId]);
                        ActiveSynergyObjects.Remove(synergyId); // Usuń wpis ze słownika
                    }
                }
            }
        }
    }

    public void UpdateEnemy()
    {
        foreach (Synergy synergy in Synergie)
        {
            int synergyId = synergy.Id; // Zakładam, że klasa Synergy ma pole Id

            if (synergy.CheckIsActive(3))
            {
                // Sprawdź, czy obiekt już istnieje
                if (!ActiveSynergyObjectsEnemy.ContainsKey(synergyId) || ActiveSynergyObjectsEnemy[synergyId] == null)
                {
                    // Jeśli nie istnieje, utwórz nowy obiekt
                    GameObject newSynergyObject = Instantiate(synergy.gameObject, new Vector3(0, 0, 0), Quaternion.identity);
                    newSynergyObject.GetComponent<Synergy>().Enemy = true;
                    newSynergyObject.transform.SetParent(parentObjectEnemy.transform);
                    if (ActiveSynergyObjectsEnemy.ContainsKey(synergyId))
                    {
                        ActiveSynergyObjectsEnemy[synergyId] = newSynergyObject; // Aktualizuj istniejący wpis
                    }
                    else
                    {
                        ActiveSynergyObjectsEnemy.Add(synergyId, newSynergyObject); // Dodaj nowy wpis
                    }
                }
            }
            else
            {
                // Jeśli obiekt istnieje i zmienna isActive jest false, usuń obiekt
                if (ActiveSynergyObjectsEnemy.ContainsKey(synergyId) && ActiveSynergyObjectsEnemy[synergyId] != null)
                {
                    Destroy(ActiveSynergyObjectsEnemy[synergyId]);
                    ActiveSynergyObjectsEnemy.Remove(synergyId); // Usuń wpis ze słownika
                }
            }
        }
    }

    public void ClearEnemySynergies()
    {
        foreach (var synergyPair in ActiveSynergyObjectsEnemy)
        {
            if (synergyPair.Value != null)
            {
                Destroy(synergyPair.Value); // Niszczymy obiekt gry
            }
        }

        ActiveSynergyObjectsEnemy.Clear(); // Czyścimy słownik
    }
}