using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    [SerializeField] private GameObject platform;
    private Collider2D thisCollider;
    [SerializeField] private float destroyTimer;
    [SerializeField] private float regenerateTimer;
    [SerializeField] private bool regenerate = true;

    private void Start()
    {
        thisCollider = this.GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        StartCoroutine(PlayerInteraction());

    }

    private IEnumerator PlayerInteraction()
    {
        yield return new WaitForSeconds(destroyTimer);
        platform.SetActive(false);
        thisCollider.enabled = false;
        yield return new WaitForSeconds(regenerateTimer);
        if(regenerate) platform.SetActive(true);
        thisCollider.enabled = true;
    }
}
