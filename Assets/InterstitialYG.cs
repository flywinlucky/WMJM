using System.Collections;
using UnityEngine;
using YG;

public class InterstitialYG : MonoBehaviour
{
    private bool isGameReady;
    public float countdownTimer; // Durata countdown-ului în secunde
    public bool enableInterstitials = true; // Starea countdown-ului
    private bool isShowingAd = false; // Flag pentru a preveni apeluri multiple

    private void Start()
    {
        YG2.SwitchLanguage("en");
        string lang = YG2.envir.language;
        Debug.Log("Detected language: " + lang);

        StartCoroutine(WaitAndSignalReady());
    }

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

    private IEnumerator WaitAndSignalReady()
    {
        // Simulează un delay de pregătire sau așteaptă o scenă, UI etc.
        yield return new WaitForSeconds(2f); // sau WaitUntil(...)

        YG2.GameReadyAPI();
        Debug.Log("✅ GameReady() called — game is now ready for interaction.");
        isGameReady = true;
    }
}