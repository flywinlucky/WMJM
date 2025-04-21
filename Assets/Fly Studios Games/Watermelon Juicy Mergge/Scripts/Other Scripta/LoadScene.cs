using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;


public class LoadScene : MonoBehaviour
{
    [ValueDropdown("GetSceneNames")]
    public string sceneName;

    private void Awake()
    {

        GetComponent<Button>().onClick.AddListener(SwitchScene);
    }

    public void SwitchScene()
    {
        if (sceneName != null)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private List<string> GetSceneNames()
    {
        List<string> sceneNames = new List<string>();

        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            sceneNames.Add(sceneName);
        }

        return sceneNames;
    }
}