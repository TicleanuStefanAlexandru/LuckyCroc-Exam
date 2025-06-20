using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class SnackSpawner : MonoBehaviour
{
    public GameObject snackPrefab;
    public float spawnRadius = 10f;
    public float spawnInterval = 5f; // Time delay between spawns
    public int maxSnackCount = 30;

    private float spawnTimer;
    private Queue<GameObject> pendingSpawns = new Queue<GameObject>();
    private int currentSnackCount = 0;

    void Start()
    {
        // Optional: spawn some snacks at start
        for (int i = 0; i < maxSnackCount; i++)
        {
            SpawnSnack();
        }
    }

    void Update()
    {
        if (pendingSpawns.Count > 0)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0f)
            {
                spawnTimer = spawnInterval;
                TrySpawnFromQueue();
            }
        }
    }

    public void NotifySnackEaten(GameObject snack)
    {
        currentSnackCount = Mathf.Max(0, currentSnackCount - 1);
        pendingSpawns.Enqueue(snackPrefab);
    }

    void TrySpawnFromQueue()
    {
        if (currentSnackCount >= maxSnackCount) return;

        GameObject snackToSpawn = pendingSpawns.Dequeue();
        SpawnSnack(snackToSpawn);
    }

    void SpawnSnack()
    {
        SpawnSnack(snackPrefab);
    }

    void SpawnSnack(GameObject prefab)
    {
        Vector3 randomPos = transform.position + Random.insideUnitSphere * spawnRadius;
        randomPos.y += 10f;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPos, out hit, 20f, NavMesh.AllAreas))
        {
            GameObject snack = Instantiate(prefab, hit.position, Quaternion.identity);

            // Assign this spawner to the snack so it can notify when eaten
            SnackableAnimal snackScript = snack.GetComponent<SnackableAnimal>();
            if (snackScript != null)
            {
                snackScript.spawner = this;
            }

            currentSnackCount++;
        }
        else
        {
            Debug.LogWarning("Failed to find NavMesh position for snack spawn.");
        }
    }
}
