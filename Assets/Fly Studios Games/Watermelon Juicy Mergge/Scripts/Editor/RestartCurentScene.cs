using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartCurentScene : EditorWindow
{
    [MenuItem("Tools/Restart Curent Scene")]
    public static void RestartCurentSceneIndex()
    {
        if (Application.isPlaying)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Restart Curent Scene");
        }
    }
}