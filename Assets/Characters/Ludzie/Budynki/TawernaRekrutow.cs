using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class TawernaRekrutow : ShopVisitor
{
    public override List<GameObject> Filter(List<GameObject> prev)
    {
        // Filtruj listę, zwracając tylko te obiekty, które spełniają warunek
        return prev.Where(obj =>
        {
            Unit unit = obj.GetComponent<Unit>();
            return unit != null && unit.Cost <= 3;
        }).ToList();
    }

}
