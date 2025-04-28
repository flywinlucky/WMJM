using System.Collections;
using UnityEngine;

public class ActiveDelay : MonoBehaviour
{
    public float activeDelay = 2f; // Delay in seconds
    public GameObject handAnimated;

    private void OnEnable()
    {
        handAnimated.SetActive(false); // optional: îl ascundem inițial

        StartCoroutine(ActivateAfterDelay());
    }

    private IEnumerator ActivateAfterDelay()
    {
        if (handAnimated != null)
        {
            yield return new WaitForSeconds(activeDelay);
            handAnimated.SetActive(true);
        }
    }
}
