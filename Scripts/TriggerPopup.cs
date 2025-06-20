using UnityEngine;
using UnityEngine.UI; // Required for UI elements
using UnityEngine.SceneManagement; // For loading scenes if needed

public class TriggerPopup : MonoBehaviour
{
    public GameObject popup; // UI Popup to display
    public Button playButton; // Reference to the Play button
    public GameObject slotUIPanel; // Reference to the slot machine UI

    private Collider triggerCollider; // Reference to the trigger collider

    private void Start()
    {
        // Ensure popup is initially hidden
        if (popup != null)
        {
            popup.SetActive(false);
        }

        // Ensure Slot UI is initially hidden
        if (slotUIPanel != null)
        {
            slotUIPanel.SetActive(false);
        }

        // Set up Play Button listener
        if (playButton != null)
        {
            playButton.onClick.AddListener(OpenSlotUI);
        }

        triggerCollider = GetComponent<Collider>();
        if (triggerCollider == null || !triggerCollider.isTrigger)
        {
            Debug.LogError("Collider is missing or not set as Trigger on the object.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player or specific object enters the trigger
        if (other.CompareTag("Player")) // Assuming 'Player' tag for the player
        {
            ShowPopup();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HidePopup();
        }
    }

    // Show popup
    private void ShowPopup()
    {
        if (popup != null)
        {
            popup.SetActive(true);
        }
    }

    // Hide popup
    private void HidePopup()
    {
        if (popup != null)
        {
            popup.SetActive(false);
        }
    }

    // Open Slot UI when Play button is clicked
    private void OpenSlotUI()
    {
        if (slotUIPanel != null)
        {
            slotUIPanel.SetActive(true);  // Show the slot machine UI
            HidePopup();  // Hide the popup panel
        }
    }
}
