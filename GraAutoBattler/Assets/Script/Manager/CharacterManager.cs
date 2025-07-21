using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterManager : MonoBehaviour
{
    public List <GameObject> characters;
    public List <GameObject> Spells;

    void Start()
    {
        int i = 0;
        foreach (GameObject character in characters)
        {
            character.GetComponent<Unit>().Id = i;
            i++;
            if (character.GetComponent<Heros>() && !character.GetComponent<Building>())
            {
                Heros heros = character.GetComponent<Heros>();
                if (heros.Star != 0 && !heros.Evolution && !heros.EvolveHeroes.GetComponent<WesołaBrygada>() && !character.GetComponent<Jaskolka>())
                {
                     Heros lepszy = heros.EvolveHeroes.GetComponent<Heros>();
                    if (lepszy.Name.Length == 0)
                    {
                        //Debug.Log(heros.Name[0]);
                        lepszy.Name = new string[5];
                        lepszy.Name = heros.Name;
                    }

                    lepszy.MaxHealth = (int)(heros.MaxHealth);
                    lepszy.Health = (int)(heros.Health);

                    lepszy.Attack = (int)(heros.Attack);
                    lepszy.AP = (int)(heros.AP);
                    lepszy.Cost = (int)(heros.Cost);
                    lepszy.Initiative = (int)(heros.Initiative);

                    for(int j = 0;  j< heros.UpgradeNeed -1; j++)
                    {
                        lepszy.MaxHealth += (int)(heros.MaxHealth * 0.4);
                        lepszy.Health += (int)(heros.Health * 0.4);

                        lepszy.Attack += (int)(heros.Attack * 0.4f);
                        lepszy.AP += (int)(heros.AP * 0.4f);
                        lepszy.Cost += (int)(heros.Cost);
                        lepszy.Initiative += (int)(heros.Initiative * 0.1f);
                    }
                }
            }
        }
        i = 0;
        foreach(GameObject spell in Spells)
        {
            spell.GetComponent<Unit>().Id = i;
            i++;
        }
    }
}
