using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kawalerzysta : Heros
{
    public override IEnumerator Action()
    {
        GameObject pole = findPole();
        if(pole != null && pole.GetComponent<Pole>().unit == null)
        while(pole != null && pole.GetComponent<Pole>().unit == null)
        {
            float temp = Attack;
            temp *= (Evolution ? 2f : 1.5f);
            Attack = (int)temp;
            yield return StartCoroutine(Jump(findPole()));
            pole = findPole();
        }
        yield return null;
    }
}
