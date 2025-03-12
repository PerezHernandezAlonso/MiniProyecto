using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;
    private ParticleSystem healCircle;

    void Awake()
    {
        healCircle = GameManager.Singleton.particles[0];
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
        GameManager.Singleton.SpawnVFX(GameManager.Singleton.particles[0], this.gameObject);
    }

    public float GetCurrentHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }

    private void Die()
    {
        Debug.Log("El jugador ha muerto.");
        Respawn();
        // Implementar lógica de muerte (respawn, pantalla de fin, etc.)
    }

    void Respawn()
    {
        Debug.Log("¡El jugador ha muerto! Respawneando...");

        Vector3 respawnPosition = CheckpointManager.Singleton.GetLastCheckpoint();
        Debug.Log("Posición de respawn: " + respawnPosition);

        if (respawnPosition != Vector3.zero) // Si hay un checkpoint guardado
        {
            CharacterController controller = GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
                transform.position = respawnPosition;
                controller.enabled = true;
            }
            currentHealth = maxHealth; // Restaurar la vida
            Debug.Log("Jugador reapareció en: " + transform.position);
        }
        else
        {
            Debug.Log("No hay checkpoints guardados, reiniciando en posición inicial.");
            transform.position = Vector3.zero; // Reinicia en la posición de inicio si no hay checkpoint
            currentHealth = maxHealth;
        }
    }
}
