using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        Debug.Log($"Jugador recibió {damage} de daño. Vida restante: {currentHealth}");

        if (currentHealth == 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        Debug.Log($"Jugador curado en {amount} puntos. Vida actual: {currentHealth}");
    }

    public float GetCurrentHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }

    private void Die()
    {
        Debug.Log("El jugador ha muerto.");
        // Implementar lógica de muerte (respawn, pantalla de fin, etc.)
    }
}
