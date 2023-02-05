using UnityEngine;
using System.Collections;

public class Cookie : MonoBehaviour
{
	[Header("Animation")]
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float animationDuration;
    [SerializeField] private float animDelay; // Delay from going up to down.
	private float timer;

    private void Start()
    {
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        timer = 0.0f;

        while (timer <= animationDuration)
        {
            float curveValue = animationCurve.Evaluate(timer / animationDuration) * Time.deltaTime;
            transform.position += new Vector3(0f, curveValue, 0f);
            timer += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(animDelay);
        timer = 0.0f;
        while (timer <= animationDuration)
        {
            float curveValue = animationCurve.Evaluate(timer / animationDuration) * Time.deltaTime;
            transform.position += new Vector3(0f, -curveValue, 0f);
            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(animDelay);
        StartCoroutine(Animate());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameEvents.current.PlayerGetsCookie();
        Destroy(this.gameObject);
    }
}
