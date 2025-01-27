using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static GameObject eventSystem;

    public CharacterManager characterManager;
    public EnemyManager enemyManager;
    public FightManager fightManager;
    public MoneyManager moneyManager;
    public ObjectManager objectManager;
    public SaveManager saveManager;
    public ShopManager shopManager;
    public StatsManager statsManager;
    
    private void Awake()
    {
        eventSystem = this.gameObject;
    }
}
