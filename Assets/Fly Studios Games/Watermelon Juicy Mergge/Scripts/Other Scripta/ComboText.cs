using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ComboText : MonoBehaviour
{
    public Text comboText;
    public new ParticleSystem particleSystem;

    private void OnEnable()
    {
        //comboText = GetComponent<Text>();
        //particleSystem = GetComponent<ParticleSystem>();
    }

    [System.Obsolete]
    public void ComboTextUI(string textMessage, Color textColor) // , Color textColor
    {
        if (comboText)
        {
            comboText.text = textMessage.ToString();
            comboText.color = textColor;
            particleSystem.startColor = textColor;
            particleSystem.Play();
        }
    }
}