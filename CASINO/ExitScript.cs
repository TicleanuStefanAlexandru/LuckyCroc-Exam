using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitZoneScript : MonoBehaviour
{
    public GameObject popupPanel; // Assign this in Inspector
    public Text popupText;        // Assign this in Inspector
    public Button exitButton;     // Assign this in Inspector
    public string targetScene;    // Name of the scene to load (e.g., "Dave's Apartment")

    void Start()
    {
        popupPanel.SetActive(false);

        if (exitButton != null)
            exitButton.onClick.AddListener(ExitToScene);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            popupText.text = "Do you want to end your gambling session and go home?";
            popupPanel.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            popupPanel.SetActive(false);
        }
    }

    void ExitToScene()
    {
        DaveStats daveStats = FindFirstObjectByType<DaveStats>();
        if (daveStats != null)
        {
            daveStats.AdvanceDay();
        }
        else
        {
            Debug.LogWarning("DaveStats not found when trying to advance the day.");
        }

        if (!string.IsNullOrEmpty(targetScene))
        {
            SceneManager.LoadScene(targetScene);
        }
    }
}
