using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drzewo : Heros
{
    public static int IleDrzew(bool isEnemy)
    {
        int ile = 0;
        foreach(Unit unit in EventSystem.eventSystem.GetComponent<FightManager>().units)
        {
            if(unit.Typy.Contains(CreatureType.Drzewo) && unit.Enemy == isEnemy)
            {
                ile++;
            }
        }
        return ile;
    }

    public override void Evolve()
    {
        GameObject newUnitObject = Instantiate(EvolveHeroes, gameObject.transform.position, Quaternion.identity);
        Heros newUnit = newUnitObject.GetComponent<Heros>();

        newUnit.Cost = Cost;

        GetComponent<DragObject>().pole.unit = newUnitObject;
        GetComponent<DragObject>().pole.Start();
        Destroy(gameObject);
    }

    public override IEnumerator Move()
    {
        yield return null;
    }

    public override void Sell()
    {
        EventSystem.eventSystem.GetComponent<ShopManager>().FreeRoll++;
        if(RealCost != 0)
            MoneyManager.money += RealCost - 1;
        else
            MoneyManager.money += Cost - 1;
        Destroy(gameObject);
    }

    public void APBuff()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null)
            {
                pole.unit.GetComponent<Unit>().AP += 7;
                pole.unit.GetComponent<Unit>().ShowPopUp("+7", new Color(0.5f, 0f, 1f));
            }
        }
    }
    public void DMGBuff()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null )
            {
                pole.unit.GetComponent<Unit>().Attack += 7;
                pole.unit.GetComponent<Unit>().ShowPopUp("+7", Color.yellow);
            }
        }
    }
    public void HPBuff()
    {
        foreach(Pole pole in GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null)
            {
                pole.unit.GetComponent<Unit>().Health += 9;
                pole.unit.GetComponent<Unit>().MaxHealth += 9;
                pole.unit.GetComponent<Unit>().ShowPopUp("+9", Color.green);
            }
        }
    }
}
