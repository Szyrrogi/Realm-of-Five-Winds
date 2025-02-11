using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palladyn : Heros
{
    public int turyNiesmiertlenosci;


    public override int BeforDamage(GameObject enemy, int damage)
    {
        if(turyNiesmiertlenosci >= FightManager.Turn)
            return 0;
        return damage;
    }

}
