using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{


    [SerializeField][Range(1f, 20f)] float movementSpeed = 5f;
    [SerializeField][Range(1f, 20f)] float jumpSpeed = 4f;
    [SerializeField] bool canGlide = true;
    [SerializeField][Range(0f, 5f)] float glideSpeed = 2f;
    Rigidbody2D rigidbody2D;
    bool canJump = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        transform.position += new Vector3(Time.deltaTime * horizontalAxis * movementSpeed, 0);
        ProcessJumpAndGlide();
    }

    private void ProcessJumpAndGlide()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpSpeed);
            canJump = false;
        }
        else if (Input.GetKey(KeyCode.Space) && canGlide)
        {
            if (rigidbody2D.velocity.y < -glideSpeed)
            {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, -glideSpeed);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        canJump = true;
    }

}
