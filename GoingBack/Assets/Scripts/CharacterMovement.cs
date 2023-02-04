using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public enum Direction
    {
        LEFT,
        RIGHT
    }

    // Public Properties
    public Direction facing { get; private set; } = Direction.LEFT;


    public GameObject reachableHookPoint { get; private set; } = null;
    [SerializeField] public bool canHook { get; private set; } = true;
    public bool isJumping { get; private set; } = false;
    public bool isGliding { get; private set; } = false;
    public float jumpCharge { get; private set; } = 0f;
    public bool isOnGround { get; private set; } = true;


    // Private Properties/Fields

    [SerializeField][Range(0.5f, 2f)] float jumpChargeRate = 1f;

    [SerializeField][Range(1f, 20f)] float movementSpeed = 5f;
    [SerializeField][Range(1f, 20f)] float maxJumpSpeed = 10f;
    [SerializeField] bool canGlide = true;
    [SerializeField][Range(0f, 5f)] float glideSpeed = 2f;
    [SerializeField][Range(5f, 50f)] float hookPointThresholdsMax = 25f;
    [SerializeField][Range(0f, 50f)] float hookPointThresholdsMin = 1f;
    [SerializeField][Range(0f, 5f)] float hookStrength = 0.5f;
    Rigidbody2D rbody2d;
    HookBehaviour hook;
    float gravityScale;

    HingeJoint2D hinge;


    // Public Methods

    // Private Methods

    // Start is called before the first frame update
    void Start()
    {
        rbody2d = GetComponent<Rigidbody2D>();
        gravityScale = rbody2d.gravityScale;
        if (canHook)
        {
            hook = transform.Find("Hook").gameObject.GetComponent<HookBehaviour>();
            hinge = GetComponent<HingeJoint2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if hook is attached, then no other movement is allowed
        if (!canHook || !hook.isHooked)
        {
            ProcessMovement();
            ProcessJumpAndGlide();
        }
        if (canHook)
        {
            ProcessHook();
        }
    }

    void ProcessHook()
    {
        if (canHook)
        {
            UpdateReachableHookPoint();
            if (hook.isHooked)
            {
                DoHook();
            }
        }
    }

    void DoHook()
    {
        Vector2 TDirection = (hook.hookedTo.transform.position - transform.position).normalized;
        var F = TDirection * hookStrength;

        rbody2d.AddForce(F);

    }

    void ProcessMovement()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");

        if (horizontalAxis > 0)
        {
            if (isOnGround)
            {
                rbody2d.velocity = new Vector3(
              horizontalAxis * movementSpeed,
               rbody2d.velocity.y);
            }
            else
            {
                rbody2d.velocity = new Vector3(
                Mathf.Max(horizontalAxis * movementSpeed, rbody2d.velocity.x),
                 rbody2d.velocity.y);
            }
            facing = Direction.RIGHT;
        }
        else if (horizontalAxis < 0)
        {
            if (isOnGround)
            {
                rbody2d.velocity = new Vector3(
                  horizontalAxis * movementSpeed,
                  rbody2d.velocity.y
                );
            }
            else
            {
                rbody2d.velocity = new Vector3(
                Mathf.Min(horizontalAxis * movementSpeed, rbody2d.velocity.x),
                 rbody2d.velocity.y);
            }
            facing = Direction.LEFT;
        }
    }

    void ProcessJumpAndGlide()
    {
        if (isOnGround && Input.GetKey(KeyCode.Space))
        {
            jumpCharge = Mathf.Min(jumpCharge + Time.deltaTime * maxJumpSpeed / jumpChargeRate, maxJumpSpeed);
        }
        if (Input.GetKeyUp(KeyCode.Space) && isOnGround)
        {
            rbody2d.velocity = new Vector2(rbody2d.velocity.x, jumpCharge);
            jumpCharge = 0f;

            if (jumpCharge > 1f)
            {
                isOnGround = false;
                isJumping = true;
            }
        }
        else if (Input.GetKey(KeyCode.Space) && canGlide)
        {
            isGliding = true;
            if (rbody2d.velocity.y < -glideSpeed)
            {
                rbody2d.velocity = new Vector2(rbody2d.velocity.x, -glideSpeed);
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isGliding = false;
        }
    }

    void UpdateReachableHookPoint()
    {
        var hookPoints = ListAllHooksOrdered();

        foreach (var hookPoint in hookPoints)
        {
            if (!isReachable(hookPoint))
            {
                //then no other hook point will be reachable, since they are ordered from closest to furthest
                UnmarkHookPoint(reachableHookPoint);

                reachableHookPoint = null;
                return;
            }
            if (!isBeyondMinThreshold(hookPoint)) continue;
            if (!isAbovePlayer(hookPoint)) continue;
            if (!isInFacingDirection(hookPoint)) continue;
            if (!NoObstableInbewteen(hookPoint)) continue;
            if (hookPoint != reachableHookPoint)
            {
                UnmarkHookPoint(reachableHookPoint);
                MarkHookPoint(hookPoint);
                reachableHookPoint = hookPoint;
            }
            return;

        }
    }

    bool isAbovePlayer(GameObject hookPoint)
    {
        return hookPoint.transform.position.y > transform.position.y;
    }

    bool isBeyondMinThreshold(GameObject hookPoint)
    {
        var distance = Vector3.Distance(transform.position, hookPoint.transform.position);
        return distance > hookPointThresholdsMin;
    }

    bool NoObstableInbewteen(GameObject hookPoint)
    {
        var direction = hookPoint.transform.position - transform.position;
        var distance = Vector3.Distance(transform.position, hookPoint.transform.position);
        var hit = Physics2D.Raycast(transform.position, direction, distance);
        return hit.collider == null || hit.collider.gameObject == hookPoint;
    }

    bool isInFacingDirection(GameObject hookPoint)
    {
        if (facing == Direction.LEFT)
        {
            return hookPoint.transform.position.x < transform.position.x;
        }
        //if (facing == Direction.RIGHT)
        return hookPoint.transform.position.x > transform.position.x;
    }

    void UnmarkHookPoint(GameObject hookPoint)
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
    void MarkHookPoint(GameObject hookPoint)
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

    bool isReachable(GameObject hookPoint)
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
        if (!other.isTrigger && other.transform.position.y < transform.position.y)
        {
            isOnGround = true;
            isJumping = false;
            UnmarkHookPoint(reachableHookPoint);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.isTrigger && other.transform.position.y < transform.position.y)
        {
            isOnGround = false;
            isJumping = true;
        }
    }

    void NotifyEnterCollisionWithBlock() { }
    void NotifyExitCollisionWithBlock() { }
}
