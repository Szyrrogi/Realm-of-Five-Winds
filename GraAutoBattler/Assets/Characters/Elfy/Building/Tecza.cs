using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tecza : Building
{
    public override void AfterBattle()
    {
        Health -= 1;
        MaxHealth -= 1;
        if(Health == 0)
        {

            for(int i = 0; i < 2; i++)
            {
            List<GameObject> characters = EventSystem.eventSystem.GetComponent<CharacterManager>().characters;
            


            var matchingObjects = characters.Where(go => 
            {
                bool hasBuilding = go.GetComponent<Building>() != null;
                Heros heroesComponent = go.GetComponent<Heros>();
                bool hasHeroesWithUnitFalse = heroesComponent != null && ((heroesComponent.Evolution == false && i == 0) || (heroesComponent.Evolution == true && i == 1)) && heroesComponent.Star == 4;
                return !hasBuilding && hasHeroesWithUnitFalse;
            }).ToList();

            // Jeśli nie ma pasujących obiektów, zakończ
            if(matchingObjects.Count == 0) return;

            // Znajdź wolne pole
            Pole poleDocelowe = null;
            foreach(Pole pole in EventSystem.eventSystem.GetComponent<ShopManager>().lawka)
            {
                if(pole.unit == null)
                {
                    poleDocelowe = pole;
                    break;
                }
            }
            if(poleDocelowe == null) return;

            // Wybierz losowy obiekt z pasujących
            GameObject randomUnitPrefab = matchingObjects[Random.Range(0, matchingObjects.Count)];
            
            // Utwórz nową jednostkę
            Vector3 pos = poleDocelowe.gameObject.transform.position;
            pos.z -= 2f;
            GameObject newUnit = Instantiate(randomUnitPrefab, pos, Quaternion.identity);
            
            // Przypisz do pola
            poleDocelowe.unit = newUnit;
            newUnit.GetComponent<DragObject>().pole = poleDocelowe;
            }
            Destroy(gameObject);
        }
    }
}