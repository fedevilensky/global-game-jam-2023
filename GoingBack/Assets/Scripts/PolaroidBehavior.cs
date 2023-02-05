using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolaroidBehavior : MonoBehaviour
{
    private float alphaNumber = 255f;
    private SpriteRenderer _spriteRenderer;
    void Start()
    {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        GameEvents.current.onGetsPolaroid += AlphaAnimationVoid;
        StartCoroutine(AlphaAnimation());
    }

    private void AlphaAnimationVoid()
    {
        StartCoroutine(AlphaAnimation());
    }
    private IEnumerator AlphaAnimation()
    {
        while (alphaNumber != 0)
        {
            alphaNumber = Mathf.Lerp(alphaNumber, 0f, .2f);
            _spriteRenderer.color = new Color(255f, 255f, 255f, alphaNumber);
        }

        yield return null;
    }
}
