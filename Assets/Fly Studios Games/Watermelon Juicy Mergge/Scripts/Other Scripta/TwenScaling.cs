using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;

public class TwenScaling : MonoBehaviour
{
    [SerializeField] private float _increaseScale;
    private Vector3 _originalScale;

    private void Start()
    {
        _originalScale = transform.localScale;
    }

    private void OnDisable()
    {
        DOTween.Kill(transform);
        transform.localScale = _originalScale;
    }

    [Button]
    public void TwenObjectScaling()
    {
        Vector3 targetScale = transform.localScale + Vector3.one * _increaseScale;

        transform.DOScale(targetScale, _increaseScale)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                transform.DOScale(_originalScale, _increaseScale)
                    .SetEase(Ease.OutQuad);
            });
    }
}