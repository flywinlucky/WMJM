using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Sirenix.OdinInspector;

public class MediationManager : MonoBehaviour
{
    private bool offlinePanelInstantiated;

    public bool networkRequest;

    [Header("Offline Panel")]
    [ShowIf("networkRequest")]
    public GameObject offlinePanelPopUp;
    public Transform offlinePanelSpawnPosition;
    private GameObject instantiedOfflinePopUp;

    [Header("Networking Settings")]
    [Space]
    [Range(0f, 10f)]
    [ShowIf("networkRequest")]
    public float refreshRate;

    [Header("Debug")]
    [Space]
    public bool showDebugStats;

    [Header("NETWORK STATUS")]
    [ShowIf("showDebugStats")]
    [ReadOnly]
    public bool networkStatus;

    [Header("PING STATUS")]
    [Space]
    [ShowIf("showDebugStats")]
    [ReadOnly]
    public bool pingStatus;

    [ShowIf("showDebugStats")]
    [Space]
    [ReadOnly]
    public float ping_float;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckRequestRoutine());
    }

    #region Check Internet Connection

    private IEnumerator CheckRequestRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(refreshRate);

            if (networkRequest)
                StartCoroutine(CheckInternetConnection());
        }
    }

    private IEnumerator CheckInternetConnection()
    {
        UnityWebRequest request = new UnityWebRequest("https://www.google.com/");
        yield return request.SendWebRequest();

        if (string.IsNullOrEmpty(request.error))
        {
            networkStatus = false;

            if (offlinePanelInstantiated)
            {
                DestroyOfflinePanel();
            } 
        }
        else
        {
            networkStatus = true;

            if (!offlinePanelInstantiated)
            {
                instantiedOfflinePopUp = Instantiate(offlinePanelPopUp, offlinePanelSpawnPosition);
                offlinePanelInstantiated = true;
            }
        }
    }

    private void DestroyOfflinePanel()
    {
        Destroy(instantiedOfflinePopUp);
        offlinePanelInstantiated = false;
    }

    #endregion
}
