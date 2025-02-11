using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archaniol : Heros
{
    int tombCount = 0;
    public int healCount;
    public bool inBattle;

    public override IEnumerator OnBattleStart()
    {
        inBattle = true;
        yield return null;
    }

    public static int IsArchaniol(Unit unit)
    {
        foreach(Pole pole in unit.gameObject.GetComponent<DragObject>().pole.line.pola)
        {
            if(pole.unit != null && pole.unit.GetComponent<Archaniol>() && unit.Enemy == pole.unit.GetComponent<Unit>().Enemy && pole.unit.GetComponent<Archaniol>().healCount > 0)
            {
                Debug.Log("weszlo");
                pole.unit.GetComponent<Archaniol>().healCount--;
                return pole.unit.GetComponent<Archaniol>().AP;
            }
        }
        return 0;
    }

    // public override IEnumerator PreAction()
    // {
    //     if(inBattle && tombCount < FightManager.Tomb.Count && healCount > 0)
    //     {
    //         tombCount = FightManager.Tomb.Count;
    //         Vector2 unitToResolt = findDeath();
    //         if(unitToResolt.y == (Enemy ? 1 : 0) && unitToResolt != Vector2.zero)
    //         {
    //             Pole newPole = FindPole();
    //             if(newPole != null && newPole !=  GetComponent<DragObject>().pole)
    //             {
    //                 healCount--;
    //                 GameObject newUnit = Instantiate(EventSystem.eventSystem.GetComponent<CharacterManager>().characters[(int)unitToResolt.x], new Vector3(0,0,0), Quaternion.identity);
    //                 if(Enemy)
    //                 {
    //                     newUnit.GetComponent<SpriteRenderer>().flipX = !newUnit.GetComponent<SpriteRenderer>().flipX;
    //                 }
    //                 newUnit.GetComponent<Unit>().Enemy = Enemy;
    //                 newUnit.GetComponent<Unit>().Health = AP;
    //                 newPole.unit = newUnit;
    //                 newPole.Start();
    //                 yield return new WaitForSeconds(1 / FightManager.GameSpeed);
    //             }
    //         }
    //     }
    //     yield return null;
    // }

    // public Vector2 findDeath()
    // {
    //     if(inBattle)
    //     {
    //         for(int i = FightManager.Tomb.Count - 1; i >= 0; i--)
    //         {
    //             if(FightManager.Tomb[i].y == (Enemy? 1 : 0))
    //             {
    //                 return FightManager.Tomb[i];
    //             }
    //         }
    //     }
    //     return Vector2.zero;
    // }

    // public Pole FindPole()
    // {
    //     int nr = GetComponent<DragObject>().pole.nr;
    //     int rzad = GetComponent<DragObject>().pole.line.nr;
    //     FightManager fightManager = EventSystem.eventSystem.GetComponent<FightManager>();
    //     Debug.Log(rzad + " - " + nr);
    //     Pole pole = fightManager.GetPole(rzad , nr);

    //     Debug.Log(rzad + " - " + nr);
    //     for(int i = GetComponent<DragObject>().pole.line.pola.Count - 1; i >= 0; i--)
    //     {
    //         pole = fightManager.GetPole(rzad, i);
    //         if(pole != null && pole.unit == null && !pole.onlyBuilding)
    //         {
    //             return pole;
    //         }
    //     }
    //     return pole;
    // }
}
