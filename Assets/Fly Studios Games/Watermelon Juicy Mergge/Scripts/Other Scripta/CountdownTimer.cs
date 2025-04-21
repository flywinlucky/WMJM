using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float timeRemaining;
    public bool timerAutoRunning = false;
    public TMP_Text timeText;

    void Update()
    {
        if (timerAutoRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerAutoRunning = false;
            }
        }
    }

    public void SetTimmerValue(float time)
    {
        timeRemaining = time;
        timerAutoRunning = true;
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float seconds = Mathf.FloorToInt(timeToDisplay);

        timeText.text = string.Format("{0:00}", seconds);
    }
}