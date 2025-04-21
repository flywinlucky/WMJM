using System;
using UnityEngine;

public class PopUpActiveChecker : MonoBehaviour
{
    public static event Action OnEnabled;
    public static event Action OnDisabled;

    public static int PopupsActive;

    private void OnEnable()
    {
        PopupsActive++;
        OnEnabled?.Invoke();
    }

    private void OnDisable()
    {
        PopupsActive--;
        OnDisabled?.Invoke();
    }
}