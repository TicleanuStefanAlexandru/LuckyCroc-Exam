using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathSceneManager : MonoBehaviour
{
    public Button retryButton;
    public string apartmentSceneName = "Dave's Apartment";

    void Start()
    {
        retryButton.onClick.AddListener(RetryDay);
    }

    void RetryDay()
    {
        // Find Dave in the scene
        DaveStats dave = FindObjectOfType<DaveStats>();
        if (dave != null)
        {
            dave.ResetToDefault();
        }

        // Load the apartment scene
        SceneManager.LoadScene(apartmentSceneName);
    }
}
