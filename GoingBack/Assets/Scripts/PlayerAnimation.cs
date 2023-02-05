using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer _spriteRenderer;
    private CharacterMovement movementData;
    private Rigidbody2D _rigidbody2D;

    private AudioSource audioClip;
    [SerializeField] private AudioClip walkingSound;
    [SerializeField] private AudioClip jumpSound;

    void Start()
    {
        animator = this.GetComponent<Animator>();
        movementData = this.GetComponent<CharacterMovement>();
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _rigidbody2D = this.GetComponent<Rigidbody2D>();
        audioClip = this.GetComponent<AudioSource>();
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

        // Movement animation and sound
        if (movementData.isOnGround && _rigidbody2D.velocity.x != 0)
        {
            animator.SetBool("Walking", true);
            if (audioClip)
            {
                audioClip.clip = walkingSound;
                audioClip.loop = true;
                audioClip.Play();
            }
        }
        else
        {
            if (audioClip.clip == walkingSound) audioClip.Stop();
            animator.SetBool("Walking", false);
        }

        // Jump
        if (movementData.jumpCharge > 0)
        {
            if (audioClip)
            {
                audioClip.clip = jumpSound;
                audioClip.loop = false;
                audioClip.Play();
            }
            animator.SetFloat("Jumping", movementData.jumpCharge);
        }
        else animator.SetFloat("Jumping", 0f);


        // Floating
        if(movementData.isOnGround == false && _rigidbody2D.velocity.y < 0) animator.SetBool("Floating", true);
        else animator.SetBool("Floating", false);
    }
}
