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

    public event Action onPlayerDies;

    public void PlayerDies()
    {
        onPlayerDies?.Invoke();
    }

    public event Action onGetsPolaroid;

    public void GetsPolaroid()
    {
        onGetsPolaroid?.Invoke();
    }
}
