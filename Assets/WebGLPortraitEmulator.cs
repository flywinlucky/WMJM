using UnityEngine;

public class WebGLPortraitEmulator : MonoBehaviour
{
    public Color sideColor = new Color32(0xFF, 0xEB, 0x9D, 0xFF);
    private Texture2D sideTexture;

    private int portraitWidth = 720;
    private int portraitHeight = 1280;

    private void Start()
    {
        SetupResolution();
        CreateSideTexture();
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

    private void OnGUI()
    {
        float targetRatio = (float)portraitWidth / portraitHeight;
        float currentRatio = (float)Screen.width / Screen.height;

        if (currentRatio > targetRatio)
        {
            int adjustedWidth = Mathf.RoundToInt(Screen.height * targetRatio);
            int barWidth = (Screen.width - adjustedWidth) / 2;
            // Draw left bar
            GUI.DrawTexture(new Rect(0, 0, barWidth, Screen.height), sideTexture);
            // Draw right bar
            GUI.DrawTexture(new Rect(Screen.width - barWidth, 0, barWidth, Screen.height), sideTexture);
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
