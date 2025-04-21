using System;
using UnityEngine;
using TMPro;
using System.Collections;

namespace ADS
{
    public class AdsUI : MonoBehaviour
    {
        [Header("Sponsor message popup")]
        [SerializeField] private GameObject _default;
        [SerializeField] private TMP_Text _defaultCounter;

        [Header("Remove ads popup")]
        [SerializeField] private GameObject _offer;

        private int _timesShown = 0;
        private Action _adCall;

        public void ShowAlert(Action adCall)
        {
            _timesShown++;
            _adCall = adCall;

            int offerInterval = RemoteConfigManager.Instance.Get<int>("inter_popup_removeads_offer_interval", 3);

            if (offerInterval > 0 && _timesShown % offerInterval == 0)
            {
                _offer.SetActive(true);
            }
            else
            {
                _default.SetActive(true);
                StartCoroutine(DefaultAlertCountdown(2));
            }
        }

        private IEnumerator DefaultAlertCountdown(int seconds)
        {
            while (seconds > 0)
            {
                _defaultCounter.text = $"0:0{seconds}";
                yield return new WaitForSeconds(1);
                seconds--;
            }

            _adCall?.Invoke();
            _adCall = null;

            _default.SetActive(false);
        }

        public void OfferAcceptClicked()
        {
            Debug.Log("Ads UI -> offer accept clicked");

            PurchaseManager.Instance.BeginPurchase(EProduct.RemoveAds, () =>
            {
                Debug.Log($"[AdsUI] BeginPurchase onFailedCallback: {_adCall != null}");
                _adCall?.Invoke();
                _adCall = null;
            });

            _offer.SetActive(false);
        }

        public void OfferDeclineClicked()
        {
            Debug.Log("Ads UI -> offer decline clicked");

            // show ad
            _adCall?.Invoke();
            _adCall = null;

            _offer.SetActive(false);
        }
    }
}
