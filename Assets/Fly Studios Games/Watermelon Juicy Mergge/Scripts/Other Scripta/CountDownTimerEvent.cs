using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CountDownTimerEvent : MonoBehaviour
{
    public float timeRemaining;
    public float dublicateTimeRemaining;

    public bool timerIsRunning;
  
    [Header("Events")]
    [Space]
    public UnityEvent OnTimerRunOut;

    private void Start()
    {
        dublicateTimeRemaining = timeRemaining;
    }
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                OnTimerRunOut.Invoke();
                Debug.Log("Time hash ben run");

                timeRemaining = dublicateTimeRemaining;
            }
        }
    }
}