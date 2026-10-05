using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprzedawca : Heros
{

    public ShopManager shop;

    void Start()
    {
        shop = EventSystem.eventSystem.GetComponent<ShopManager>();
        base.Start();
    }
    public override void AfterBuy()
    {
        RealCost = Cost + 1;
    }

    public override void UpgradeHeros(Unit newUnit)
    {
        RealCost-=1;
        base.UpgradeHeros(newUnit);
    }

    public override void Evolve()
    {
        for(int i = 0; i < 5; i++)
        {
                shop.character[i].unit.GetComponent<Unit>().RealCost = shop.character[i].unit.GetComponent<Unit>().Cost - 1;
                shop.character[i].price.text = (shop.character[i].unit.GetComponent<Unit>().RealCost.ToString());
        }
        Debug.Log("czesc");
        GameObject newUnitObject = Instantiate(EvolveHeroes, gameObject.transform.position, Quaternion.identity);
        Heros newUnit = newUnitObject.GetComponent<Heros>();

        newUnit.Cost = Cost;

        newUnit.Initiative = Initiative;
        newUnit.Health = Health;
        newUnit.MaxHealth = MaxHealth;
        newUnit.Attack = Attack;
        newUnit.Defense = Defense;
        newUnit.AP = AP;
        newUnit.MagicResist = MagicResist;

        newUnit.UpgradeLevel = 0;
        newUnit.UpgradeNeed = 0;
        newUnit.RealCost = Cost + 1;
        newUnit.Evolution = true;

        if(newUnit.gameObject.GetComponent<Wizard>())
        {
            
            newUnit.gameObject.GetComponent<Wizard>().AddSpell(GetComponent<Wizard>().spell);
            
        }
        newUnit.PlaySound(SoundEffect.evolve);
        GetComponent<DragObject>().pole.unit = newUnitObject;
        GetComponent<DragObject>().pole.Start();
        Destroy(gameObject);
    }
}
