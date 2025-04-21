using UnityEngine;
using YG;
using TMPro;
using System.Collections;

public class InterstitialYG : MonoBehaviour
{
    public TMP_Text countdownInterText; // Textul pentru countdown
    public GameObject interstitialAdAlert; // Obiectul pentru reclama interstitială  

    public float countdownTimer = 65f; // Durata countdown-ului în secunde
    public bool enableInterstitials = true; // Starea countdown-ului
    private bool isShowingAd = false; // Flag pentru a preveni apeluri multiple

    void Update()
    {
        // Actualizează countdown-ul automat dacă este activ
        if (enableInterstitials && !isShowingAd)
        {
            countdownTimer -= Time.deltaTime;

            // Când countdown-ul expiră
            if (countdownTimer <= 0f)
            {
                StartCoroutine(ShowInterstitialWithCountdown());
            }
        }
    }

    IEnumerator ShowInterstitialWithCountdown()
    {
        if (isShowingAd) yield break;
        isShowingAd = true;

        // Activează UI-ul pentru countdown
        interstitialAdAlert.SetActive(true);
        countdownInterText.gameObject.SetActive(true);

        // Numărătoare inversă simplă 3, 2, 1
        countdownInterText.text = "3";
        yield return new WaitForSeconds(1f);

        countdownInterText.text = "2";
        yield return new WaitForSeconds(1f);

        countdownInterText.text = "1";
        yield return new WaitForSeconds(1f);

        // Ascunde UI-ul
        interstitialAdAlert.SetActive(false);
        countdownInterText.gameObject.SetActive(false);

        // Afișează reclama interstitială
        Debug.Log("Showing interstitial ad after countdown");
        YG2.InterstitialAdvShow();

        // Resetează timer-ul și starea
        countdownTimer = 65f;
        isShowingAd = false;
    }
}