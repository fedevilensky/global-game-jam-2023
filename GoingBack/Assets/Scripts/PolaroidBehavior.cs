using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolaroidBehavior : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioClip;
    void Start()
    {
        animator = this.GetComponent<Animator>();
        audioClip = this.GetComponent<AudioSource>();
        
        GameEvents.current.onGetsPolaroid += AlphaAnimationVoid;
    }

    private void AlphaAnimationVoid()
    {
        if (animator) animator.SetBool("Disappear", true);
        StartCoroutine(Destroy());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (audioClip) audioClip.Play();
        GameEvents.current.GetsPolaroid();
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
}
