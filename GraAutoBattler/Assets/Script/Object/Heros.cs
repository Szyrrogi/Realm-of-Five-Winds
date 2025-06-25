using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Heros : Unit
{
    public Text AttackText;
    public Text HealthText;

    public GameObject EvolveHeroes;
    public bool Evolution;
    
    Image attackRamka;

    public static bool SzescsetHP;

    GameObject ShildObject;

    public virtual void Update()
    {
        if(ShildObject == null && BoskaTarcza)
        {
            ShildObject = Instantiate(
                EventSystem.eventSystem.GetComponent<FightManager>().ShildObject, 
                gameObject.transform.position, 
                Quaternion.identity, 
                AttackText.transform.parent.transform.parent
            );
        }
        if(ShildObject != null && !BoskaTarcza)
        {
            Destroy(ShildObject);
        }
        if(Health >= 600 && !SzescsetHP && !Enemy)
        {
            SzescsetHP = true;
            Bestiariusz.AddAchivments(10);
        }
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
                yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, AP)),TypeDamage.typeDamage.Magic));
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
                        if(!attackAP)
                            yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, Attack))));
                        else
                            yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, AP)),TypeDamage.typeDamage.Magic));
                    }
                }
            }
        }
        yield return null;
    }

    public override void Morale()
    {
        ShowPopUp("MORALE", Color.green);
        Attack += (int)(Attack * 0.2f);
        AP += (int)(AP * 0.2f);
        Health += (int)(Health * 0.2f);
        MaxHealth += (int)(MaxHealth * 0.2f);
    }

    public override void Bezradnosc()
    {
        ShowPopUp("BEZRADNOŚĆ", Color.red);
        Attack -= (int)(Attack * 0.2f);
        AP -= (int)(AP * 0.2f);
        Health -= (int)(Health * 0.2f);
        MaxHealth -= (int)(MaxHealth * 0.2f);
    }

    

    public override IEnumerator Jump()
    {
        List<int> wolne = new List<int>();
        FightManager fightManager = EventSystem.eventSystem.GetComponent<FightManager>();
        wolne.Add(Enemy ? 3: 0);
        wolne.Add(Enemy ? 4: 1);
        wolne.Add(Enemy ? 5: 2);
        int rng;
        do{
            rng = Random.Range(0, wolne.Count);
            if(!fightManager.linie[wolne[rng]].EndBattle)
            {
                break;
            }
            else
            {
                wolne.RemoveAt(rng);
            }
        }while(wolne.Count != 0);
        Debug.Log(wolne.Count + " SKOK");
        if (wolne.Count != 0)
        {
            Debug.Log("weszlo");
            Linia line = fightManager.linie[wolne[rng]];
            List<Pole> help = line.pola.ToList();

            // Odwracamy kopię listy
            help.Reverse();

            // Przechodzimy przez odwróconą kopię
            foreach (var pole in help)
            {
                if (pole.unit == null)
                {
                    yield return StartCoroutine(Jump(pole.gameObject));
                    ReadyToJump = false;
                    Debug.Log(pole.nr);
                    
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

    public virtual void UpgradeHeros(Unit newUnit)
    {
        Health += (int)(newUnit.Health * 0.4f);
        MaxHealth += (int)(newUnit.MaxHealth * 0.4f);
        Attack += (int)(newUnit.Attack * 0.4f);
        AP += (int)(newUnit.AP * 0.4f);
        Cost += (int)(newUnit.Cost);
        RealCost += (int)(newUnit.RealCost);
        Initiative += (int)(newUnit.Initiative * 0.1f);
        UpgradeLevel += newUnit.UpgradeLevel;
        GetComponent<DragObject>().pole.unit = gameObject;

        if(GetComponent<Wizard>())
        {
            if(newUnit.gameObject.GetComponent<Wizard>().spell.Cost > GetComponent<Wizard>().spell.Cost)
            {
                Debug.Log(GetComponent<Wizard>().spell.gameObject.name);
                GetComponent<Wizard>().AddSpell(newUnit.gameObject.GetComponent<Wizard>().spell);
                Debug.Log("podmianka");
                Debug.Log(GetComponent<Wizard>().spell.gameObject.name);
            }
        }

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
        newUnit.RealCost = Cost;
        newUnit.Evolution = true;

        if(newUnit.gameObject.GetComponent<Wizard>())
        {
            
            newUnit.gameObject.GetComponent<Wizard>().AddSpell(GetComponent<Wizard>().spell);
            
        }

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
                    yield return StartCoroutine(Jump(findPole()));
                }
            }
        }
        else
            yield return StartCoroutine(Jump(findPole()));
        yield return null;
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
