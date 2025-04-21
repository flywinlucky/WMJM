using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UnityEngine.UI.Button))]

public class StaticButtonEvent : MonoBehaviour
{
    public UnityEvent OnClick;
    private ADS.AdsManager ads;

    // Start is called before the first frame update
    void Start()
    {
        ads = ADS.AdsManager.Instance;

        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenOrCloseUIPanels);
    }

    private void OpenOrCloseUIPanels()
    {
        ads.DestroyBanner();
    }
}