using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiLevelSelect : MonoBehaviour
{
    public GameObject popupUI;

    public string casinoSceneName = "Casino";
    public string pawnShopSceneName = "Pawn Shop";
    public string gymSceneName = "Gym";
    public string apartmentSceneName = "Dave's Apartment";

    public string casinoSpawnPointName = "CasinoEntranceSpawn";
    public string pawnShopSpawnPointName = "PawnShopEntranceSpawn";
    public string gymSpawnPointName = "GymEntranceSpawn";
    public string apartmentSpawnPointName = "ApartmentEntranceSpawn";

    public Vector3 cancelPosition = new Vector3(0f, 0f, 0f);

    private GameObject dave;
    private Rigidbody daveRb;
    private MonoBehaviour playerMovement;

    void Start()
    {
        popupUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dave = other.gameObject;
            daveRb = dave.GetComponent<Rigidbody>();
            playerMovement = dave.GetComponent<MonoBehaviour>(); // Replace with specific movement script

            if (daveRb != null)
            {
                daveRb.linearVelocity = Vector3.zero;
                daveRb.isKinematic = true;
            }

            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }

            popupUI.SetActive(true);
        }
    }

    public void GoToCasino()
    {
        SceneTransfer.targetSpawnPoint = casinoSpawnPointName;
        LoadLevel(casinoSceneName);
    }

    public void GoToPawnShop()
    {
        SceneTransfer.targetSpawnPoint = pawnShopSpawnPointName;
        LoadLevel(pawnShopSceneName);
    }

    public void GoToGym()
    {
        SceneTransfer.targetSpawnPoint = gymSpawnPointName;
        LoadLevel(gymSceneName);
    }

    public void GoToApartment()
    {
        SceneTransfer.targetSpawnPoint = apartmentSpawnPointName;
        LoadLevel(apartmentSceneName);
    }

    public void Cancel()
    {
        popupUI.SetActive(false);

        if (dave != null)
        {
            dave.transform.position = cancelPosition;
            UnfreezeDave();
        }
    }

    private void LoadLevel(string sceneName)
    {
        UnfreezeDave();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject spawn = GameObject.Find(SceneTransfer.targetSpawnPoint);
        if (spawn != null && dave != null)
        {
            dave.transform.position = spawn.transform.position;
        }

        UnfreezeDave();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void UnfreezeDave()
    {
        if (daveRb != null)
        {
            daveRb.isKinematic = false;
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }

    public static class SceneTransfer
    {
        public static string targetSpawnPoint;
    }
}
