using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SecurityAI2 : MonoBehaviour, IBiteable
{
    [Header("Settings")]
    public float detectionRange = 30f;
    public float attackRange = 5f;
    public float attackCooldown = 1.5f;
    public int maxHealth = 200;
    public int damage = 100;
    public float patrolDistance = 20f;
    public float patrolWaitTime = 2f;

    private int currentHealth;
    private Transform player;
    private DaveStats daveStats;
    private NavMeshAgent agent;
    private Coroutine attackCoroutine;
    private bool isPatrolling = true;
    private Vector3 startPosition;
    private Vector3 patrolTarget;

    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;
        patrolTarget = startPosition + transform.forward * patrolDistance;

        GameObject daveObj = GameObject.FindWithTag("Player");
        if (daveObj != null)
        {
            player = daveObj.transform;
            daveStats = daveObj.GetComponent<DaveStats>();
        }
        else
        {
            Debug.LogError("SecurityAI: Player not found!");
        }

        GameObject prefab = Resources.Load<GameObject>("DetectionCircle");
        if (prefab != null)
        {
            GameObject circle = Instantiate(prefab, transform);
            circle.transform.localPosition = Vector3.zero;

            var renderer = circle.GetComponent<DetectionCircleRenderer>();
            if (renderer != null)
            {
                renderer.SetRadius(detectionRange);
            }
        }

        StartCoroutine(PatrolRoutine());
    }

    void Update()
    {
        if (player == null || daveStats == null) return;

        float distanceToDave = Vector3.Distance(transform.position, player.position);

        if (daveStats.isWanted && distanceToDave <= detectionRange)
        {
            if (isPatrolling)
            {
                StopAllCoroutines();
                isPatrolling = false;
            }

            agent.SetDestination(player.position);

            if (distanceToDave <= attackRange)
            {
                if (attackCoroutine == null)
                {
                    attackCoroutine = StartCoroutine(AttackRoutine());
                }
            }
            else
            {
                if (attackCoroutine != null)
                {
                    StopCoroutine(attackCoroutine);
                    attackCoroutine = null;
                }
            }
        }
        else if (!daveStats.isWanted || distanceToDave > detectionRange * 1.5f)
        {
            if (!isPatrolling)
            {
                if (attackCoroutine != null)
                {
                    StopCoroutine(attackCoroutine);
                    attackCoroutine = null;
                }
                isPatrolling = true;
                StartCoroutine(PatrolRoutine());
            }
        }
    }

    private IEnumerator PatrolRoutine()
    {
        while (isPatrolling)
        {
            agent.SetDestination(patrolTarget);
            yield return new WaitUntil(() => agent.remainingDistance <= 0.2f);
            yield return new WaitForSeconds(patrolWaitTime);

            agent.SetDestination(startPosition);
            yield return new WaitUntil(() => agent.remainingDistance <= 0.2f);
            yield return new WaitForSeconds(patrolWaitTime);
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (daveStats != null && daveStats.Health > 0)
        {
            float distanceToDave = Vector3.Distance(transform.position, player.position);
            if (distanceToDave <= attackRange)
            {
                Debug.Log("Security attacks Dave!");
                daveStats.TakeDamage(damage);
                yield return new WaitForSeconds(attackCooldown);
            }
            else
            {
                // Pause attacking if out of range; will restart when back in range
                attackCoroutine = null;
                yield break;
            }
        }
        attackCoroutine = null;
    }

    public void OnBitten(int biteDamage)
    {
        currentHealth -= biteDamage;
        Debug.Log($"Security took {biteDamage} damage. Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;

    private void Die()
    {
        Debug.Log("Security down!");
        Destroy(gameObject);
    }
}
