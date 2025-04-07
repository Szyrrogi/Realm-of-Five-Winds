using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Szczur : Heros
{
    public GameObject szczur;
    public override IEnumerator Death()
    {
        if(Evolution)
        {
            if(findPole().GetComponent<Pole>().unit != null && findPole().GetComponent<Pole>().unit.GetComponent<Czerw>())
            {
                findPole().GetComponent<Pole>().unit.GetComponent<Czerw>().buff();
            }
            Ghul.IsGhul(this);
            yield return Summon(szczur); 
        }
        else
            StartCoroutine(base.Death());
        yield return null;
    }

    public override IEnumerator Summon(GameObject summonMinion)
    {
        Skip = true;
        GameObject newUnitObject = Instantiate(summonMinion, gameObject.transform.position, Quaternion.identity);
        Heros newUnit = newUnitObject.GetComponent<Heros>();
        newUnit.Enemy = Enemy;
        if(Enemy)
        {
            newUnit.GetComponent<SpriteRenderer>().flipX = !newUnit.GetComponent<SpriteRenderer>().flipX;
        }
        newUnit.Health = MaxHealth;
        newUnit.Attack = Attack;
        newUnit.MaxHealth = MaxHealth;
        GetComponent<DragObject>().pole.unit = newUnitObject;
        GetComponent<DragObject>().pole.Start();
        yield return StartCoroutine(newUnit.OnBattleStart());
        Destroy(gameObject);
        yield return null;
    }
}
