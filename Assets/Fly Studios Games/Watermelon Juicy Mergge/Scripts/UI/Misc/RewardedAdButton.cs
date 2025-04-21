using System;
using UnityEngine;
using UnityEngine.Events;

public class RewardedAdButton : MonoBehaviour
{
    [SerializeField] private UnityEvent _onCompleted;

    public void ButtonClicked()
    {
        string placement = "rewarded_settings_clear_small_itmes";

        Action notReadyCallback = () => Debug.Log("[SettingsPopup] No ads available");
        Action completedCallback = () => _onCompleted?.Invoke();

 
    }
}
