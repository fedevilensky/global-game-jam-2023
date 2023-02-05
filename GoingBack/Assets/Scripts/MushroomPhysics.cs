using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using Random = System.Random;

public class MushroomPhysics : MonoBehaviour
{
  [SerializeField][Range(5f, 20f)] float jumpSpeed = 5f;
  private AudioSource audioClip;

  private void Start()
  {
    audioClip = this.GetComponent<AudioSource>();
  }

  void OnCollisionEnter2D(Collision2D other)
  {
    if (audioClip) audioClip.Play();
    var direction = (other.transform.position - transform.position).normalized;
    Debug.Log(direction.y);
    if (direction.y > 0f)
    {
      other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpSpeed);
    }
  }
  void OnTriggerEnter2D(Collider2D other)
  {
    if (audioClip) audioClip.Play();
    
    var direction = (other.transform.position - transform.position).normalized;
    Debug.Log(direction.y);
    if (direction.y > 0f)
    {
      other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpSpeed);
    }
  }
}
