using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BohaterInGame : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public void Show(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
