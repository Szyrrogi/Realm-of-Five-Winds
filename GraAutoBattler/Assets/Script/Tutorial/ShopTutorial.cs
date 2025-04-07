using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ShopTutorial : ShopManager
{
    public void Start()
    {
        CharacterManager characterManager = EventSystem.eventSystem.GetComponent<CharacterManager>();
        filteredObjects = characterManager.characters.ToList();

        if(filteredObjects.Count == 0)
        {
            filteredObjects.Add(Chochil);
        }

            for(int i = 0; i < 5; i++)
            {
                int rng = (i == 2 && Tutorial.Step == 1) ? 12 : 101;

                character[i].unit = filteredObjects[rng];

                filteredObjects[rng].GetComponent<Unit>().RealCost = 0;
                character[i].image.sprite = filteredObjects[rng].GetComponent<SpriteRenderer>().sprite;
                character[i].name.text = filteredObjects[rng].GetComponent<Unit>().Name;
                character[i].price.text = 
                (filteredObjects[rng].GetComponent<Unit>().RealCost == 0 ? filteredObjects[rng].GetComponent<Unit>().Cost.ToString() : filteredObjects[rng].GetComponent<Unit>().RealCost.ToString());
                if(character[i].unit.GetComponent<Heros>())
                    character[i].SetStats();
                else
                    character[i].stats.SetActive(false);

            
            }
    }

    public void FakeRoll()
    { if(Tutorial.Step > 7)
    {
        Roll();
    }
    else
    {
       if(MoneyManager.money >= RollCost || FreeRoll > 0)
        {
            if(FreeRoll > 0)
                FreeRoll--;
            else
                MoneyManager.money -= RollCost;
       CharacterManager characterManager = EventSystem.eventSystem.GetComponent<CharacterManager>();
        filteredObjects = characterManager.characters.ToList();

        if(filteredObjects.Count == 0)
        {
            filteredObjects.Add(Chochil);
        }

            for(int i = 0; i < 5; i++)
            {
                int rng = (i == 2 ) ? 5 : 101;

                character[i].unit = filteredObjects[rng];

                filteredObjects[rng].GetComponent<Unit>().RealCost = 0;
                character[i].image.sprite = filteredObjects[rng].GetComponent<SpriteRenderer>().sprite;
                character[i].name.text = filteredObjects[rng].GetComponent<Unit>().Name;
                character[i].price.text = 
                (filteredObjects[rng].GetComponent<Unit>().RealCost == 0 ? filteredObjects[rng].GetComponent<Unit>().Cost.ToString() : filteredObjects[rng].GetComponent<Unit>().RealCost.ToString());
                if(character[i].unit.GetComponent<Heros>())
                    character[i].SetStats();
                else
                    character[i].stats.SetActive(false);
            } 
            EventSystem.eventSystem.GetComponent<Tutorial>().ShowPorada(8);
        }
    }
}
}
