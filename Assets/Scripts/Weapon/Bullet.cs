using UnityEngine;
public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;
    public float lifetime = 3f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Comprobar si el objeto golpeado tiene un componente de salud
        Health target = collision.gameObject.GetComponent<Health>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        // Generar efectos de impacto (opcional)
        Debug.Log("Impacto de bala en: " + collision.gameObject.name);

        Destroy(gameObject);
    }
}