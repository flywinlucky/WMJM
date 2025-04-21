using DG.Tweening;
using System;
using UnityEngine;

namespace ADS
{
    public class ApplovinInterstitial
    {
        private const string UNIT_ID = "87d4a748dfe53c91";

        private int _retryAttempt;

        private Action _onLoaded;
        private Action _onDisplayed;
        private Action _onHidden;

        public bool IsReady
        {
            get => MaxSdk.IsInterstitialReady(UNIT_ID);
        }

        public ApplovinInterstitial(ApplovinAgent agent)
        {
            // Attach callback
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

            _onLoaded += agent.OnInterstitialLoaded;

            LoadInterstitial();

            Debug.Log("Interstitial initialized");
        }

        public void Show(AdsManager.CallbackStruct callbacks)
        {
            _onDisplayed = callbacks.onDisplayed;
            _onHidden = callbacks.onHidden;

            MaxSdk.ShowInterstitial(UNIT_ID);
        }

        private void LoadInterstitial()
        {
            MaxSdk.LoadInterstitial(UNIT_ID);

            Debug.Log("Load Interstitial");
        }

        #region Callbacks

        private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'
            _onLoaded?.Invoke();

            // Reset retry attempt
            _retryAttempt = 0;

            Debug.Log("Interstitial loaded event");
        }

        private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Interstitial ad failed to load 
            // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)
            Debug.Log("Interstitial failed to load");

            _retryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, _retryAttempt));

            DOVirtual.DelayedCall((float)retryDelay, LoadInterstitial);
        }

        private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            _onDisplayed?.Invoke();
            _onDisplayed = null;
        }

        private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
            LoadInterstitial();
        }

        private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

        private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is hidden. Pre-load the next ad.
            LoadInterstitial();

            _onHidden?.Invoke();
            _onHidden = null;
        }

        #endregion
    }

}
