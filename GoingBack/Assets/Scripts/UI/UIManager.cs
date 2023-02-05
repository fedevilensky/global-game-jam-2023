using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    // Cookies
    private int currentCookies = 0;
    [SerializeField] private GameObject cookieUI;
    private Image cookieSprite;
    private Animator animator;
    private AudioSource cookieAudio;
    [SerializeField] private Sprite[] cookieStates;

    private void Start()
    {
        // CookiesCounter
        currentCookies = GameManager.cookies;
        GameEvents.current.onPlayerGetsCookie += UpdateCookies;
        cookieSprite = cookieUI.GetComponent<Image>();
        animator = cookieUI.GetComponent<Animator>();
        cookieAudio = cookieUI.GetComponent<AudioSource>();
    }

    private void UpdateCookies()
    {
        StartCoroutine(AnimateCookieCounter());
        // It should take cookies from GameManager, but GameEvents have to be in certain order.
        if (currentCookies < 5) currentCookies++;

        if (currentCookies == 1) cookieSprite.enabled = true;
        else cookieSprite.sprite = cookieStates[currentCookies - 1];
        
        if (cookieAudio) cookieAudio.Play();
    }

    private IEnumerator AnimateCookieCounter()
    {
        animator.SetBool("NewCookie", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("NewCookie", false);
    }
}
