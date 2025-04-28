using UnityEngine;

public class WebGLPortraitEmulator : MonoBehaviour
{
    [Header("Resolution Settings")]
    public Color sideColor = new Color32(0xFF, 0xEB, 0x9D, 0xFF);
    private Texture2D sideTexture;

    [Header("Shadow Settings")]
    [SerializeField] private Color shadowColor = new Color(0, 0, 0, 0.5f); // Culoare inițială umbră
    [SerializeField][Range(4, 32)] private int shadowWidth = 16; // Lățime umbră (pixeli)
    [SerializeField][Range(0.1f, 1f)] private float shadowOpacity = 0.5f; // Opacitate umbră
    [SerializeField][Range(4, 64)] private int shadowGradientLength = 16; // Lungime gradient umbră

    private Texture2D shadowTextureLeft; // Textură pentru umbra din stânga
    private Texture2D shadowTextureRight; // Textură pentru umbra din dreapta

    private int portraitWidth = 720;
    private int portraitHeight = 1280;

    private void Start()
    {
        SetupResolution();
        CreateSideTexture();
        CreateShadowTextures();
    }

    private void SetupResolution()
    {
        // Calculate portrait height based on the actual screen ratio
        float targetAspect = (float)portraitWidth / portraitHeight;
        float currentAspect = (float)Screen.width / Screen.height;

        if (currentAspect > targetAspect)
        {
            int adjustedWidth = Mathf.RoundToInt(Screen.height * targetAspect);
            int blackBarWidth = (Screen.width - adjustedWidth) / 2;
            Camera.main.rect = new Rect((float)blackBarWidth / Screen.width, 0, (float)adjustedWidth / Screen.width, 1);
        }
        else
        {
            Camera.main.rect = new Rect(0, 0, 1, 1);
        }
    }

    private void CreateSideTexture()
    {
        sideTexture = new Texture2D(1, 1);
        sideTexture.SetPixel(0, 0, sideColor);
        sideTexture.Apply();
    }

    private void CreateShadowTextures()
    {
        // Textură pentru umbra din dreapta (gradient de la intens la transparent, spre dreapta)
        shadowTextureRight = new Texture2D(shadowGradientLength, 1);
        Color startColor = new Color(shadowColor.r, shadowColor.g, shadowColor.b, shadowOpacity);
        Color endColor = new Color(shadowColor.r, shadowColor.g, shadowColor.b, 0);
        for (int x = 0; x < shadowTextureRight.width; x++)
        {
            float t = (float)x / (shadowTextureRight.width - 1);
            shadowTextureRight.SetPixel(x, 0, Color.Lerp(startColor, endColor, t));
        }
        shadowTextureRight.Apply();

        // Textură pentru umbra din stânga (gradient de la transparent la intens, spre stânga)
        shadowTextureLeft = new Texture2D(shadowGradientLength, 1);
        for (int x = 0; x < shadowTextureLeft.width; x++)
        {
            float t = (float)x / (shadowTextureLeft.width - 1);
            shadowTextureLeft.SetPixel(x, 0, Color.Lerp(endColor, startColor, t)); // Inversăm gradientul
        }
        shadowTextureLeft.Apply();
    }

    private void OnGUI()
    {
        float targetRatio = (float)portraitWidth / portraitHeight;
        float currentRatio = (float)Screen.width / Screen.height;

        if (currentRatio > targetRatio)
        {
            int adjustedWidth = Mathf.RoundToInt(Screen.height * targetRatio);
            int barWidth = (Screen.width - adjustedWidth) / 2;

            // Desenează barele laterale
            GUI.DrawTexture(new Rect(0, 0, barWidth, Screen.height), sideTexture);
            GUI.DrawTexture(new Rect(Screen.width - barWidth, 0, barWidth, Screen.height), sideTexture);

            // Desenează umbra la marginea stângă (gradient orientat spre stânga)
            GUI.DrawTexture(new Rect(barWidth - shadowWidth, 0, shadowWidth, Screen.height), shadowTextureLeft, ScaleMode.StretchToFill, true);
            // Desenează umbra la marginea dreaptă (gradient orientat spre dreapta)
            GUI.DrawTexture(new Rect(Screen.width - barWidth, 0, shadowWidth, Screen.height), shadowTextureRight, ScaleMode.StretchToFill, true);
        }
    }

    private void Update()
    {
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            SetupResolution();
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
        }
    }

    private int lastScreenWidth = 0;
    private int lastScreenHeight = 0;
}