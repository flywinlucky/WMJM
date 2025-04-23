using UnityEngine;
using YG;
using TMPro;
using System.Collections;

public class InterstitialYG : MonoBehaviour
{
    public float countdownTimer; // Durata countdown-ului în secunde
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

        yield return new WaitForSeconds(1f);

        // Afișează reclama interstitială
        Debug.Log("Showing interstitial ad");
        YG2.InterstitialAdvShow();

        // Resetează timer-ul și starea
        countdownTimer = 65f;
        isShowingAd = false;
    }
}