using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
  GameManager gameManager;

  void Start()
  {
    gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
  }

  void OnTriggerEnter2D(Collider other)
  {
    if (other.gameObject.tag == "Player")
    {
      gameManager.SetCheckpoint(transform);
    }
  }
}
