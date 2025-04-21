using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    public bool enableScriptOnStart;

    static bool dontDestroyOnLoadIsCreated = false;

    // Start is called before the first frame update
    void Start()
    {
        if (enableScriptOnStart)
        {
            // Dacă obiectul nu a fost încă creat
            if (!dontDestroyOnLoadIsCreated)
            {
                // Îl facem nemuritor
                DontDestroyOnLoad(gameObject);
                dontDestroyOnLoadIsCreated = true; // Setăm flagul ca obiectul a fost creat
            }
            else
            {
                // Dacă obiectul a fost deja creat în altă scenă, distrugem duplicatul
                Destroy(gameObject);
            }
        }
    }
}
