using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    enum Direction
    {
        LEFT,
        RIGHT
    }

    [SerializeField][Range(1f, 20f)] float movementSpeed = 5f;
    [SerializeField][Range(1f, 20f)] float jumpSpeed = 10f;
    [SerializeField] bool canGlide = true;
    [SerializeField][Range(0f, 5f)] float glideSpeed = 2f;
    [SerializeField] public bool canHook { get; private set; } = true;
    [SerializeField][Range(5f, 50f)] float hookPointThresholdsMax = 25f;
    [SerializeField][Range(0f, 50f)] float hookPointThresholdsMin = 1f;
    Rigidbody2D rbody2d;
    public GameObject reachableHookPoint { get; private set; } = null;

    Direction facing = Direction.LEFT;
    bool canJump = true;
    bool isJumping = false;

    // Start is called before the first frame update
    void Start()
    {
        rbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessMovement();
        ProcessJumpAndGlide();
        ProcessHook();
    }

    void ProcessHook()
    {
        UpdateReachableHookPoint();
    }


    private void ProcessMovement()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        transform.position += new Vector3(Time.deltaTime * horizontalAxis * movementSpeed, 0);
        if (horizontalAxis > 0)
        {
            facing = Direction.RIGHT;
        }
        else if (horizontalAxis < 0)
        {
            facing = Direction.LEFT;
        }
    }

    private void ProcessJumpAndGlide()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rbody2d.velocity = new Vector2(rbody2d.velocity.x, jumpSpeed);
            canJump = false;
            isJumping = true;
        }
        else if (Input.GetKey(KeyCode.Space) && canGlide)
        {
            if (rbody2d.velocity.y < -glideSpeed)
            {
                rbody2d.velocity = new Vector2(rbody2d.velocity.x, -glideSpeed);
            }
        }
    }

    void UpdateReachableHookPoint()
    {
        var hookPoints = ListAllHooksOrdered();
        UnmarkHookPoint(reachableHookPoint);

        foreach (var hookPoint in hookPoints)
        {
            if (!isReachable(hookPoint))
            {
                //then no other hook point will be reachable, since they are ordered from closest to furthest
                reachableHookPoint = null;
                return;
            }
            if (!isBeyondMinThreshold(hookPoint)) continue;
            if (!isAbovePlayer(hookPoint)) continue;
            if (!isInFacingDirection(hookPoint)) continue;
            if (!NoObstableInbewteen(hookPoint)) continue;

            MarkHookPoint(hookPoint);
            reachableHookPoint = hookPoint;
            return;

        }
    }

    private bool isAbovePlayer(GameObject hookPoint)
    {
        return hookPoint.transform.position.y > transform.position.y;
    }

    private bool isBeyondMinThreshold(GameObject hookPoint)
    {
        var distance = Vector3.Distance(transform.position, hookPoint.transform.position);
        return distance > hookPointThresholdsMin;
    }

    private bool NoObstableInbewteen(GameObject hookPoint)
    {
        var direction = hookPoint.transform.position - transform.position;
        var distance = Vector3.Distance(transform.position, hookPoint.transform.position);
        var hit = Physics2D.Raycast(transform.position, direction, distance);
        return hit.collider == null || hit.collider.gameObject == hookPoint;
    }

    private bool isInFacingDirection(GameObject hookPoint)
    {
        if (facing == Direction.LEFT)
        {
            return hookPoint.transform.position.x < transform.position.x;
        }
        //if (facing == Direction.RIGHT)
        return hookPoint.transform.position.x > transform.position.x;
    }

    private void UnmarkHookPoint(GameObject hookPoint)
    {
        if (hookPoint != null)
        {
            var spriteRenderer = hookPoint.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.white;
            }
        }
    }
    private void MarkHookPoint(GameObject hookPoint)
    {
        if (hookPoint != null)
        {
            var spriteRenderer = hookPoint.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.blue;
            }
        }
    }

    private bool isReachable(GameObject hookPoint)
    {
        return Vector3.Distance(transform.position, hookPoint.transform.position) < hookPointThresholdsMax;
    }

    List<GameObject> ListAllHooksOrdered()
    {
        GameObject[] hooksArr = GameObject.FindGameObjectsWithTag("HookPoint");
        var hooks = new List<GameObject>(hooksArr);
        hooks.Sort((GameObject hook1, GameObject hook2) =>
        {
            var d1 = Vector3.Distance(transform.position, hook1.transform.position);
            var d2 = Vector3.Distance(transform.position, hook2.transform.position);
            return d1.CompareTo(d2);
        });
        return hooks;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        canJump = true;
        isJumping = false;
        UnmarkHookPoint(reachableHookPoint);
    }

    void NotifyCollisionWithBlock() { }
    void NotifyExitCollisionWithBlock() { }


}
