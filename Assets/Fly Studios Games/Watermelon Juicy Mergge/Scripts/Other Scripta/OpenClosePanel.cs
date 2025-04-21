using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class OpenClosePanel : MonoBehaviour
{
    public GameObject[] UIpanel;

    public GameObject[] openPanels;
    public GameObject[] closePanels;

    [Space]
    public AudioSource audioSource;

    [Space]
    public UnityEvent OnClick;


    private void Awake()
    {
        if (audioSource)
            audioSource.playOnAwake = false;

        GetComponent<Button>().onClick.AddListener(OpenOrCloseUIPanels);
    }

    private void OpenOrCloseUIPanels()
    {
        if (audioSource)
        {
            audioSource.Play();

        }
        foreach (GameObject panel in UIpanel)
        {
            panel.SetActive(!panel.activeSelf);
            OnClick?.Invoke();
        }

        CloseUIPanels(openPanels, true);
        CloseUIPanels(closePanels, false);
    }

    private void CloseUIPanels(GameObject[] gameObjects, bool state)
    {
        if (audioSource)
        {
            audioSource.Play();
        }

        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(state);
        }
    }
}