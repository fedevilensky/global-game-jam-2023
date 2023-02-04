using UnityEngine;

public class HookBehaviour : MonoBehaviour
{
    public bool isHooked { get => spriteRenderer.enabled; }


    GameObject parent;
    CharacterMovement parentMovement;
    Transform hookedTo;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
        parentMovement = parent.GetComponent<CharacterMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (parentMovement.canHook)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                if (parentMovement.reachableHookPoint != null)
                {
                    ThrowHook();
                }
            }
            if (Input.GetKeyUp(KeyCode.V))
            {
                PullHook();
            }
            if (Input.GetKey(KeyCode.V) && hookedTo != null)
            {
                UpdateHookTransform();
            }
        }

    }
    private void PullHook()
    {
        spriteRenderer.enabled = false;
        hookedTo = null;
    }

    private void ThrowHook()
    {
        hookedTo = parentMovement.reachableHookPoint.transform;
        spriteRenderer.enabled = true;

        UpdateHookTransform();
    }

    private void UpdateHookTransform()
    {

        // if no hook point is reachable, then the hook should be pulled
        if (HasObstacleBetweenPlayerAndHookPoint())
        {
            PullHook();
            return;
        }

        //set hook position to mid point between player and reachable hook
        var midPoint = (parent.transform.position + hookedTo.transform.position) / 2;
        transform.position = midPoint;
        //set hook rotation to face the hook point
        var direction = hookedTo.transform.position - parent.transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //set hook scale to distance between player and hook point
        var distance = Vector3.Distance(parent.transform.position, hookedTo.transform.position);
        transform.localScale = new Vector3(distance, 0.5f, 1);
    }

    private bool HasObstacleBetweenPlayerAndHookPoint()
    {
        var direction = hookedTo.transform.position - parent.transform.position;
        var distance = Vector3.Distance(parent.transform.position, hookedTo.transform.position);
        var hit = Physics2D.Raycast(parent.transform.position, direction, distance);
        return hit.collider != null && hit.collider.gameObject != hookedTo;
    }
}
