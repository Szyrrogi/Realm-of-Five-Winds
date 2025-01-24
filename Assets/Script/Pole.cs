using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    public GameObject line;
    public GameObject unit;
    public GameObject potencialUnit;

    private Camera mainCamera;

    public bool onlyHeros;
    public bool onlyBuilding;

    public int nr;

    public void Start()
    {
        mainCamera = Camera.main;
        if(unit != null)
        {
            unit.GetComponent<DragObject>().pole = gameObject;
            Vector3 newPosition = transform.position;

            newPosition.z -= 2f;

            unit.transform.position = newPosition;

            if(!EventSystem.eventSystem.GetComponent<FightManager>().units.Contains(unit.GetComponent<Unit>()))
            {
                EventSystem.eventSystem.GetComponent<FightManager>().setList();
                EventSystem.eventSystem.GetComponent<FightManager>().SortUnits();
            }
        }
    }
    public void Update()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        float distance = Vector3.Distance(mousePosition, transform.position);
        if (distance < 0.8f && DragObject.moveObject != null && (!onlyHeros || DragObject.moveObject.GetComponent<Heros>()) && 
        (!onlyBuilding || DragObject.moveObject.GetComponent<Building>()))
        {
            potencialUnit = DragObject.moveObject;
            potencialUnit.GetComponent<DragObject>().potencialPole = this.gameObject;

        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }
}
