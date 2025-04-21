using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public float duration;
    public bool enableOnStart;

    [ShowIf("enableOnStart")]
    [Required]
    public GameObject loadingScreenObject;

    private void Start()
    {
        if (enableOnStart && loadingScreenObject)
        {
            StartCoroutine(ShowLoadingScreenCoroutine());
        }
    }

    private IEnumerator ShowLoadingScreenCoroutine()
    {
        var instantiedObject = Instantiate(loadingScreenObject);
        loadingScreenObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        Destroy(instantiedObject);
    }
}