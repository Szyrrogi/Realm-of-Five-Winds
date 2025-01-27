using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static int money;
    public static int income;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI incomeText;

    void Start()
    {
        money = 5;
        income = 5;
    }

    void Update()
    {
        moneyText.text = money.ToString();
        incomeText.text = income.ToString();
    }

    public static void ActiveIncom()
    {
        money += income;
        //if(StatsManager.Round % 2 == 0)
        if(income < 12)
            income++;
    }
}
