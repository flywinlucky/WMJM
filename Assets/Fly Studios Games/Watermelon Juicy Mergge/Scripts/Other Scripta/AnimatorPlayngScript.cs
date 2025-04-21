using Sirenix.OdinInspector;
using UnityEngine;

public class AnimatorPlayngScript : MonoBehaviour
{
    Animator animator;
    public string animationKey;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    [Button]
    public void PlayAnimation()
    {
        if (Application.isPlaying)
        {
            animator.Play("Base Layer." + animationKey, 0, 0.25f);
        }
    }
}