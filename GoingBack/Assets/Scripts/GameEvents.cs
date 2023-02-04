using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }
    
    public event Action onPlayerGetsCookie;

    public void PlayerGetsCookie()
    {
        onPlayerGetsCookie?.Invoke();
    }
    
    
}
