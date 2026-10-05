using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ChoseRegion : MonoBehaviour
{
    [Range(0, 1)]
    public float alphaThreshold = 0.1f;

    void Awake()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = alphaThreshold;
    }
    public void OnClickEneter()
    {
        Debug.Log(gameObject.name);
    }
}
