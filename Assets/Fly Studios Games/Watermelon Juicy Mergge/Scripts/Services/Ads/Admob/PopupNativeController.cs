using UnityEngine;

public class PopupNativeController : MonoBehaviour
{
    [SerializeField] private AdmobNativeController _controller;

    private void OnEnable()
    {
        Debug.Log("[NativeController] PopupNative OnEnable");

        _controller.gameObject.SetActive(false);

        if (!RemoteConfigManager.Instance.Get<bool>("use_popup_native_ad", false))
        {
            Destroy(gameObject);
            return;
        }

        ADS.AdsManager.Instance.InitNative(_controller);
    }
}
