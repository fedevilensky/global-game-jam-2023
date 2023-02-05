using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PolaroidEvent : MonoBehaviour
{
    [SerializeField] private GameObject polaroid;
    private Animator eventAnimator;
    private Animator imageAnimator;
    private void Start()
    {
        eventAnimator = this.GetComponent<Animator>();
        imageAnimator = polaroid.GetComponent<Animator>();

        GameEvents.current.onGetsPolaroid += GetPolaroid;
        
        GetPolaroid();
    }

    private void GetPolaroid()
    {
        eventAnimator.SetBool("Active", true);
        StartCoroutine(StartEvent());
    }

    private IEnumerator StartEvent()
    {
        yield return new WaitForSeconds(4f);
        imageAnimator.SetBool("Shown", true);
        yield return new WaitForSeconds(5f);
        // SceneManager.LoadScene(2); //Loads second level
    }
}
