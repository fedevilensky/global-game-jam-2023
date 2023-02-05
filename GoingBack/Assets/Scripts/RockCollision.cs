using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCollision : MonoBehaviour
{
  void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.tag == "Player")
    {
      var characterMovement = other.gameObject.GetComponent<CharacterMovement>();

      characterMovement.NotifyCollisionWithBlock();
    }
  }

  void OnCollisionExit2D(Collision2D other)
  {
    if (other.gameObject.tag == "Player")
    {
      var characterMovement = other.gameObject.GetComponent<CharacterMovement>();

      characterMovement.NotifyExitCollisionWithBlock();
    }
  }
}
