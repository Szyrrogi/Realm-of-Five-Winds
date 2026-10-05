using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WładcaPiorunów : Heros
{
    public override IEnumerator Fight()
    {
        if(findPole() != null && findPole().GetComponent<Pole>().unit != null && Enemy != findPole().GetComponent<Pole>().unit.GetComponent<Unit>().Enemy)
            {
                Unit enemyUnit = findPole().GetComponent<Pole>().unit.GetComponent<Unit>();
                yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, AP)),TypeDamage.typeDamage.Magic));
                int APnow = AP;
                Unit unitNow = enemyUnit;
                while(APnow > 10 && enemyUnit != null && enemyUnit.PrefUnit() != null) 
                {
                    if(unitNow.PrefUnit() != null)
                    {
                        APnow -= 10;
                        Debug.Log(APnow);
                        Unit unitHelp = unitNow.PrefUnit();
                        yield return StartCoroutine(unitNow.PrefUnit().TakeDamage(this, unitNow.PrefUnit().BeforDamage(gameObject, BeforAttack(unitNow.PrefUnit().gameObject, APnow)),TypeDamage.typeDamage.Magic));
                        
                        unitNow = unitHelp;
                        
                        if(Evolution)
                            APnow += 10;
                    }
                    else
                    {
                        break;
                    }
                }

                    
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
                            yield return StartCoroutine(enemyUnit.TakeDamage(this, enemyUnit.BeforDamage(gameObject, BeforAttack(enemyUnit.gameObject, AP)),TypeDamage.typeDamage.Magic));
                            int APnow = AP;
                            Unit unitNow = enemyUnit;
                            while(APnow > 10 && enemyUnit != null && enemyUnit.PrefUnit() != null) 
                            {
                                if(unitNow.PrefUnit() != null)
                                {
                                    APnow -= 10;
                                    Debug.Log(APnow);
                                    Unit unitHelp = unitNow.PrefUnit();
                                    yield return StartCoroutine(unitNow.PrefUnit().TakeDamage(this, unitNow.PrefUnit().BeforDamage(gameObject, BeforAttack(unitNow.PrefUnit().gameObject, APnow)),TypeDamage.typeDamage.Magic));
                                    
                                    unitNow = unitHelp;
                                    
                                    if(Evolution)
                                        APnow += 10;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
    }
}
