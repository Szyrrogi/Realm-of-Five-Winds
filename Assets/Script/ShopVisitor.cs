using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopVisitor : MonoBehaviour
{
    public ShopManager shop;

    void Start()
    {
        shop = EventSystem.eventSystem.GetComponent<ShopManager>();
    }

    public void Update()
    {
        if(!GetComponent<Unit>().Enemy)
        {
            if(GetComponent<DragObject>().pole.line != null)
            {
                if(!EventSystem.eventSystem.GetComponent<ShopManager>().shopVisitors.Contains(this))
                    EventSystem.eventSystem.GetComponent<ShopManager>().shopVisitors.Add(this);
            }
            else
            {
                if(EventSystem.eventSystem.GetComponent<ShopManager>().shopVisitors.Contains(this))
                {
                    EventSystem.eventSystem.GetComponent<ShopManager>().shopVisitors.Remove(this);
                }
            }
        }
    }

    public virtual List<GameObject> Filter(List<GameObject> prev)
    {
        return prev;
    }

    public virtual void PostRoll()
    {

    }

    public virtual void FirstRoll()
    {

    }
}
