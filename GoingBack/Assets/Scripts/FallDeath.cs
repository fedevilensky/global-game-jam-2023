using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDeath : MonoBehaviour
{
  GameManager gameManager;
  private AudioSource audioClip;

void Start()
  {
    gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    audioClip = this.GetComponent<AudioSource>();
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.tag == "Player")
    {
      gameManager.RespawnPlayer();
      if (audioClip) audioClip.Play();
    }
  }
}
