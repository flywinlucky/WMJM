using UnityEngine;
using UnityEngine.UI;

public class ComboText : MonoBehaviour
{
    public Text comboText;
    public ParticleSystem comboParticles; // redenumit pentru a evita conflictul

    public void ComboTextUI(string textMessage, Color textColor)
    {
        if (comboText)
        {
            comboText.text = textMessage;
            comboText.color = textColor;

            if (comboParticles)
            {
                var main = comboParticles.main;
                main.startColor = textColor;
                comboParticles.Play();
            }
        }
    }
}
