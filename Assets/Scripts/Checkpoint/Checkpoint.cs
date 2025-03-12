using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Checkpoint alcanzado: " + transform.position);
            CheckpointManager.Singleton.SetCheckpoint(transform.position);
        }
    }
}