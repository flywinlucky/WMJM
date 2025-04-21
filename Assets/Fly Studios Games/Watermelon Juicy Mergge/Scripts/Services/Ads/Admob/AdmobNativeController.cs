using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GoogleMobileAds.Api;
using System;

public class AdmobNativeController : MonoBehaviour
{
    [SerializeField] private TMP_Text AdvertiserText;
    [SerializeField] private TMP_Text HeadlineText;
    [SerializeField] private TMP_Text CallToActionText;
    [SerializeField] private RawImage AdChoicesLogo;
    [SerializeField] private RawImage Icon;
    [SerializeField] private RawImage MainImage;

    public NativeAd NativeAd;

    public event Action OnDisabled;

    private void OnDisable()
    {
        OnDisabled?.Invoke();
        OnDisabled = null;
    }

    public void SetAdvertiser(string advertiserText)
    {
        if (AdvertiserText != null)
        {
            AdvertiserText.text = advertiserText;
            if (!NativeAd.RegisterAdvertiserTextGameObject(AdvertiserText.gameObject))
            {
                AdvertiserText.gameObject.SetActive(false);
            }
        }
    }

    public void SetHeadline(string headlineText)
    {
        if (HeadlineText != null)
        {
            HeadlineText.text = headlineText;
            NativeAd.RegisterHeadlineTextGameObject(HeadlineText.gameObject);
        }
    }

    public void SetCallToAction(string callToActionText)
    {
        if (CallToActionText != null)
        {
            CallToActionText.text = callToActionText;
            NativeAd.RegisterCallToActionGameObject(CallToActionText.gameObject);
        }
    }

    public void SetIcon(Texture2D icon)
    {
        if (Icon != null)
        {
            Icon.texture = icon;
            NativeAd.RegisterIconImageGameObject(Icon.gameObject);
        }
    }

    public void SetMainImage(Texture2D image)
    {
        if (MainImage != null)
        {
            MainImage.texture = image;
            NativeAd.RegisterImageGameObjects(new() { MainImage.gameObject });
        }
    }

    public void SetAdChoicesLogo(Texture2D adChoicesLogo)
    {
        if (AdChoicesLogo != null)
        {
            AdChoicesLogo.texture = adChoicesLogo;
            if (!NativeAd.RegisterAdChoicesLogoGameObject(AdChoicesLogo.gameObject))
            {
                Debug.LogWarning("[NativeController] Could not register ad choices logo game object!");
            }
        }
    }
}
