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

    [Space(10)]
    public int UpgradeLevel;
    public int UpgradeNeed;
    public bool Enemy;
    
    public int RealCost;
    [HideInInspector]
    public GameObject PopUp;
    public bool BoskaTarcza;

    void Awake()
    {
        Health = MaxHealth;
    }
    

    void Start()
    {
        PopUp = EventSystem.eventSystem.GetComponent<ObjectManager>().PopUp;
        if(UpgradeLevel == 0)
            UpgradeLevel = 1;
    }

    public void Update()
    {

    }


    public virtual void Sell()
    {
        if(RealCost != 0)
            MoneyManager.money += RealCost;
        else
            MoneyManager.money += Cost;
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

    public virtual IEnumerator TakeDamageMagic(Unit enemyUnit, int damageStart)
    {
        if(BoskaTarcza)
        {
            BoskaTarcza = false;
            GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
            pop.GetComponent<PopUp>().SetText("0", new Color(0.5f, 0f, 1f));
            yield return new WaitForSeconds(0.7f);
        }
        else
        {
            int damage = ReducedDamageMagic(damageStart);
            Health -= damage;
            GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
            pop.GetComponent<PopUp>().SetText(damage.ToString(), new Color(0.5f, 0f, 1f));
            yield return new WaitForSeconds(0.7f);
            if(Health <= 0)
            {
                StartCoroutine(Death());
                yield break;
            }
        }
    }

    public virtual IEnumerator TakeDamage(Unit enemyUnit, int damageStart)
    {
        if(BoskaTarcza)
        {
            BoskaTarcza = false;
            GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
            pop.GetComponent<PopUp>().SetText("0", Color.red);
            yield return new WaitForSeconds(0.7f);
        }
        else
        {
            int damage = ReducedDamage(damageStart);
            Health -= damage;
            GameObject pop = Instantiate(PopUp, gameObject.transform.position, Quaternion.identity);
            pop.GetComponent<PopUp>().SetText(damage.ToString(), Color.red);
            yield return new WaitForSeconds(0.7f);
            if(Health <= 0)
            {
                StartCoroutine(Death());
                yield break;
            }
        }
    }

    public virtual IEnumerator Death()
    {
        FightManager.Tomb.Add(new Vector2(Id, (Enemy ? 1 : 0)));
        Debug.Log(FightManager.Tomb[0]);
        Destroy(this.gameObject);
        yield return null;
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
        yield return new WaitForSeconds(0.2f);
        if(showOpis && DragObject.moveObject == null)
        {
            DescriptionManager.opis.SetActive(true);
            DescriptionManager.opis.GetComponent<DescriptionManager>().unit = this;
        }
    }
}
