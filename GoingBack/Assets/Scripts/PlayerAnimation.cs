using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer _spriteRenderer;
    private CharacterMovement movementData;
    private Rigidbody2D _rigidbody2D;

    void Start()
    {
        animator = this.GetComponent<Animator>();
        movementData = this.GetComponent<CharacterMovement>();
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _rigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Error handling
        if (!animator || !movementData || !_spriteRenderer || !_rigidbody2D)
        {
            Debug.Log("Some components not found");
            return;
        }

        if (movementData.facing == CharacterMovement.Direction.LEFT) _spriteRenderer.flipX = true;
        else _spriteRenderer.flipX = false;
        
        if (movementData.isOnGround && _rigidbody2D.velocity.x != 0)
        {
            animator.SetBool("Walking", true);
        } else animator.SetBool("Walking", false);
        
        if (movementData.jumpCharge > 0) animator.SetFloat("Jumping", movementData.jumpCharge);
        else animator.SetFloat("Jumping", 0f);
        
        if(movementData.isOnGround == false && _rigidbody2D.velocity.y < 0) animator.SetBool("Floating", true);
        else animator.SetBool("Floating", false);
    }
}
