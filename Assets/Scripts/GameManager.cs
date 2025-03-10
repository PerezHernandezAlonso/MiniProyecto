using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; }
    public PlayerInputActions PlayerInputActions { get; private set; }
    [HideInInspector] public PlayerHealth Player { get; set; }
    

    private void Start()
    {
        Player = FindAnyObjectByType<PlayerHealth>();
    }
    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        PlayerInputActions = new PlayerInputActions();
    }
}
