using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResetPlayerPrefs : MonoBehaviour
{
    [MenuItem("Tools/Reset Player Prefs")]
    public static void ResetAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("** Player Prefs Reseted **");
    }
}