using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynSzczurSciana : Synergy
{
    public override IEnumerator BeforBattle()
    {
        
        for (int i = 0; i < 3; i++)
        {
            Linia line = EventSystem.eventSystem.GetComponent<FightManager>().linie[i + (Enemy ? 3 : 0)];
            List<Unit> newUnits = new List<Unit>(units);
            foreach (var pole in line.pola)
            {
                foreach (Unit unit in units)
                {
                    if (pole.unit != null && unit.Name == pole.unit.GetComponent<Unit>().Name)
                    {
                        Unit unitToRemove = newUnits.Find(u => u.Name == pole.unit.GetComponent<Unit>().Name);

                        if (unitToRemove != null)
                        {
                            newUnits.Remove(unitToRemove);
                        }
                    }
                }
                if (newUnits.Count == 0)
                {
                    yield return StartCoroutine(Buff(line.nr));
                    break;
                }
            }
        }

        yield return null;
    }

    public GameObject kanal;

    public IEnumerator Buff(int linenumber)
    {
        
        Linia line = EventSystem.eventSystem.GetComponent<FightManager>().linie[linenumber];
        foreach (var pole in line.pola)
        {
            if (pole.unit != null && pole.unit.GetComponent<Unit>().Name == units[0].Name)
            {
                Debug.Log("emm");
                yield return new WaitForSeconds(0.3f);
                Unit unit = pole.unit.GetComponent<Unit>();
                unit.Skip = true;
                GameObject newUnitObject = Instantiate(kanal, unit.gameObject.transform.position, Quaternion.identity);
                Unit newUnit = newUnitObject.GetComponent<Unit>();
                newUnit.Enemy = unit.Enemy;
                if(unit.Enemy)
                {
                    newUnit.GetComponent<SpriteRenderer>().flipX = !newUnit.GetComponent<SpriteRenderer>().flipX;
                }

                unit.GetComponent<DragObject>().pole.unit = newUnitObject;
                unit.GetComponent<DragObject>().pole.Start();
                //yield return StartCoroutine(newUnit.OnBattleStart());
                Destroy(unit.gameObject);
                yield return null;
            }
        }
    }

}
