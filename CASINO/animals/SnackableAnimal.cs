using UnityEngine;

public class SnackableAnimal : MonoBehaviour, IBiteable
{
    public int maxHealth = 1;
    private int currentHealth;
    private bool hasBeenEaten = false;
    public SnackSpawner spawner;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void OnBitten(int damage)
    {
        if (hasBeenEaten) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            GetEaten();
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

    private void GetEaten()
    {
        hasBeenEaten = true;

        int money = Random.Range(40, 140);
        Debug.Log($"Dave ate a snack and stole ₵{money}");

        DaveStats daveStats = FindFirstObjectByType<DaveStats>();
        if (daveStats != null)
        {
            daveStats.money += money;
        }

        if (spawner != null)
        {
            spawner.NotifySnackEaten(gameObject);
        }

        Destroy(gameObject);
    }
}
