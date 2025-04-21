using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowSkinUnlocked : UIMonoBehaviour
{
    [SerializeField] private List<SphereImageNumber> _sphereImages;

    [Serializable]
    public struct SphereImageNumber
    {
        public Image Image;
        public int Number;
    }

    public void Show(List<WatermelonGameClone.GameManager.Spheres> spheres)
    {
        foreach(var image in _sphereImages)
        {
            image.Image.sprite = spheres[image.Number].sphere.itemSpriteRenderer.sprite;
        }

        gameObject.SetActive(true);
    }
}
