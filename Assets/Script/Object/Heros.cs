using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heros : Unit
{
    public Text AttackText;
    public Text HealthText;

    public GameObject EvolveHeroes;
    public bool Evolution;
    public bool attackAP;
    Image attackRamka;

    public void Update()
    {
        if(attackAP)
        {
            attackRamka = AttackText.GetComponentInParent<Image>();
            attackRamka.color = new Color(0.5f, 0f, 1f);
            AttackText.text = AP.ToString();
        }
        else
            AttackText.text = Attack.ToString();
        HealthText.text = Health.ToString();
        base.Update();
    }
    public override IEnumerator Fight()
    {

        if(findPole() != null && findPole().GetComponent<Pole>().unit != null && Enemy != findPole().GetComponent<Pole>().unit.GetComponent<Unit>().Enemy)
        {
            Unit enemyUnit = findPole().GetComponent<Pole>().unit.GetComponent<Unit>();
            if(!attackAP)
                yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, Attack))));
            else
                yield return StartCoroutine(enemyUnit.TakeDamageMagic(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, AP))));
        }
        else
        {
            if(Range > 0)
            {
                if(findPole() != null)
                {
                    GameObject pole = findPole();
                    if(findPole(pole.GetComponent<Pole>()) != null && findPole(pole.GetComponent<Pole>()).GetComponent<Pole>().unit != null && Enemy != findPole(pole.GetComponent<Pole>()).GetComponent<Pole>().unit.GetComponent<Unit>().Enemy)
                    {
                        Unit enemyUnit = findPole(pole.GetComponent<Pole>()).GetComponent<Pole>().unit.GetComponent<Unit>();
                        yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject ,BeforAttack(enemyUnit.gameObject, Attack))));
                    }
                }
            }
        }

        yield return null;
        if(Health <= 0)
        {
            StartCoroutine(Death());
        }
    }

    public virtual void UpgradeHeros(Unit newUnit)
    {
        Health += (int)(newUnit.Health * 0.3f);
        MaxHealth += (int)(newUnit.MaxHealth * 0.3f);
        Attack += (int)(newUnit.Attack * 0.3f);
        AP += (int)(newUnit.AP * 0.3f);
        Cost += (int)(newUnit.Cost - 1);
        Initiative += (int)(newUnit.Initiative * 0.1f);
        UpgradeLevel += newUnit.UpgradeLevel;
        GetComponent<DragObject>().pole.unit = gameObject;

        Destroy(newUnit.gameObject);

        if(!Evolution)
        {
            GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
            pop.GetComponent<PopUp>().SetText(UpgradeLevel + "/" + UpgradeNeed,  Color.white);
            
            if(UpgradeLevel >= UpgradeNeed)
            {
                Evolve();
            }
        }
    }

    public virtual void Evolve()
    {
        GameObject newUnitObject = Instantiate(EvolveHeroes, gameObject.transform.position, Quaternion.identity);
        Heros newUnit = newUnitObject.GetComponent<Heros>();

        newUnit.Cost = Cost;

        newUnit.Initiative = Initiative;
        newUnit.Health = Health;
        newUnit.Attack = Attack;
        newUnit.Defense = Defense;
        newUnit.Range = Range;
        newUnit.AP = AP;
        newUnit.MagicResist = MagicResist;

        newUnit.UpgradeLevel = 0;
        newUnit.UpgradeNeed = 0;
        newUnit.Evolution = true;

        GetComponent<DragObject>().pole.unit = newUnitObject;
        GetComponent<DragObject>().pole.Start();
        Destroy(gameObject);
        
    }

    public override IEnumerator Move()
    {
        if(Range > 0)
        {
            GameObject pole = findPole();
            if(pole != null && pole.GetComponent<Pole>().unit == null)
            {
                pole = findPole(pole.GetComponent<Pole>());
                if(pole != null && (pole.GetComponent<Pole>().unit == null || Enemy == pole.GetComponent<Pole>().unit.GetComponent<Unit>().Enemy))
                {
                    Debug.Log(Name);
                    yield return StartCoroutine(Jump(findPole()));
                }
            }
        }
        else
            yield return StartCoroutine(Jump(findPole()));
        yield return null;
    }

    public GameObject findPole()
    {
        return findPole(GetComponent<DragObject>().pole);
    }

    public GameObject findPole(Pole pole)
    {
        if(Enemy == pole.line.GetComponent<Linia>().enemyLine)
        {
            if(pole.nr > 0)
                return pole.line.GetComponent<Linia>().pola[pole.nr - 1].gameObject;
            else
            {
                if(pole.nr == 0)
                {
                    int nrLini = pole.line.GetComponent<Linia>().nr;
                    Linia linia;
                    if(nrLini < 3)
                    {
                        linia = EventSystem.eventSystem.GetComponent<FightManager>().linie[nrLini + 3];
                    }
                    else
                    {
                        linia = EventSystem.eventSystem.GetComponent<FightManager>().linie[nrLini - 3];
                    }
                    return linia.pola[0].gameObject;
                }
            }
        }
        else
        {
            if(pole.nr != pole.line.GetComponent<Linia>().pola.Count - 1 && (pole.line.GetComponent<Linia>().pola[pole.nr + 1].unit == null || pole != GetComponent<DragObject>().pole))
                return pole.line.GetComponent<Linia>().pola[pole.nr + 1].gameObject;
        }
        return null;
    }

    public IEnumerator Jump(GameObject pole)
    {
        if(pole != null && pole.GetComponent<Pole>().unit == null)
        {
            Vector3 startPosition = transform.position; // Pozycja początkowa obiektu
            Vector3 targetPosition = pole.transform.position; // Pozycja pola, do której zmierzamy
            targetPosition.z = -2;
            float elapsedTime = 0f;
            float duration = 1f;

            while (elapsedTime < duration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime; // Zwiększamy czas o czas klatki
                yield return null; // Czekamy na kolejną klatkę
            }

            transform.position = targetPosition; // Upewniamy się, że obiekt jest na końcowej pozycji
            GetComponent<DragObject>().pole.unit = null;
            GetComponent<DragObject>().pole = pole.GetComponent<Pole>();
            pole.GetComponent<Pole>().unit = gameObject;
        }
    }
}
