using TMPro;
using UnityEngine;

public class FxText : MonoBehaviour
{
    public TMP_Text text;
    public void CFXRCustomText(int customText, Color color)
    {
        // Modificăm direct variabila text cu valoarea customText convertită la șir de caractere
        text.text = "+" + customText.ToString();
        text.color = color;
    }
}