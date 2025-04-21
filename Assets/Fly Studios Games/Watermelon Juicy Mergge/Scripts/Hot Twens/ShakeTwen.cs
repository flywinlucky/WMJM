using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;

public class ShakeTwen : MonoBehaviour
{
    private Sequence bounceSequence;
    private bool isAnimating = false;

    public float oscilationIntensity = 30f;
    public float oscilationSpeed = 0.5f;
    public float bounceScaleAmount = 0.2f;
    public float bounceDuration = 0.3f;

    [Button]
    public void ShakeOscilationEffect()
    {
        if (Application.isPlaying)
        {
            Transform pivot = transform;

            Vector3 oscilationRotation = new Vector3(Random.Range(-oscilationIntensity, oscilationIntensity),
                                                     Random.Range(-oscilationIntensity, oscilationIntensity),
                                                     Random.Range(-oscilationIntensity, oscilationIntensity));

            pivot.DOPunchRotation(oscilationRotation, oscilationSpeed);
        }
    }

    [Button]
    public void ShakeCircularBouncingEffect()
    {
        if (Application.isPlaying && !isAnimating)
        {
            Transform pivot = transform;

            Vector3 initialScale = pivot.localScale;
            Vector3 bounceScale = Vector3.Scale(initialScale, new Vector3(1f + bounceScaleAmount, 1f + bounceScaleAmount, 1f + bounceScaleAmount));

            isAnimating = true;

            // Cancel any tweens currently running for pivot
            pivot.DOKill();

            bounceSequence = DOTween.Sequence();
            bounceSequence.Append(pivot.DOScale(bounceScale, bounceDuration / 2f))
                          .Append(pivot.DOScale(initialScale, bounceDuration / 2f))
                          .OnComplete(() => isAnimating = false);

            bounceSequence.Play();
        }
    }

    private void OnDestroy()
    {
        // Cancel the DOTween sequence when the object is destroyed
        if (bounceSequence != null)
        {
            bounceSequence.Kill();
            bounceSequence = null;
        }
    }
}