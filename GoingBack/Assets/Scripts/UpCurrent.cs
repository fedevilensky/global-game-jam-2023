using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpCurrent : MonoBehaviour
{
    [SerializeField][Range(0f, 10f)] float currentSpeed = 2f;
    [SerializeField] GlydePhysics physics = GlydePhysics.ConstantAcceleration;

    float timeElapsed = 0;
    [SerializeField][Range(0, 5)] float lerpDuration = 1;
    Vector2 startVelocity = Vector2.zero;
    void OnTriggerStay2D(Collider2D other)
    {
        // TODO(jcasanoval): Check if the player is gliding
        switch (physics)
        {
            case GlydePhysics.ConstantAcceleration:
                other.gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(0, currentSpeed * Time.deltaTime);
                break;
            case GlydePhysics.DynamicAcceleration:
                float otherCenter = other.transform.position.y + other.transform.localScale.y / 2;
                float positionPercentage = Mathf.Clamp01((otherCenter - transform.localScale.y) / -transform.localScale.y);
                other.gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(0, currentSpeed * Time.deltaTime * positionPercentage);
                break;
            case GlydePhysics.StaticSpeed:
                other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, currentSpeed);
                break;
            case GlydePhysics.StaticAccelerated:
                if (timeElapsed < lerpDuration)
                {
                    timeElapsed += Time.deltaTime;
                    Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
                    Vector2 newVelocity = new Vector2(rb.velocity.x, currentSpeed);
                    other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.Lerp(startVelocity, newVelocity, timeElapsed / lerpDuration);
                }
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (physics == GlydePhysics.StaticAccelerated)
        {
            timeElapsed = 0;
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            startVelocity = rb.velocity;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (physics == GlydePhysics.StaticAccelerated)
        {
            other.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }
}


enum GlydePhysics { ConstantAcceleration, DynamicAcceleration, StaticSpeed, StaticAccelerated }