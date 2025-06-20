using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public DaveStats stats;
    private Rigidbody rb;
    private Vector3 moveDirection;


    void Start()
    {
        stats = GetComponent<DaveStats>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get raw input once per frame (avoids jitter)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;
    }

    void FixedUpdate()
    {
        float currentSpeed = stats.speed;

        // Sprint check
        if (Input.GetKey(KeyCode.LeftShift) && stats.stamina > 0)
        {
            stats.isSprinting = true;
            currentSpeed *= 1.5f;
            stats.stamina -= stats.staminaDrainPerSecond * Time.fixedDeltaTime;
        }
        else
        {
            stats.isSprinting = false;
        }

        // Stamina regen when not sprinting
        if (!stats.isSprinting && stats.stamina < stats.maxStamina)
        {
            stats.stamina += stats.staminaRegenRate * Time.fixedDeltaTime;
            stats.stamina = Mathf.Min(stats.stamina, stats.maxStamina);
        }

        // Move using Rigidbody
        Vector3 newPosition = rb.position + moveDirection * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }


}

