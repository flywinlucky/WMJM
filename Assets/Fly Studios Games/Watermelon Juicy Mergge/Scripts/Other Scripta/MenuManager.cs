using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject continueButton;

    private void Start()
    {
        if (PlayerPrefs.HasKey("restoreSpheresData"))
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }

    public void NewGame()
    {
        Debug.Log("Start New Game");
        PlayerPrefs.SetInt("newGamevalue", 1);

        GameStatistics.GameStart();
        Analytics.AnalyticsEvents.LogGameStart();
    }
}