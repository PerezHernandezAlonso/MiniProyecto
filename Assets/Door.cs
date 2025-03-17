using UnityEngine;

public class Door : MonoBehaviour
{
    public bool canOpen = false;
    public GameObject barrier;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenDoor();
        }
        
    }
    private void OpenDoor()
    {
        if (canOpen)
        {
            barrier.SetActive(false);
        }
    }
}
