using UnityEngine;

public class SocialMedia : MonoBehaviour
{
    [SerializeField] private string url; // Wklej link w Inspectorze

    public void OpenLink()
    {
        if (!string.IsNullOrEmpty(url))
        {
            Application.OpenURL(url);
        }
    }
}