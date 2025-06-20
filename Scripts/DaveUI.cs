using UnityEngine;
using UnityEngine.UI;

public class DaveUI : MonoBehaviour
{
    public DaveStats daveStats;

    public Text healthText;
    public Text staminaText;
    public Text moneyText;
    public Text targetHealthText;
    public Text dayText; // <-- New field

    public Text wantedStatusText; // Add this

    void Update()
    {
        if (daveStats == null) return;

        healthText.text = "Health: " + daveStats.Health + "/" + daveStats.maxHealth;
        staminaText.text = "Stamina: " + Mathf.FloorToInt(daveStats.stamina) + "/" + daveStats.maxStamina;
        moneyText.text = "Money: " + daveStats.money;
        dayText.text = "Day: " + daveStats.currentDay; // <-- New line


        UpdateTargetHealth();

        // Show or hide wanted text
        wantedStatusText.gameObject.SetActive(daveStats.isWanted);

        if (daveStats.isWanted)
        {
            wantedStatusText.text = "WANTED";
        }
    }

    void UpdateTargetHealth()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Biteable")))
        {
            IBiteable biteable = hit.collider.GetComponent<IBiteable>();
            if (biteable != null)
            {
                targetHealthText.text = $"Target HP: {biteable.GetCurrentHealth()} / {biteable.GetMaxHealth()}";
                return;
            }
        }

        targetHealthText.text = "";
    }
}
