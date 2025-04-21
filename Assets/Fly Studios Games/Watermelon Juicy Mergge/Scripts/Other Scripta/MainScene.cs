using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    public LoadScene loadScene;

    int firstOpenGame;

    // Start is called before the first frame update
    void Start()
    {

        if (PlayerPrefs.HasKey("firstOpenGame"))
        {

        }
        else
        {
            firstOpenGame += 1;
            PlayerPrefs.SetInt("firstOpenGame", firstOpenGame);

            loadScene.SwitchScene();
        }
    }
}