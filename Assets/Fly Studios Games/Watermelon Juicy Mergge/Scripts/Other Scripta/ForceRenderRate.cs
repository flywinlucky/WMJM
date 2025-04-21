using System.Collections;
using System.Threading;
using UnityEngine;
using Sirenix.OdinInspector;

public class ForceRenderRate : MonoBehaviour
{
    [Header("Enable Force Rate")]
    [Space]
    public bool enableForceRate;
    [Space]
    [Range(0f, 60f)]
    [ShowIf("enableForceRate")]
    public float rate;
    float currentFrameTime;


    private void Awake()
    {
        if (enableForceRate)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 9999;
            currentFrameTime = Time.realtimeSinceStartup;
            StartCoroutine("WaitForNextFrame");
        }
    }

    IEnumerator WaitForNextFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            currentFrameTime += 1.0f / rate;
            var t = Time.realtimeSinceStartup;
            var sleepTime = currentFrameTime - t - 0.01f;
            if (sleepTime > 0)
                Thread.Sleep((int)(sleepTime * 1000));
            while (t < currentFrameTime)
                t = Time.realtimeSinceStartup;
        }
    }
}