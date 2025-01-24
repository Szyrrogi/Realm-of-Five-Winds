using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static GameObject eventSystem;
    
    private void Awake()
    {
        eventSystem = this.gameObject;
    }
}
