using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public int damage = 10;

    private NavMeshAgent agent;
    private Animator animator;
    private bool canAttack = true;
    private Vector3 originalPosition;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        originalPosition = transform.position;

        // Desactivar control automático del NavMeshAgent para Root Motion
        agent.updatePosition = true;
        agent.updateRotation = true;
    }

    private void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            ReturnToOriginalPosition();
        }
        
       
        // Actualizar animaciones
        
    }

    private void ChasePlayer(float distanceToPlayer)
    {
        if (distanceToPlayer > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position); // Solo establece el camino
            if (agent.velocity.magnitude > 0.1f)
            {
                animator.SetBool("moving", true);
            }
            else
            {
                animator.SetBool("moving", false);
            }
        }
        else if (canAttack)
        {
            StartCoroutine(AttackPlayer());
        }
    }

    private IEnumerator AttackPlayer()
    {
        canAttack = false;
        agent.isStopped = true;
        animator.SetBool("attacking", true);

        Debug.Log("¡El enemigo ataca!");

        yield return new WaitForSeconds(0.5f); // Sincronizar con la animación

        if (player.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damage);
        }

        yield return new WaitForSeconds(attackCooldown - 0.5f);

        animator.SetBool("attacking", false);
        agent.isStopped = false;
        canAttack = true;
    }

    private void ReturnToOriginalPosition()
    {
        if (Vector3.Distance(transform.position, originalPosition) > 1f)
        {
            agent.isStopped = false;
            agent.SetDestination(originalPosition);
        }
        else
        {
            agent.isStopped = true;
        }
    }

    private void OnAnimatorMove()
    {
        if (animator)
        {
            Vector3 rootMotionDelta = animator.deltaPosition; // Obtener el movimiento del Root Motion
            rootMotionDelta.y = 0; // Evitar que el enemigo se mueva en el eje Y
            transform.position += rootMotionDelta; // Aplicar el movimiento manualmente
            transform.rotation = animator.rootRotation; // Aplicar la rotación de Root Motion
        }
    }
}
