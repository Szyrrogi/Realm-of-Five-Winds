using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General : Heros
{
    public GameObject Rycerz;
    public override IEnumerator Death()
    {
        GameObject newUnitObject = Instantiate(Rycerz, gameObject.transform.position, Quaternion.identity);
        Heros newUnit = newUnitObject.GetComponent<Heros>();
        newUnit.Enemy = Enemy;
        GetComponent<DragObject>().pole.GetComponent<Pole>().unit = newUnitObject;
        GetComponent<DragObject>().pole.GetComponent<Pole>().Start();
        Destroy(gameObject);
        yield return null;
    }
}
