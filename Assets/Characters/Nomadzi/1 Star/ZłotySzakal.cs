using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ZłotySzakal : Heros
{
    int turaNow = -1;
     public override IEnumerator Jump()
    {
        List<int> wolne = new List<int>();
        FightManager fightManager = EventSystem.eventSystem.GetComponent<FightManager>();
        wolne.Add(Enemy ? 3 : 0);
        wolne.Add(Enemy ? 4 : 1);
        wolne.Add(Enemy ? 5 : 2);
        int rng;
        do
        {
            rng = Random.Range(0, wolne.Count);
            if (!fightManager.linie[wolne[rng]].EndBattle)
            {
                break;
            }
            else
            {
                wolne.RemoveAt(rng);
            }
        } while (wolne.Count != 0);
        Debug.Log(wolne.Count + " SKOK");
        if (wolne.Count != 0)
        {
            Debug.Log("weszlo");
            Linia line = fightManager.linie[wolne[rng]];
            List<Pole> help = line.pola.ToList();

            help.Reverse();

            foreach (var pole in help)
            {
                if (pole.unit == null)
                {

                    yield return StartCoroutine(Jump(pole.gameObject));
                    ReadyToJump = false;
                    Debug.Log(pole.nr);
                    if (turaNow != StatsManager.Round)
                    {
                        if (!Enemy)
                        {
                            MoneyManager.money += (Evolution ? 2 : 1);
                        }
                        GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
                        pop.GetComponent<PopUp>().SetText((Evolution ? 2 : 1).ToString(), Color.yellow);
                        yield return new WaitForSeconds(0.4f);
                        turaNow = StatsManager.Round;
                    }
                }
                    else
                    {
                        if (pole.unit.GetComponent<Unit>().Enemy != Enemy)
                        {
                            break;
                        }
                    }
            }

            Debug.Log("Jump " + Name + " " + wolne[rng]);
        }
        else
        {
            Skip = true;
        }
        yield return null;

    }
}
