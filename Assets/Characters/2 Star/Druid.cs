using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Druid : Heros
{
    public List <GameObject> zwierzeta;
    public override IEnumerator Death()
    {
        GameObject newUnitObject = Instantiate(zwierzeta[Random.Range(0,zwierzeta.Count-1)], gameObject.transform.position, Quaternion.identity);
        Heros newUnit = newUnitObject.GetComponent<Heros>();
        newUnit.Enemy = Enemy;

        GetComponent<DragObject>().pole.unit = newUnitObject;
        GetComponent<DragObject>().pole.Start();
        Destroy(gameObject);
        yield return null;
    }
}
