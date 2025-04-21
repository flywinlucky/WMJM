using System.Collections;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private float _restartButtonAppearDelay;
    [SerializeField] private GameObject _restartButton;

    private void OnEnable()
    {
        _restartButton.SetActive(false);
        StartCoroutine(ShowRestartButton());
    }

    private IEnumerator ShowRestartButton()
    {
        yield return new WaitForSeconds(_restartButtonAppearDelay);
        _restartButton.SetActive(true);
    }
}