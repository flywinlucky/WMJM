using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    [SerializeField] private float _sceneChangeDelay;
    private static bool _hasSceneSwitched = false;

    private void Awake()
    {
        // Asigură-te că obiectul nu este distrus între scene
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (!_hasSceneSwitched)
        {
            _hasSceneSwitched = true;
            StartCoroutine(LoadSceneAfterDelay(_sceneChangeDelay));
        }
        else
        {
            // Dacă deja s-a schimbat scena, poți distruge obiectul
            Destroy(gameObject);
        }
    }

    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(1);
    }
}
