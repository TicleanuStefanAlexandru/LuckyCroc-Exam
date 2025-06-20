using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Predator : MonoBehaviour, IBiteable
{
    [Header("Settings")]
    public float detectionRange = 20f;
    public float attackRange = 5f;
    public float attackCooldown = 3f;
    public int maxHealth = 1000;
    public int damage = 400;

    private int currentHealth;
    private Transform player;
    private DaveStats daveStats;
    private NavMeshAgent agent;
    private bool isAttacking = false;

    private Vector3 startPosition;
    private bool goingForward = true;
    public float patrolDistance = 10f;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 10f;  // Added chase speed

    private bool isChasing = false;

    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;

        GameObject daveObj = GameObject.FindWithTag("Player");
        if (daveObj != null)
        {
            player = daveObj.transform;
            daveStats = daveObj.GetComponent<DaveStats>();
        }
        else
        {
            Debug.LogError("Predator: Player not found!");
        }
    }

    void Update()
    {
        if (player == null || daveStats == null) return;

        float distanceToDave = Vector3.Distance(transform.position, player.position);

        if (!isChasing && daveStats.money > 20000 && distanceToDave <= detectionRange)
        {
            isChasing = true;
            Debug.Log("Predator: Target acquired. Starting chase!");
        }

        if (isChasing)
        {
            if (daveStats.Health > 0)
            {
                agent.speed = chaseSpeed; // Set chase speed here
                agent.SetDestination(player.position);

                if (distanceToDave <= attackRange && !isAttacking)
                {
                    StartCoroutine(AttackRoutine());
                }
            }
            else
            {
                isChasing = false;
                agent.speed = patrolSpeed; // Reset speed to patrol when done
                agent.SetDestination(startPosition);
            }
        }
        else
        {
            agent.speed = patrolSpeed;
            Patrol();
        }
    }

    private void Patrol()
    {
        if (!agent.hasPath || agent.remainingDistance < 0.5f)
        {
            Vector3 targetPos = goingForward
                ? startPosition + transform.forward * patrolDistance
                : startPosition;

            agent.SetDestination(targetPos);

            goingForward = !goingForward;
        }
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        while (daveStats != null && daveStats.Health > 0 && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            Debug.Log("Predator attacks Dave!");
            daveStats.TakeDamage(damage);
            yield return new WaitForSeconds(attackCooldown);
        }

        isAttacking = false;
    }

    public void OnBitten(int biteDamage)
    {
        currentHealth -= biteDamage;
        Debug.Log($"Predator took {biteDamage} damage. Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;

    private void Die()
    {
        Debug.Log("Predator down!");
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
