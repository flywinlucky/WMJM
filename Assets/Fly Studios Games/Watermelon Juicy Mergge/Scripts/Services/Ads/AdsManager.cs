using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ADS
{
    public class AdsManager : MonoBehaviour
    {
        public static AdsManager Instance;

        [SerializeField] private AdsUI _adsUI;
        [SerializeField] private GameObject _bannerRemoveAdsButton;
        [SerializeField] private GameObject _bannerGrassObject;

        private ApplovinAgent _applovinAgent;
        private AdmobAgent _admobAgent;

        private float _interCycleStart;
        private bool _canShowBanner = true;

        private bool NoAdsPurchased
        {
            get => PurchaseManager.Instance.GetProductPurchased(EProduct.RemoveAds);
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            SetActiveRemoveAdsButton(false);
        }

        private void Start()
        {
            _admobAgent = new(() =>
            {
                Debug.Log("[AdsManager] Admob agent initialized");
            });

            _applovinAgent = new(() =>
            {
                Debug.Log("[AdsManager] Applovin agent initialized");

                RemoteConfigManager.Instance.OnInitialized(() =>
                {
                    if (NoAdsPurchased)
                    {
                        DestroyBanner();
                        return;
                    }

                    InitializeBanner();
                    InitializeInterstitial();
                });
            });
        }

        #region Banner ads

        private void InitializeBanner()
        {
            float bannerDelay = 10f;
            DOVirtual.DelayedCall(bannerDelay, () =>
            {
                // if banner is disabled by a native ad, don't show
                if (!_canShowBanner)
                    return;

                SetBannerActive(true);
            });
        }

        public void SetBannerActive(bool bannerActive, bool grassActive = true)
        {
            _canShowBanner = bannerActive;
            bool noAdsNotPurchased = !NoAdsPurchased;

            if (bannerActive && noAdsNotPurchased)
                _applovinAgent.ShowBanner();
            else
                _applovinAgent.HideBanner(false);

            SetActiveRemoveAdsButton(bannerActive && noAdsNotPurchased);
            _bannerGrassObject.SetActive(grassActive);
        }

        public void DestroyBanner()
        {
            _canShowBanner = false;
            _applovinAgent.HideBanner(true);
            SetActiveRemoveAdsButton(false);
        }

        #endregion

        #region Interstitial ads

        #region Persistent interstitial cycle

        private void InitializeInterstitial()
        {
            float firstInterDelay = RemoteConfigManager.Instance.Get<float>("inter_cycle_start_delay", 60f);
            Debug.Log($"[AdsManager] start inter cycle in {firstInterDelay} seconds");

            _interCycleStart = Time.time;
            DOVirtual.DelayedCall(firstInterDelay, StartInterstitialCycle);
        }

        private void StartInterstitialCycle()
        {
            CancelInvoke(nameof(StartInterstitialCycle));
            float delay = RemoteConfigManager.Instance.Get<float>("interstitial_show_rate", 30f);

            TryShowInterstitial(
                GetCycleInterPlacement(),
                () => Invoke(nameof(StartInterstitialCycle), 5f),
                () => Invoke(nameof(StartInterstitialCycle), delay));
        }

        private void TryShowInterstitial(string placement, Action notReadyCallback, Action completedCallback)
        {
            bool suitableEnvironment = PopUpActiveChecker.PopupsActive < 1 && SceneManager.GetActiveScene().buildIndex != 0;
            bool canShow = _applovinAgent.IsInterstitialReady && !NoAdsPurchased;

            if (!canShow || !suitableEnvironment)
            {
                notReadyCallback?.Invoke();
                return;
            }

            _adsUI.ShowAlert(() => ShowInter(placement, completedCallback));
        }

        #endregion

        public void ShowInterstitialNoAlert(string placement, Action notReadyCallback, Action completedCallback)
        {
            if (!_applovinAgent.IsInterstitialReady)
            {
                notReadyCallback?.Invoke();
                return;
            }

            ShowInter(placement, completedCallback);
        }

        private void ShowInter(string placement, Action completedCallback)
        {
            var callbacks = new CallbackStruct()
            {
                onDisplayed = () => OnVideoAdsShown("Interstitial", placement),
                onHidden = completedCallback
            };

            _applovinAgent.ShowInterstitial(callbacks);
        }

        #endregion

        #region Rewarded ads

        public void ShowRewarded(string placement, Action notReadyCallback, Action completedCallback)
        {
            bool isLoaded = _applovinAgent.IsRewardedReady;

            RewardUtils.TryShow(ShowNoRewarded, isLoaded);

            if (!isLoaded)
            {
                notReadyCallback?.Invoke();
                return;
            }

            var callbacks = new CallbackStruct()
            {
                onDisplayed = () => OnVideoAdsShown("Rewarded", placement),
                onHidden = null,
                onCompleted = () =>
                {
                    completedCallback?.Invoke();

                    RewardUtils.TryShowAfter(() =>
                    {
                        if (_applovinAgent.IsInterstitialReady)
                            _adsUI.ShowAlert(() => ShowInter("inter_after_reward", null));
                    });
                }
            };
            _applovinAgent.ShowRewarded(callbacks);
        }

        /// <summary>
        /// inter_start_2 thingy
        /// </summary>
        private void ShowNoRewarded()
        {
            if (!_applovinAgent.IsInterstitialReady)
            {
                _applovinAgent.InterstitialLoaded += Show;
                return;
            }

            Show();

            void Show()
            {
                _applovinAgent.InterstitialLoaded -= Show;
                _adsUI.ShowAlert(() => ShowInter("inter_start_2", null));
            }
        }

        #endregion

        #region Native ads

        public bool InitNative(AdmobNativeController controller)
        {
            if (NoAdsPurchased)
                return false;

            return _admobAgent.ShowNative(controller);
        }

        #endregion

        #region Ad callbacks related

        public struct CallbackStruct
        {
            public Action onDisplayed;
            public Action onHidden;
            public Action onCompleted;
        }

        private void OnVideoAdsShown(string type, string placement)
        {
            Analytics.AnalyticsEvents.LogVideoAdsStarted(type, placement, "Applovin");
        }

        private string GetCycleInterPlacement()
        {
            float min = (Time.time - _interCycleStart) / 60f;
            string placement = "inter_min";

            if (min < 2)
            {
                return placement + "1-2";
            }
            if (min < 5)
            {
                return placement + "2-5";
            }
            if (min < 10)
            {
                return placement + "5-10";
            }
            if (min < 20)
            {
                return placement + "10-20";
            }

            return placement + "20+";
        }

        #endregion

        #region Remove ads

        private void SetActiveRemoveAdsButton(bool active)
        {
            if (_bannerRemoveAdsButton)
                _bannerRemoveAdsButton.SetActive(active);
        }

        #endregion
    }
}
