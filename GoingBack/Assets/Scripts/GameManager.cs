using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static int cookies;
  public GameObject player;
  Transform lastCheckpoint;

  private void Start()
  {
    GameEvents.current.onPlayerGetsCookie += GetsCookie;
    player = GameObject.Find("Player");
  }

  private void GetsCookie()
  {
    Debug.Log("Working");
    if (cookies < 5)
      cookies++;
  }

  internal void SetCheckpoint(Transform transform)
  {
    lastCheckpoint = transform;
  }

  internal void RespawnPlayer()
  {
    player.transform.position = lastCheckpoint.position;

  }
}
