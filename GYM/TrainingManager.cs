using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TrainingManager : MonoBehaviour
{
    [Header("Zone Triggers")]
    public GameObject strengthZone;
    public GameObject staminaZone;

    [Header("Training UI")]
    public GameObject strengthPanel;
    public GameObject staminaPanel;
    public Button strengthButton;
    public Button staminaButton;

    [Header("Feedback")]
    public GameObject feedbackPanel;
    public Text feedbackText;

    private bool hasTrainedStrength = false;
    private bool hasTrainedStamina = false;
    private DaveStats daveStats;

    void Start()
    {
        strengthPanel.SetActive(false);
        staminaPanel.SetActive(false);
        feedbackPanel.SetActive(false);

        strengthButton.onClick.AddListener(TrainStrength);
        staminaButton.onClick.AddListener(TrainStamina);

        daveStats = FindFirstObjectByType<DaveStats>();
    }

    void Update()
    {
        // Check if player is inside the strength or stamina trigger zone
        if (IsPlayerInZone(strengthZone))
        {
            strengthPanel.SetActive(true);
        }
        else
        {
            strengthPanel.SetActive(false);
        }

        if (IsPlayerInZone(staminaZone))
        {
            staminaPanel.SetActive(true);
        }
        else
        {
            staminaPanel.SetActive(false);
        }
    }

    bool IsPlayerInZone(GameObject zone)
    {
        Collider zoneCollider = zone.GetComponent<Collider>();
        Collider playerCollider = daveStats.GetComponent<Collider>();

        return zoneCollider.bounds.Intersects(playerCollider.bounds);
    }

    void TrainStrength()
    {
        if (!hasTrainedStrength)
        {
            daveStats.strength += 10;
            daveStats.maxHealth += 10;
            ShowFeedback("Dave flexes hard! +10 Strength, +10 maxHealth.");
            hasTrainedStrength = true;
        }
        else
        {
            ShowFeedback("Gustave: Go gamble already, you meathead!");
        }
    }

    void TrainStamina()
    {
        if (!hasTrainedStamina)
        {
            daveStats.maxStamina += 10;
            daveStats.speed += 0.1f;
            ShowFeedback("Dave breathes deep! +10 Stamina, +0.1 speed");
            hasTrainedStamina = true;
        }
        else
        {
            ShowFeedback("Gustave: Nothing gets the heart pumping more than gambling!");
        }
    }

    void ShowFeedback(string message)
    {
        StopAllCoroutines();
        feedbackText.text = message;
        feedbackPanel.SetActive(true);
        StartCoroutine(HideFeedbackAfterSeconds(3f));
    }

    IEnumerator HideFeedbackAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        feedbackPanel.SetActive(false);
    }

    public void ResetTraining()
    {
        hasTrainedStrength = false;
        hasTrainedStamina = false;
    }

}
