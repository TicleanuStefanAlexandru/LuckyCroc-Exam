using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Load the main game scene (change to your scene name or index)
        SceneManager.LoadScene("Dave's Apartment");
    }

    public void ExitGame()
    {
        Debug.Log("Exiting...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // For editor testing
#endif
    }
}
