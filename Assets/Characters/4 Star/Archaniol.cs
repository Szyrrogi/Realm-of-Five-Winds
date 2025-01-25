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

    public override IEnumerator PreAction()
    {
        if(inBattle && tombCount < FightManager.Tomb.Count && healCount > 0)
        {
            tombCount = FightManager.Tomb.Count;
            if(FightManager.Tomb[tombCount - 1].y == (Enemy ? 1 : 0))
            {
                Pole newPole = FindPole();
                if(newPole != null && newPole !=  GetComponent<DragObject>().pole)
                {
                    healCount--;
                    GameObject newUnit = Instantiate(EventSystem.eventSystem.GetComponent<CharacterManager>().characters[(int)FightManager.Tomb[tombCount - 1].x], new Vector3(0,0,0), Quaternion.identity);
                    newUnit.GetComponent<Unit>().Enemy = Enemy;
                    newUnit.GetComponent<Unit>().Health = AP;
                    newPole.unit = newUnit;
                    newPole.Start();
                    yield return new WaitForSeconds(1);
                }
            }
        }
        yield return null;
    }

    public Pole FindPole()
    {
        int nr = GetComponent<DragObject>().pole.nr;
        int rzad = GetComponent<DragObject>().pole.line.nr;
        FightManager fightManager = EventSystem.eventSystem.GetComponent<FightManager>();
        Debug.Log(rzad + " - " + nr);
        Pole pole = fightManager.GetPole(rzad , nr);

            Debug.Log(rzad + " - " + nr);
        for(int i = nr; i >= 0; i--)
        {
            pole = fightManager.GetPole(rzad, i);
            if(pole != null && pole.unit == null)
            {
                return pole;
            }
        }
        return pole;
    }
}
