using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private CharacterMovement movementData;
    void Start()
    {
        animator = this.GetComponent<Animator>();
        movementData = this.GetComponent<CharacterMovement>();
    }

    void Update()
    {
        if (!animator || !movementData) return;
        
        
    }
}
