using UnityEngine;

public class GameSceneNativeController : MonoBehaviour
{
    [SerializeField] private AdmobNativeController _controller;

    private void Awake()
    {
        _controller.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (!RemoteConfigManager.Instance.Get<bool>("use_game_scene_native_ad", false))
        {
            Destroy(gameObject);
            return;
        }

        float delay = RemoteConfigManager.Instance.Get<float>("game_scene_native_appear_delay", 35f);
        Debug.Log($"[NativeController] Show game scene in {delay} seconds");
        ScheduleCall(nameof(Initialize), delay);
    }   

    private void ScheduleCall(string methodName, float delay)
    {
        CancelInvoke(methodName);
        Invoke(methodName, delay);
    }

    public void Initialize()
    {
        if (!ADS.AdsManager.Instance.InitNative(_controller))
        {
            Debug.Log($"[NativeController] Could not show game scene native, schedule another call");
            Repeat();
        }
    }

    public void OnCloseButtonClicked()
    {
        _controller.gameObject.SetActive(false);

        Repeat();
    }

    private void Repeat()
    {
        float delay = RemoteConfigManager.Instance.Get<float>("game_scene_native_repeat_delay", 20f);
        Debug.Log($"[NativeController] Show game scene in {delay} seconds");
        ScheduleCall(nameof(Initialize), delay);
    }
}
