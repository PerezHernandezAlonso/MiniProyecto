using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float rotationSpeed = 5f;
    public int damage = 10;
    public float attackCooldown = 2f;

    private float nextAttackTime = 0f;
    private bool hasDealtDamage = false;

    private Animator animator;
    private bool isMoving = false;
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.applyRootMotion = true;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool canAttack = Time.time >= nextAttackTime;

        if (distanceToPlayer <= attackRange)
        {
            if (canAttack)
            {
                isMoving = false;
                isAttacking = true;
                nextAttackTime = Time.time + attackCooldown;
                hasDealtDamage = false; // Reset para este ataque
            }
            else
            {
                isAttacking = false;
                isMoving = false;
            }
        }
        else if (distanceToPlayer <= detectionRange)
        {
            isMoving = true;
            isAttacking = false;
            RotateTowardsPlayer();
        }
        else
        {
            isMoving = false;
            isAttacking = false;
        }

        animator.SetBool("moving", isMoving);
        animator.SetBool("attacking", isAttacking);
    }

    void OnAnimatorMove()
    {
        if (isMoving && animator.applyRootMotion)
        {
            transform.position += animator.deltaPosition;
        }
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // 🔥 Esta función se llama desde la animación (evento)
    public void PerformAttack()
    {
        if (hasDealtDamage) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            GameManager.Singleton.Player.TakeDamage(damage);
            hasDealtDamage = true;
        }
    }

    // ✋ Esta función se llama al final de la animación de ataque (evento)
    public void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("attacking", false);
        hasDealtDamage = false;
        Debug.Log("Ataque terminado");
    }
}
