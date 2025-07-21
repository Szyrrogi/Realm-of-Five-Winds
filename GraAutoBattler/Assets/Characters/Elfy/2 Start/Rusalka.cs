using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rusalka : Heros
{
    public override IEnumerator OnBattleStart()
    {
        for (int i = 0; i < (Evolution ? 2 : 1); i++)
        {

            List<Pole> pola = new List<Pole>();
            foreach (Pole pole in GetComponent<DragObject>().pole.line.pola)
            {
                if (pole.unit == null)
                {
                    pola.Add(pole);
                }
            }
            if (pola.Count > 0)
            {
                int randomIndex = Random.Range(0, pola.Count);
                Pole randomPole = pola[randomIndex];

                GameObject newUnitObject = Instantiate(this.gameObject, gameObject.transform.position, Quaternion.identity);
                Heros newUnit = newUnitObject.GetComponent<Heros>();
                newUnit.Enemy = Enemy;
                if (Enemy)
                {
                    newUnit.GetComponent<SpriteRenderer>().flipX = !newUnit.GetComponent<SpriteRenderer>().flipX;
                }

                randomPole.unit = newUnitObject;
                randomPole.Start();
                newUnitObject.transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}