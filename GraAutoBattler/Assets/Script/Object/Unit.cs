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
    public List<CreatureType> Typy;

    public enum CreatureType
    {
        Zwierzęta,
        Wilki,
        Szkielety,
        OddziałyLordów, 
        Anioł,
        Drzewo,
        Wampir,
        Szczur
    }

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
    public bool Skip;
    public bool CanJump;
    public bool ReadyToJump;

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

    public virtual IEnumerator Summon(GameObject summonMinion)
    {
        Skip = true;
        GameObject newUnitObject = Instantiate(summonMinion, gameObject.transform.position, Quaternion.identity);
        Heros newUnit = newUnitObject.GetComponent<Heros>();
        newUnit.Enemy = Enemy;
        if(Enemy)
        {
            newUnit.GetComponent<SpriteRenderer>().flipX = !newUnit.GetComponent<SpriteRenderer>().flipX;
        }

        GetComponent<DragObject>().pole.unit = newUnitObject;
        GetComponent<DragObject>().pole.Start();
        yield return StartCoroutine(newUnit.OnBattleStart());
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
    public Unit PrefUnit()
    {
        int line = GetComponent<DragObject>().pole.line.nr;
        int pole = GetComponent<DragObject>().pole.nr;
        FightManager fightManager = EventSystem.eventSystem.GetComponent<FightManager>();
        if((line > 2 && Enemy) || (line <= 2 && !Enemy))
        {
            if(fightManager.GetPole(line, pole + 1) != null && fightManager.GetPole(line, pole + 1).unit != null)
            {
                return fightManager.GetPole(line, pole + 1).unit.GetComponent<Unit>();
            }
            else
            {
                return null;
            }
        }
        else
        {
            if(pole == 0)
            {
                line = (line + 3) % 6;
                if(fightManager.GetPole(line, pole) != null && fightManager.GetPole(line, pole).unit != null)
                {
                    return fightManager.GetPole(line, 0).unit.GetComponent<Unit>();
                }
            }
            else
            {
                if(fightManager.GetPole(line, pole - 1) != null && fightManager.GetPole(line, pole - 1).unit != null)
                {
                    return fightManager.GetPole(line, pole - 1).unit.GetComponent<Unit>();
                }
            }
        }
        return null;
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
        //Debug.Log(Name + " fight");
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
        yield return new WaitForSeconds(0.7f);
    }


    public virtual IEnumerator TakeDamage(Unit enemyUnit, int damageStart)
    {
        TypeDamage.typeDamage damageType = TypeDamage.typeDamage.Phisical;
        yield return StartCoroutine(TakeDamage(enemyUnit, damageStart, damageType));
    }

    public virtual IEnumerator TakeDamage(Unit enemyUnit, int damageStart, TypeDamage.typeDamage typeDamage)
    {
        if(PrefUnit() != null && PrefUnit().gameObject.GetComponent<Protektor>())
        {
            yield return StartCoroutine(PrefUnit().TakeDamage(enemyUnit, damageStart, typeDamage));
            yield break;
        }
        if(BoskaTarcza)
        {
            BoskaTarcza = false;
            GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
            pop.GetComponent<PopUp>().SetText("0", Color.red);
            yield return new WaitForSeconds(0.7f);
        }
        else
        {
            if(typeDamage == TypeDamage.typeDamage.Phisical)
            {
                int damage = ReducedDamage(damageStart);
                Health -= damage;
                GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
                pop.GetComponent<PopUp>().SetText(damage.ToString(), Color.red);
                yield return new WaitForSeconds(0.7f);
            }
            if(typeDamage == TypeDamage.typeDamage.TrueDamage)
            {
                int damage = damageStart;
                Health -= damage;
                GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
                pop.GetComponent<PopUp>().SetText(damage.ToString(), Color.white);
                yield return new WaitForSeconds(0.7f);
            }
            if(typeDamage == TypeDamage.typeDamage.Magic)
            {
                int damage = ReducedDamageMagic(damageStart);
                Health -= damage;
                GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
                pop.GetComponent<PopUp>().SetText(damage.ToString(), new Color(0.5f, 0f, 1f));
                yield return new WaitForSeconds(0.7f);
            }
            if(Health <= 0)
            {
                yield return new WaitForSeconds(0.2f);
                try{
                    
                    if(this != null)
                        StartCoroutine(Death());
                }
                catch (System.Exception ex)
                {
                    // Log the exception with the object's name
                    Debug.LogError("encountered an error during death: " + ex.Message);
                }
                yield break;
            }
        }
        yield break;
    }

    public virtual IEnumerator Death()
    {
        int archaniol = Archaniol.IsArchaniol(this);
        if(findPole().GetComponent<Pole>().unit != null && findPole().GetComponent<Pole>().unit.GetComponent<Czerw>())
        {
            findPole().GetComponent<Pole>().unit.GetComponent<Czerw>().buff();
        }
        Ghul.IsGhul(this);
        Loch.FirstDeathCheck(this);
        if(archaniol == 0)
        {
            archaniol = Nekromanta.IsNekromanta(this);
            if(archaniol == 0)
            {
                FightManager.Tomb.Add(new Vector2(Id, (Enemy ? 1 : 0)));
                Destroy(this.gameObject);
                yield return null;
            }
            else
            {
                yield return Summon(EventSystem.eventSystem.GetComponent<CharacterManager>().characters[archaniol]);
            }
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
    
    public virtual IEnumerator Jump()
    {
        yield return null;
    }

    public virtual void Morale()
    {

    }

    public void OnMouseExit()
    {
        showOpis = false;
        DescriptionManager.opis.SetActive(false);
    }

    IEnumerator WaitDescription()
    {
        yield return new WaitForSeconds(0.2f);
        if(showOpis && DragObject.moveObject == null)
        {
            DescriptionManager.opis.SetActive(true);
            DescriptionManager.opis.GetComponent<DescriptionManager>().unit = this;
        }
    }
}
