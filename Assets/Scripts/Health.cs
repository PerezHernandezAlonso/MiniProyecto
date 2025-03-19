using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public GameObject LevelManager;
    public bool isTarget = false;
    public bool isTrap = false;
    public GameObject drop;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " ha recibido " + damage + " de daño. Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " ha muerto.");
       if (LevelManager != null)
        {
            if (LevelManager.name == "Level1Manager")
            {
                Level1Manager level1manager = LevelManager.GetComponent<Level1Manager>();
                if (isTrap)
                {
                    level1manager.ActiveTraps--;
                    if (level1manager.ActiveTraps <= 0)
                    {
                        level1manager.EnableSecondDoor();
                    }

                }
                else if (isTarget)
                {
                    level1manager.ActiveTargets--;
                    if (level1manager.ActiveTargets <= 0)
                    {
                        level1manager.EnableFirstDoor();
                    }
                }
            }
        }
       
        if (drop != null)
        {
            GameObject DroppedItem = Instantiate(drop, transform.position, transform.rotation);
        }
        Destroy(gameObject);

    }
}
