using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class ScaleBouncingSpawn : MonoBehaviour
{
    [Header("Settings")]
    [Space]

    public float initializeAfer;

    public float bounceDuration = 1f;

    [Space]
    public bool initializeOnEnable;
    public bool desactivateAfterDisableAnim;
    [Space]
    public bool useCustomScale;

    [ShowIf("useCustomScale")]
    private Vector3 customScale;

    [Space]
    public Ease onEnableAnimation;
    public Ease onDisableAnimation;

    [ShowIf("useCustomScale")]
    public Vector3 initialScale;
    private Tween bounceTween;

    public AudioSource audioSource;

    private bool onlyOanceGetScale;

    private void OnEnable()
    {
        if (audioSource)
            audioSource.playOnAwake = false;

        if (!onlyOanceGetScale)
        {
            if (useCustomScale)
            {
                initialScale = customScale;
            }
            else
            {
                initialScale = transform.localScale;
            }

            onlyOanceGetScale = true;
        }



        if (initializeOnEnable)
        {
            transform.localScale = new Vector3(0, 0, 0);
            Invoke("StartScaling", initializeAfer);
        }
    }

    public void StartScaling()
    {
        if (audioSource)
            audioSource.Play();

        transform.localScale = Vector3.zero;
        bounceTween = transform.DOScale(initialScale, bounceDuration)
            .SetEase(onEnableAnimation);
    }

    public void StartDiiscaling()
    {
        transform.localScale = initialScale;
        bounceTween = transform.DOScale(Vector3.zero, bounceDuration)
            .SetEase(onDisableAnimation)
            .OnComplete(DeactivateObjectIfNeeded);
    }

    private void DeactivateObjectIfNeeded()
    {
        if (desactivateAfterDisableAnim)
        {
            gameObject.SetActive(false);
        }
    }


    private void OnDisable()
    {
        // Asigură-te că tweener-ul este oprit (kill) când obiectul este distrus
        if (bounceTween != null)
        {
            bounceTween.Kill();
        }
    }

    private void OnDestroy()
    {
        // Asigură-te că tweener-ul este oprit (kill) când obiectul este distrus
        if (bounceTween != null)
        {
            bounceTween.Kill();
        }
    }
}