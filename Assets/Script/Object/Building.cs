using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : Unit
{
    public Text HealthText;

    void Update()
    {
        HealthText.text = Health.ToString();
        base.Update();
    }
}
