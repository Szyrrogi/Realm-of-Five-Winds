using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loch : Building
{
    public static int FirstDeath;
    public static int FirstDeathFriendly;
    public bool nekro;
    public ShopManager shop;

    void Start()
    {
        shop = EventSystem.eventSystem.GetComponent<ShopManager>();
    }
    public override IEnumerator OnBattleStart()
    {
        FirstDeath = -1;
        FirstDeathFriendly = -1;
        yield return new WaitForSeconds(0);
    }
    public override IEnumerator OnBattleEnd()
    {
        if(!Enemy && !ShopManager.isLoock && ((FirstDeath != -1 && !nekro) || (FirstDeathFriendly != -1 && nekro)))
        {
        
        int i =  Random.Range(0, 5);
        GameObject unit = null;
        if(FirstDeath != -1)
            unit = EventSystem.eventSystem.GetComponent<CharacterManager>().characters[FirstDeath];
        if(nekro)
        {
            unit = EventSystem.eventSystem.GetComponent<CharacterManager>().characters[FirstDeathFriendly];
        }
        int rng = unit.GetComponent<Unit>().Id;
        shop.character[i].image.sprite = unit.GetComponent<SpriteRenderer>().sprite;
        shop.character[i].name.text = unit.GetComponent<Unit>().Name;
        shop.character[i].price.text = unit.GetComponent<Unit>().Cost.ToString();
        shop.character[i].unit = unit;
        }
        yield return new WaitForSeconds(0);
    }
    public static void FirstDeathCheck(Unit unit)
    {
        if(FirstDeath == -1 && unit.Enemy)
        {
            FirstDeath = unit.Id;
            
        }
        else if(FirstDeathFriendly == -1 && !unit.Enemy)
        {
            FirstDeathFriendly = unit.Id;
            
        }
    }
}
