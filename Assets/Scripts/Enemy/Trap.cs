using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    GameManager gameManager;
    public int contactDamage = 10;
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colisi�n detectada con "+ other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            makeDamage();
        }else if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Health>().TakeDamage(10);
        }
    }
    public void makeDamage()
    {
        GameManager.Singleton.Player.TakeDamage(contactDamage);
        Debug.Log("Da�o realizado");
    }
    
}
