using UnityEngine;
using UnityEngine.AI;

public class SnackWander : MonoBehaviour
{
    private NavMeshAgent agent;

    public float wanderRadius = 10f;
    public float restDuration = 3f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Prevent the agent from rotating
        agent.updateRotation = false;

        // Optional: lock physics-based rotation
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.freezeRotation = true;
        }

        Wander();
    }

    void Wander()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            float travelTime = Vector3.Distance(transform.position, hit.position) / agent.speed;
            Invoke(nameof(Rest), travelTime);
        }
        else
        {
            Invoke(nameof(Wander), 0.1f); // Try again
        }
    }

    void Rest()
    {
        agent.isStopped = true;
        Invoke(nameof(ResumeWander), restDuration);
    }

    void ResumeWander()
    {
        agent.isStopped = false;
        Wander();
    }
}
