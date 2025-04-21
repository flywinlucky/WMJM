using UnityEngine;
using UnityEngine.UI;

public class SetSpriteUi : MonoBehaviour
{
    public Image image;

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }
}