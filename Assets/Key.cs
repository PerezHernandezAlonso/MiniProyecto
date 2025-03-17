using UnityEngine;

public class Key : MonoBehaviour
{
    public Door door;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.canOpen = true;
            gameObject.SetActive(false);
        }
    }
}
