using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UnityEngine.UI.Button))]

public class StaticButtonEvent : MonoBehaviour
{
    public UnityEvent OnClick;
 

    // Start is called before the first frame update
    void Start()
    {
 
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenOrCloseUIPanels);
    }

    private void OpenOrCloseUIPanels()
    {
  
    }
}