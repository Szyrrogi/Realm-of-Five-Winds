using UnityEngine;

public class FixedAspectRatio : MonoBehaviour
{
    public float targetAspect = 16f / 9f;

    void Start()
{
    Camera camera = Camera.main;


    if (camera == null)
    {
        Debug.LogError("FixedAspectRatio: Brak komponentu Camera!");
        return;
    }

    float windowAspect = (float)Screen.width / (float)Screen.height;
    float scaleHeight = windowAspect / targetAspect;

    if (scaleHeight < 1f)
    {
        // Letterbox
        Rect rect = camera.rect;
        rect.width = 1f;
        rect.height = scaleHeight;
        rect.x = 0;
        rect.y = (1f - scaleHeight) / 2f;
        camera.rect = rect;
    }
    else
    {
        // Pillarbox
        float scaleWidth = 1f / scaleHeight;
        Rect rect = camera.rect;
        rect.width = scaleWidth;
        rect.height = 1f;
        rect.x = (1f - scaleWidth) / 2f;
        rect.y = 0;
        camera.rect = rect;
    }
}

}
