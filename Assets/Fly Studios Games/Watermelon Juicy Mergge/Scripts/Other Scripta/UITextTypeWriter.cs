using System.Collections;
using TMPro;
using UnityEngine;

public class UITextTypeWriter : MonoBehaviour
{
    public bool initializeOnEnable;
    public float tipingInterval;

    TMP_Text txt;
    string story;

    private void OnEnable()
    {
        if (initializeOnEnable)
        {
            txt = GetComponent<TMP_Text>();
            story = txt.text;
            txt.text = "";

            // TODO: add optional delay when to start
            StartCoroutine(PlayText());
        }
    }

    IEnumerator PlayText()
    {
        foreach (char c in story)
        {
            txt.text += c;
            yield return new WaitForSeconds(tipingInterval); // 0.125f
        }
    }
}