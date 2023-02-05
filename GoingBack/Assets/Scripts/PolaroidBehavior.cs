using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolaroidBehavior : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = this.GetComponent<Animator>();
        
        GameEvents.current.onGetsPolaroid += AlphaAnimationVoid;
    }

    private void AlphaAnimationVoid()
    {
        if (animator) animator.SetBool("Disappear", true);
    }
}
