using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SecurityAI : MonoBehaviour, IBiteable
{
    [Header("Settings")]
    public float detectionRange = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public int maxHealth = 5;
    public int damage = 1;

   
    private int currentHealth;
    private Transform player;
    private DaveStats daveStats;
    private NavMeshAgent agent;
    private bool isAttacking = false;

    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();

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

        // Instantiate and configure detection circle
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
        else
        {
            Debug.LogError("SecurityAI: DetectionCircle prefab not found in Resources!");
        }
    }

    void Update()
    {
        if (player == null || daveStats == null) return;

        float distanceToDave = Vector3.Distance(transform.position, player.position);

        // Only pursue and attack if Dave is wanted and within detection range
        if (daveStats.isWanted && distanceToDave <= detectionRange)
        {
            agent.SetDestination(player.position);

            if (distanceToDave <= attackRange && !isAttacking)
            {
                StartCoroutine(AttackRoutine());
            }
        }
        else
        {
            agent.ResetPath(); // Stop moving if Dave isn't wanted or is too far
        }
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        while (daveStats != null && daveStats.Health > 0 && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            Debug.Log("Security attacks Dave!");
            daveStats.TakeDamage(damage);
            yield return new WaitForSeconds(attackCooldown);
        }

        isAttacking = false;
    }

    // --- IBiteable Implementation ---
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
