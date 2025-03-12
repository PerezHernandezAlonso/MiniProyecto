using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Singleton;
    private Vector3 lastCheckpoint;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject); // No destruir al cambiar de escena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCheckpoint(Vector3 position)
    {
        lastCheckpoint = position;
    }

    public Vector3 GetLastCheckpoint()
    {
        return lastCheckpoint;
    }
}