using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Basic Information")]
    public int Id;
    public string Name;
    public int Cost;
    public int Star;

    [Space(10)]
    [Header("Stats")]
    public int Initiative;
    public int MaxHealth;
    public int Health;
    public int Attack;
    public int Defense;
    public int Range;
    public int AP;
    public int MagicResist;
    public string Description;
    public int CPU;

    [Space(10)]
    public int UpgradeLevel;
    public int UpgradeNeed;
    public bool Enemy;
    public Fraction.fractionType fraction;
    
    public int RealCost;
    [HideInInspector]
    public GameObject PopUp;
    public bool BoskaTarcza;

    public bool attackAP;

    void Awake()
    {
        Health = MaxHealth;
    }
    

    public virtual void Start()
    {
        PopUp = EventSystem.eventSystem.GetComponent<ObjectManager>().PopUp;
        if(UpgradeLevel == 0)
            UpgradeLevel = 1;
        if(CPU == 0)
            CPU = Random.Range(0,100);
    }

    public void Update()
    {

    }

    public IEnumerator Summon(GameObject summonMinion)
    {
        GameObject newUnitObject = Instantiate(summonMinion, gameObject.transform.position, Quaternion.identity);
        Heros newUnit = newUnitObject.GetComponent<Heros>();
        newUnit.Enemy = Enemy;
        if(Enemy)
        {
            newUnit.GetComponent<SpriteRenderer>().flipX = !newUnit.GetComponent<SpriteRenderer>().flipX;
        }

        GetComponent<DragObject>().pole.unit = newUnitObject;
        GetComponent<DragObject>().pole.Start();
        Destroy(gameObject);
        yield return null;
    }

    public virtual void Sell()
    {
        if(RealCost != 0)
            MoneyManager.money += RealCost - 1;
        else
            MoneyManager.money += Cost - 1;
        Destroy(gameObject);
    }

    public virtual IEnumerator Move()
    {
        //Debug.Log(Name + " moved");
        yield return null;
    }

    public virtual IEnumerator Fight()
    {
        //Debug.Log(Name + " fight");
        yield return null;
    }

    public virtual IEnumerator Action()
    {
        //Debug.Log(Name + " fight");
        yield return null;
    }

    public virtual IEnumerator PreAction()
    {
        //Debug.Log(Name + " fight");
        yield return null;
    }

    public virtual int BeforAttack(GameObject enemy, int damage)
    {
        return damage;
    }

    public virtual int BeforDamage(GameObject enemy, int damage)
    {
        return damage;
    }

    public virtual string DescriptionEdit()
    {
        return Description;
    }

    public virtual void AfterBuy()
    {

    }

    public virtual void AfterBattle()
    {
        Debug.Log(Name + " fight");
    }

    public virtual IEnumerator Heal(int heal)
    {
        if(heal + Health > MaxHealth)
        {
            heal = MaxHealth - Health;
        }
        Health += heal;
        GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
        pop.GetComponent<PopUp>().SetText(heal.ToString(), Color.green);
        yield return new WaitForSeconds(0.7f / FightManager.GameSpeed);
    }


    public virtual IEnumerator TakeDamage(Unit enemyUnit, int damageStart)
    {
        TypeDamage.typeDamage damageType = TypeDamage.typeDamage.Phisical;
        yield return StartCoroutine(TakeDamage(enemyUnit, damageStart, damageType));
    }

    public virtual IEnumerator TakeDamage(Unit enemyUnit, int damageStart, TypeDamage.typeDamage typeDamage)
    {
        if(BoskaTarcza)
        {
            BoskaTarcza = false;
            GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
            pop.GetComponent<PopUp>().SetText("0", Color.red);
            yield return new WaitForSeconds(0.7f / FightManager.GameSpeed);
        }
        else
        {
            if(typeDamage == TypeDamage.typeDamage.Phisical)
            {
                int damage = ReducedDamage(damageStart);
                Health -= damage;
                GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
                pop.GetComponent<PopUp>().SetText(damage.ToString(), Color.red);
                yield return new WaitForSeconds(0.7f / FightManager.GameSpeed);
            }
            if(typeDamage == TypeDamage.typeDamage.TrueDamage)
            {
                int damage = damageStart;
                Health -= damage;
                GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
                pop.GetComponent<PopUp>().SetText(damage.ToString(), Color.white);
                yield return new WaitForSeconds(0.7f / FightManager.GameSpeed);
            }
            if(typeDamage == TypeDamage.typeDamage.Magic)
            {
                int damage = ReducedDamageMagic(damageStart);
                Health -= damage;
                GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
                pop.GetComponent<PopUp>().SetText(damage.ToString(), new Color(0.5f, 0f, 1f));
                yield return new WaitForSeconds(0.7f / FightManager.GameSpeed);
            }
            if(Health <= 0)
            {
                StartCoroutine(Death());
                yield break;
            }
        }
        yield break;
    }

    public virtual IEnumerator Death()
    {
        int archaniol = Archaniol.IsArchaniol(this);
        if(archaniol == 0)
        {
            FightManager.Tomb.Add(new Vector2(Id, (Enemy ? 1 : 0)));
            Destroy(this.gameObject);
            yield return null;
        }
        else
        {
            ShowPopUp(archaniol.ToString(), Color.green);
            Health = archaniol;
        }
    }

    public GameObject findPole()
    {
        return findPole(GetComponent<DragObject>().pole);
    }

    public GameObject findPole(Pole pole)
    {
        if(Enemy == pole.line.enemyLine)
        {
            if(pole.nr > 0)
                return pole.line.pola[pole.nr - 1].gameObject;
            else
            {
                if(pole.nr == 0)
                {
                    int nrLini = pole.line.nr;
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
            if(pole.nr != pole.line.pola.Count - 1) //&& (pole.line.pola[pole.nr + 1].unit == null || pole != GetComponent<DragObject>().pole))
                return pole.line.pola[pole.nr + 1].gameObject;
        }
        return null;
    }

    private bool showOpis;

    int ReducedDamageMagic(int damage)
    {
        float help = damage;
        for(int i = 0; i < MagicResist; i++)
        {
            help *= 0.99f;
        }
        damage = (int)help;
        return damage;
    }

    int ReducedDamage(int damage)
    {
        float help = damage;
        for(int i = 0; i < Defense; i++)
        {
            help *= 0.99f;
        }
        damage = (int)help;
        return damage;
    }

    public void ShowPopUp(string text, Color color)
    {
        GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
        pop.GetComponent<PopUp>().SetText(text, color);
    }

    public virtual IEnumerator OnBattleStart()
    {
        yield return null;
    }

    public virtual IEnumerator OnBattleEnd()
    {
        yield return null;
    }

    public void OnMouseEnter()
    {
        showOpis = true;
        StartCoroutine(WaitDescription());
    }

    public void OnMouseExit()
    {
        showOpis = false;
        DescriptionManager.opis.SetActive(false);
    }

    IEnumerator WaitDescription()
    {
        yield return new WaitForSeconds(0.2f / FightManager.GameSpeed);
        if(showOpis && DragObject.moveObject == null)
        {
            DescriptionManager.opis.SetActive(true);
            DescriptionManager.opis.GetComponent<DescriptionManager>().unit = this;
        }
    }
}
