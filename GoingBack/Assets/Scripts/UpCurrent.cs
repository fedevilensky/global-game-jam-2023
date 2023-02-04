using UnityEngine;

public class UpCurrent : MonoBehaviour
{
    [SerializeField][Range(0f, 10f)] float airCurrentSpeed = 2f;
    [SerializeField] GlydePhysics physics = GlydePhysics.ConstantAcceleration;

    float timeElapsed = 0;

    bool _wasGliding = false;
    [SerializeField][Range(0, 5)] float lerpDuration = 1;
    Vector2 startVelocity = Vector2.zero;
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player")
            return;
        if (other.gameObject.GetComponent<CharacterMovement>().isGliding)
        {
            if (!_wasGliding && physics == GlydePhysics.StaticAccelerated)
            {
                StartGlide(other);
            }
            RunPhysics(other);
        }
        else
        {
            _wasGliding = false;
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            rb.gravityScale = 1;
        }
    }

    void StartGlide(Collider2D other)
    {
        _wasGliding = true;
        timeElapsed = 0;
        Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        startVelocity = rb.velocity;
    }

    void RunPhysics(Collider2D other)
    {
        switch (physics)
        {
            case GlydePhysics.ConstantAcceleration:
                other.gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(0, airCurrentSpeed * Time.deltaTime);
                break;
            case GlydePhysics.DynamicAcceleration:
                float otherCenter = other.transform.position.y + other.transform.localScale.y / 2;
                float positionPercentage = Mathf.Clamp01((otherCenter - transform.localScale.y) / -transform.localScale.y);
                other.gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(0, airCurrentSpeed * Time.deltaTime * positionPercentage);
                break;
            case GlydePhysics.StaticSpeed:
                other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, airCurrentSpeed);
                break;
            case GlydePhysics.StaticAccelerated:
                if (timeElapsed < lerpDuration)
                {
                    timeElapsed += Time.deltaTime;
                    Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
                    Vector2 newVelocity = new Vector2(rb.velocity.x, airCurrentSpeed);
                    other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.Lerp(startVelocity, newVelocity, timeElapsed / lerpDuration);
                }
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player")
            return;
        other.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
    }
}


enum GlydePhysics { ConstantAcceleration, DynamicAcceleration, StaticSpeed, StaticAccelerated }