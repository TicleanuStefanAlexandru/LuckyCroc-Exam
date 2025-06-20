using UnityEngine;
using UnityEngine.UI;

public class TrainingDummy : MonoBehaviour, IBiteable
{
    public int maxHealth = 100;
    private int currentHealth;

    public int CurrentHealth => currentHealth;

    void Start()
    {
        currentHealth = maxHealth;

    }

    public void OnBitten(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0); // Avoid negative values

        Debug.Log("Dummy took " + damage + " damage. Remaining health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Destroy(gameObject); // Or play death animation
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}

