using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int cookies;

    private void Start()
    {
        GameEvents.current.onPlayerGetsCookie += GetsCookie;
    }

    private void GetsCookie()
    {
        Debug.Log("Working");
        if(cookies < 5)
            cookies++;
    }
}
