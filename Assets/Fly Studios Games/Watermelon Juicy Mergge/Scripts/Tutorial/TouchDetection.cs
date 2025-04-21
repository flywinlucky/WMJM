using System;
using UnityEngine;

public class TouchDetection : MonoBehaviour
{
    public event Action OnTouchDetected;

    void Update()
    {
        if (IsTouchingScreen() || IsClickingMouse())
        {
            OnTouchDetected?.Invoke();
        }
    }

    private bool IsTouchingScreen()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    private bool IsClickingMouse()
    {
        return Input.GetMouseButtonDown(0);
    }
}
