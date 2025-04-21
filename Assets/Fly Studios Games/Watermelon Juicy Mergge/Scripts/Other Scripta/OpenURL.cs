using UnityEngine;

public class OpenURL : MonoBehaviour
{
    public string url;
    public void OpenWebURL()
    {
        Application.OpenURL(url);
    }
}