using UnityEngine;

public class DaveSkills : MonoBehaviour
{
    public float biteRange = 4f;
    public float biteCooldown = 1f;
    public Transform biteOrigin;
    public LayerMask biteableLayers;

    private DaveStats stats;
    private float biteCooldownTimer;

    void Start()
    {
        stats = GetComponent<DaveStats>();
    }

    void Update()
    {
        biteCooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Alpha1) && biteCooldownTimer <= 0f)
        {
            Bite();
            biteCooldownTimer = biteCooldown;
        }
    }

    void Bite()
    {
        Collider[] hits = Physics.OverlapSphere(biteOrigin.position, biteRange, biteableLayers);
        foreach (Collider hit in hits)
        {
            IBiteable biteTarget = hit.GetComponent<IBiteable>();
            if (biteTarget != null)
            {
                int biteDamage = Mathf.RoundToInt(stats.strength * 1.5f);
                biteTarget.OnBitten(biteDamage);
                Debug.Log($"Dave bit {hit.name} for {biteDamage} damage.");

                // Check if a SecurityAI or SecurityAI2 is within detection range
                bool spotted = false;

                SecurityAI sec = FindClosestSecurity();
                if (sec != null && Vector3.Distance(transform.position, sec.transform.position) <= sec.detectionRange)
                    spotted = true;

                SecurityAI2 sec2 = FindClosestSecurity2();
                if (sec2 != null && Vector3.Distance(transform.position, sec2.transform.position) <= sec2.detectionRange)
                    spotted = true;

                if (spotted)
                {
                    stats.isWanted = true;
                    Debug.Log("Security spotted Dave biting — Wanted ON");
                }
            }
        }
    }


    // Finds the nearest SecurityAI in the scene
    SecurityAI FindClosestSecurity()
    {
        SecurityAI[] all = FindObjectsOfType<SecurityAI>();
        SecurityAI closest = null;
        float bestDist = float.MaxValue;

        foreach (SecurityAI s in all)
        {
            float d = Vector3.Distance(transform.position, s.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                closest = s;
            }
        }
        return closest;
    }

    SecurityAI2 FindClosestSecurity2()
    {
        SecurityAI2[] all = FindObjectsOfType<SecurityAI2>();
        SecurityAI2 closest = null;
        float bestDist = float.MaxValue;

        foreach (SecurityAI2 s in all)
        {
            float d = Vector3.Distance(transform.position, s.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                closest = s;
            }
        }
        return closest;
    }

    void OnDrawGizmosSelected()
    {
        if (biteOrigin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(biteOrigin.position, biteRange);
        }
    }
}
