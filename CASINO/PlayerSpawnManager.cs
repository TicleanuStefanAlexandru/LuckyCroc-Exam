using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public GameObject player; // Assign Dave manually in Inspector if needed

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player == null)
        {
            Debug.LogError("Player object not found!");
            return;
        }

        Transform spawnPoint = GetSpawnPointByScene();

        if (spawnPoint != null)
        {
            // Disable collider or trigger on spawn point if present
            Collider col = spawnPoint.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false;
            }

            // Position Dave without changing rotation
            player.transform.position = spawnPoint.position;
        }
        else
        {
            Debug.LogWarning("No spawn point found for this scene.");
        }
    }

    Transform GetSpawnPointByScene()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        string spawnName = sceneName switch
        {
            "Beggar's Pit" => "Spawn_BeggarsPit",
            "The Rabbit's Field" => "Spawn_The Rabbit's Field",
            "The Wolf's Den" => "Spawn_WolfsDen",
            "The Lion's Arena" => "Spawn_LionsArena",
            "Dave's Apartment" => "Spawn_DavesApartment",
            "Gym" => "Spawn_Gym",
            "Casino" => "Spawn_Casino",
            "Pawn Shop" => "Spawn_PawnShop",
            _ => null
        };

        if (spawnName != null)
        {
            GameObject spawnObj = GameObject.Find(spawnName);
            if (spawnObj != null)
                return spawnObj.transform;
        }

        return null;
    }
}
