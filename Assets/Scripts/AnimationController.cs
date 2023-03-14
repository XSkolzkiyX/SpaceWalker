using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    void Update()
    {
        if (Input.anyKeyDown) animator.SetTrigger("Dance");
    }
}
