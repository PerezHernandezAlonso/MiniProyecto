using UnityEngine;

public class Heal : MonoBehaviour
{
    public int healAmount = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        GameManager.Singleton.Player.Heal(healAmount);
    }
}
