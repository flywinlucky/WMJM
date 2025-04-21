using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticLoadingScreen : MonoBehaviour
{
    static bool isLoadingScreenCreated = false;
    public Animator loadingScreen;
    public float loadingScreenDuration;

    // Start is called before the first frame update
    void Start()
    {
        // Asignăm durata animației loadingScreen la loadingScreenDuration
        loadingScreenDuration = loadingScreen.runtimeAnimatorController.animationClips[0].length;

        // Dacă obiectul nu a fost încă creat
        if (!isLoadingScreenCreated)
        {
            // Îl facem nemuritor
            DontDestroyOnLoad(gameObject);
            isLoadingScreenCreated = true; // Setăm flagul ca obiectul a fost creat
        }
        else
        {
            // Dacă obiectul a fost deja creat în altă scenă, distrugem duplicatul
            Destroy(gameObject);
        }
    }

    [Button]
    public void OpenLoadingScreen()
    {
        loadingScreen.SetBool("LoadingScreenState", true);
    }

    [Button]
    public void CloseLoadingScreen()
    {
        loadingScreen.SetBool("LoadingScreenState", false);
    }

    [Button]
    public void CloseLoadingScreenWithDelay()
    {
        StartCoroutine(ClosePopUpDelay());
    
    }

    public IEnumerator ClosePopUpDelay()
    {
        yield return new WaitForSeconds(2f);

        loadingScreen.SetBool("LoadingScreenState", false);
    }


    [Button]
    public void OpenLoadingScreenAndCloseDelay()
    {
        loadingScreen.SetBool("LoadingScreenState", true);

        StartCoroutine(ClosePopUpDelay());
    }
}