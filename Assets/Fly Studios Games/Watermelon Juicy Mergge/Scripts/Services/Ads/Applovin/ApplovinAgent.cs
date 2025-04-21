using Analytics;
using System;

namespace ADS
{
    public class ApplovinAgent
    {
        private const string SDK_KEY = "U87DAmAPeHWaGi5E0xG1B-0L5mXTcq1u7nmr6GhGK04qrWVLmAAD6spQ_D1c8JKKyRHq03xQhHAli5vP2WuTFB";

        private ApplovinBanner _banner;
        private ApplovinInterstitial _interstitial;
        private ApplovinRewarded _rewarded;

        public bool IsInterstitialReady => _interstitial.IsReady;
        public bool IsRewardedReady => _rewarded.IsReady;

        public ApplovinAgent(Action onInitialized)
        {
            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
            {
                SubscribeToRevenueEvents();

                _banner = new ApplovinBanner();
                _interstitial = new ApplovinInterstitial(this);
                _rewarded = new ApplovinRewarded();

                onInitialized?.Invoke();
            };

            MaxSdk.SetSdkKey(SDK_KEY);
            MaxSdk.InitializeSdk();
        }

        public void ShowInterstitial(AdsManager.CallbackStruct callbacks)
        {
            _interstitial.Show(callbacks);
        }

        public void ShowRewarded(AdsManager.CallbackStruct callbacks)
        {
            _rewarded.Show(callbacks);
        }

        public void ShowBanner()
        {
            _banner.Show();
        }

        public void HideBanner(bool destroy = false)
        {
            _banner.Hide();

            if (destroy)
                _banner.Destroy();
        }

        #region Custom Callbacks

        public event Action InterstitialLoaded;

        public void OnInterstitialLoaded()
        {
            InterstitialLoaded?.Invoke();
        }

        #endregion Callbacks

        #region Revenue events

        private void SubscribeToRevenueEvents()
        {
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += UpdateECPM;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += UpdateECPM;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        }

        private void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            double revenue = adInfo.Revenue;

            string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
            string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
            string format = adInfo.AdFormat;

            AnalyticsEvents.LogAdRevenue(adUnitIdentifier, "Applovin", networkName, format, revenue, "USD");
        }

        private void UpdateECPM(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            string type = adInfo.AdFormat;
            double revenue = adInfo.Revenue;

            double total = GameStatistics.IncTotalRevenue(type, revenue);
            int watched = GameStatistics.IncAdsWatched(type);

            AnalyticsEvents.LogRewardedECPM(type, total, watched);
        }

        #endregion Revenue events
    }
}
