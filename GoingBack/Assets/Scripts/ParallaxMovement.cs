using UnityEngine;

class ParallaxMovement : MonoBehaviour
{

    [SerializeField][Range(0.0001f, 0.99f)] float parallaxSpeed = 0.5f;
    [SerializeField] Transform cameraTransform;

    Vector2 startPos;


    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float dist = (cameraTransform.position.x * parallaxSpeed);
        transform.position = startPos + Vector2.right * dist;
    }

}