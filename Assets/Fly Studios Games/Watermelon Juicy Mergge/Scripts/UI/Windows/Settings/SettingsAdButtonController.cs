using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsAdButtonController : MonoBehaviour
{
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Color _rewardedColor;
    [SerializeField] private Color _interstitialColor;

    private bool _showRewarded = true;

    private void OnEnable()
    {
        ResetState();
    }

    private void ResetState()
    {
        if (!_showRewarded)
            SwapAdType();
    }

    public void SwapAdType()
    {
        _showRewarded = !_showRewarded;
        _buttonImage.color = _showRewarded ? _rewardedColor : _interstitialColor;
    }

    public void AdButtonClick()
    {
        string placement = "rewarded_settings_clear_small_itmes";

        Action notReadyCallback = () => Debug.Log("[SettingsPopup] No ads available");
        Action completedCallback = () =>
        {
            gameObject.SetActive(false);
            WatermelonGameClone.GameManager.Instance.ClearSmallItems();
        };

        if (_showRewarded)
            ADS.AdsManager.Instance.ShowRewarded(placement, notReadyCallback, completedCallback);
        else
            ADS.AdsManager.Instance.ShowInterstitialNoAlert(placement, notReadyCallback, completedCallback);
    }
}
