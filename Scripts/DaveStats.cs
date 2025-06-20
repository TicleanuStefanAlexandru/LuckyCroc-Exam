using UnityEngine;
using UnityEngine.SceneManagement;

public class DaveStats : MonoBehaviour
{
    [Header("Core Stats")]
    [SerializeField] private int currentHealth = 500;
    public int maxHealth = 500;
    public float strength = 100f;
    public float speed = 10f;
    public bool isWanted = false;

    [Header("Money")]
    public int money = 10000;
    public int bankBalance = 1000;

    [Header("Stamina")]
    public float stamina = 100f;
    public float maxStamina = 100f;
    public float staminaRegenRate = 10f;
    public float staminaDrainPerSecond = 25f;
    public bool isSprinting = false;

    public int currentDay = 1;

    void Update()
    {
        RegenerateStamina();
    }

    private void RegenerateStamina()
    {
        if (!isSprinting && stamina < maxStamina)
        {
            stamina = Mathf.Clamp(stamina + staminaRegenRate * Time.deltaTime, 0f, maxStamina);
        }
    }

    public void UseStamina(float amount)
    {
        stamina = Mathf.Clamp(stamina - amount, 0f, maxStamina);
    }

    public void RecoverStamina(float amount)
    {
        stamina = Mathf.Clamp(stamina + amount, 0f, maxStamina);
    }

    public int Health => currentHealth;

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        Debug.Log($"Dave took {damage} damage, health now {currentHealth}/{maxHealth}.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Dave has died. Loading Death Scene...");
        SceneManager.LoadScene("DeathScene");
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            return true;
        }
        return false;
    }

    // 🔓 Bank functions with no limits
    public void Deposit(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            bankBalance += amount;
        }
    }

    public bool Withdraw(int amount)
    {
        if (bankBalance >= amount)
        {
            bankBalance -= amount;
            money += amount;
            return true;
        }
        return false;
    }

    public void CommitCrime()
    {
        isWanted = true;
        Debug.Log("Dave is now wanted by security!");
    }

    public void AdvanceDay()
    {
        currentDay++;
        isWanted = false;
        Debug.Log($"New day started: Day {currentDay}. Wanted status reset.");

        // Reset training
        TrainingManager trainingManager = FindFirstObjectByType<TrainingManager>();
        if (trainingManager != null)
        {
            trainingManager.ResetTraining();
        }

        // Heal Dave
        currentHealth = maxHealth;

        Debug.Log($"New day saved: Day {currentDay}. Health restored and training reset.");
    }


    public void ResetToDefault()
    {
        currentHealth = 500;
        maxHealth = 500;
        strength = 100f;
        speed = 10f;
        stamina = 100f;
        maxStamina = 100f;
        money = 10000;
        bankBalance = 1000;
        isWanted = false;
        currentDay = 1;
    }
}
