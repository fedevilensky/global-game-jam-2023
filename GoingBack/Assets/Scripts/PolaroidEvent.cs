using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PolaroidEvent : MonoBehaviour
{
    [SerializeField] private GameObject polaroid;
    [SerializeField] private GameObject backgroundGO;
    [SerializeField] private int nextLevel = 2;
    private Image background;

    private Animator eventAnimator;
    private Animator imageAnimator;
    private void Start()
    {
        eventAnimator = this.GetComponent<Animator>();
        imageAnimator = polaroid.GetComponent<Animator>();

        background = backgroundGO.GetComponent<Image>();

        GameEvents.current.onGetsPolaroid += GetPolaroid;
    }

    private void GetPolaroid()
    {
        if (background) background.enabled = true;

        eventAnimator.SetBool("Active", true);
        StartCoroutine(StartEvent());
    }

    private IEnumerator StartEvent()
    {
        yield return new WaitForSeconds(2f);
        imageAnimator.SetBool("Shown", true);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(nextLevel); //Loads second level
    }
}
