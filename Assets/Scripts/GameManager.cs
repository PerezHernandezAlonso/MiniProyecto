using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; }
    public PlayerInputActions PlayerInputActions { get; private set; }
    [HideInInspector] public PlayerHealth Player { get; set; }

    public ParticleSystem[] particles;
    

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

    public void SpawnVFX(ParticleSystem particlePrefab, GameObject target)
    {
        if (particlePrefab != null && target != null)
        {
            Debug.Log("Estas spawneando un VFX");
            ParticleSystem newVFX = Instantiate(particlePrefab, target.transform.position, Quaternion.identity, target.transform);

            newVFX.Play();

            // Destruir el efecto una vez finalice
            Destroy(newVFX.gameObject, newVFX.main.duration + newVFX.main.startLifetime.constantMax);
        }
    }
}
