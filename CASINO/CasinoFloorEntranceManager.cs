using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CasinoFloorEntranceManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject entrancePopupPanel;
    public Text floorText;
    public Button enterButton;
    public GameObject feedbackPanel;
    public Text feedbackText;

    private DaveStats daveStats;
    private System.Action currentEnterMethod;
    private string currentFloorName;

    // Floor names and their money requirements
    private readonly System.Collections.Generic.Dictionary<string, int> floorRequirements = new()
    {
        {"Beggar's Pit", 0},
        {"The Rabbit's Field", 1000},
        {"The Wolf's Den", 10000},
        {"The Lion's Arena", 100000}
    };

    private void Start()
    {
        entrancePopupPanel.SetActive(false);
        feedbackPanel.SetActive(false);
        enterButton.onClick.RemoveAllListeners();
        daveStats = FindFirstObjectByType<DaveStats>();
    }

    private void ShowEntrancePopup(string floorName)
    {
        int requiredMoney = floorRequirements.ContainsKey(floorName) ? floorRequirements[floorName] : 0;

        if (requiredMoney == 0)
            floorText.text = $"Enter: {floorName}?";
        else
            floorText.text = $"Enter: {floorName}? (min. ₵{requiredMoney:N0} entry)";

        entrancePopupPanel.SetActive(true);
    }

    public void HideEntrancePopup()
    {
        entrancePopupPanel.SetActive(false);
    }

    // Called by trigger script to assign method and show popup
    public void SetEnterMethod(System.Action enterMethod, string floorName)
    {
        currentEnterMethod = enterMethod;
        currentFloorName = floorName;

        enterButton.onClick.RemoveAllListeners();
        enterButton.onClick.AddListener(() =>
        {
            currentEnterMethod?.Invoke();
            HideEntrancePopup();
        });

        ShowEntrancePopup(floorName);
    }

    public void EnterBeggarsPit()
    {
        EnterFloor("Beggar's Pit", "Beggar's Pit");
    }

    public void EnterRabbitsField ()
    {
        int requiredMoney = floorRequirements["The Rabbit's Field"];
        if (daveStats.money < requiredMoney)
        {
            ShowFeedback($"Security: You need ₵{requiredMoney:N0} to enter The Rabbit's Field.");
            return;
        }
        EnterFloor("The Rabbit's Field", "The Rabbit's Field");
    }

    public void EnterWolfsDen()
    {
        int requiredMoney = floorRequirements["The Wolf's Den"];
        if (daveStats.money < requiredMoney)
        {
            ShowFeedback($"Security: You need ₵{requiredMoney:N0} to enter The Wolf's Den.");
            return;
        }
        EnterFloor("The Wolf's Den", "The Wolf's Den");
    }

    public void EnterLionsArena()
    {
        int requiredMoney = floorRequirements["The Lion's Arena"];
        if (daveStats.money < requiredMoney)
        {
            ShowFeedback($"Security: You need ₵{requiredMoney:N0} to enter The Lion's Arena.");
            return;
        }
        EnterFloor("The Lion's Arena", "The Lion's Arena");
    }

    private void EnterFloor(string floorName, string sceneName)
    {
        Debug.Log($"Entering {floorName}...");
        SceneManager.LoadScene(sceneName); // Uses actual Unity scene name
    }

    private void ShowFeedback(string message)
    {
        if (string.IsNullOrEmpty(message)) return;

        feedbackText.text = message;
        feedbackPanel.SetActive(true);
        CancelInvoke(nameof(HideFeedback));
        Invoke(nameof(HideFeedback), 3f);
    }

    private void HideFeedback()
    {
        feedbackPanel.SetActive(false);
    }
}
