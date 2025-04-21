using DG.Tweening;
using GoogleMobileAds.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AdmobAgent
{
    //private const string UNIT_ID = "ca-app-pub-3940256099942544/2247696110"; // test id
    private const string UNIT_ID = "ca-app-pub-6605341596800051/1812368363";

    private NativeAd _nativeAd;
    private bool _nativeAdLoaded = false;
    private bool _nativeAdDisplaying = false;

    private float _retryAttempt = 0;

    public AdmobAgent(Action onInitialized)
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            Debug.Log($"Google Ads initialized");

            LoadNativeAd();

            onInitialized?.Invoke();
        });
    }

    public bool ShowNative(AdmobNativeController controller)
    {
        if (!_nativeAdLoaded)
        {
            Debug.Log("[GoogleMobileAds] ShowNative: native ad is not loaded!");
            return false;
        }

        if (_nativeAdDisplaying)
        {
            Debug.Log("[GoogleMobileAds] ShowNative: a native ad is already displayed!");
            return false;
        }

        _nativeAdDisplaying = true;

        Texture2D iconTexture = _nativeAd.GetIconTexture();
        Texture2D adChoicesLogoTexture = _nativeAd.GetAdChoicesLogoTexture();
        List<Texture2D> imageTexture = _nativeAd.GetImageTextures();
        string headline = _nativeAd.GetHeadlineText();
        string callToAction = _nativeAd.GetCallToActionText();
        string advertiser = _nativeAd.GetAdvertiserText();
        string store = _nativeAd.GetStore();

        controller.NativeAd = _nativeAd;

        controller.SetIcon(iconTexture);
        controller.SetAdChoicesLogo(adChoicesLogoTexture);
        controller.SetMainImage(imageTexture[0]);
        controller.SetHeadline(headline);
        controller.SetCallToAction(callToAction);

        string[] titles = { advertiser, store, headline };
        controller.SetAdvertiser(titles.FirstOrDefault(t => !string.IsNullOrEmpty(t)));

        var ads = ADS.AdsManager.Instance;
        // hide the banner to avoid overlapping
        ads.SetBannerActive(false, false);
        // show the banner again when the controller is disabled
        controller.OnDisabled += () =>
        {
            ads.SetBannerActive(true);
            _nativeAdDisplaying = false;
        };

        controller.gameObject.SetActive(true);

        //string body = _nativeAd.GetBodyText();
        //string price = _nativeAd.GetPrice();
        //double starRating = _nativeAd.GetStarRating();

        LoadNativeAd();

        return true;
    }

    private void LoadNativeAd()
    {
        Debug.Log("[GoogleMobileAds] Load Native ad");
        _nativeAdLoaded = false;

        AdLoader adLoader = new AdLoader.Builder(UNIT_ID)
            .ForNativeAd()
            .Build();

        adLoader.OnNativeAdLoaded += HandleNativeAdLoaded;
        adLoader.OnAdFailedToLoad += HandleNativeAdFailedToLoad;

        adLoader.LoadAd(new AdRequest());
    }

    private void HandleNativeAdLoaded(object sender, NativeAdEventArgs args)
    {
        Debug.Log("[GoogleMobileAds] Native ad loaded");
        _nativeAdLoaded = true;
        _nativeAd = args.nativeAd;

        _retryAttempt = 0;
    }

    private void HandleNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("[GoogleMobileAds] Native ad failed to load: " + args.LoadAdError.GetMessage());
        _nativeAdLoaded = false;

        _retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, _retryAttempt));
        DOVirtual.DelayedCall((float)retryDelay, LoadNativeAd);
    }
}
