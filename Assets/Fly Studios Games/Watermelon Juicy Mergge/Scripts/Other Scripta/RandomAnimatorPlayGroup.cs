using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimatorPlayGroup : MonoBehaviour
{
    public Animator[] animator;
    public string animationKey;

    [Button]
    public void PlayAnimation()
    {
        if (Application.isPlaying)
        {
            int randomIndex = Random.Range(0, animator.Length);
            animator[randomIndex].Play("Base Layer." + animationKey, 0, 0.25f);
        }
    }
}