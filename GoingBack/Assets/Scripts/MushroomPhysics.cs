using UnityEngine;

public class MushroomPhysics : MonoBehaviour
{
  [SerializeField][Range(5f, 20f)] float jumpSpeed = 5f;
  void OnCollisionEnter2D(Collision2D other)
  {
    var direction = (other.transform.position - transform.position).normalized;
    Debug.Log(direction.y);
    if (direction.y > 0f)
    {
      other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpSpeed);
    }
  }
  void OnTriggerEnter2D(Collider2D other)
  {
    var direction = (other.transform.position - transform.position).normalized;
    Debug.Log(direction.y);
    if (direction.y > 0f)
    {
      other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpSpeed);
    }
  }
}
