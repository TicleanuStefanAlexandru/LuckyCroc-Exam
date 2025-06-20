using UnityEngine;

public class DavePersistence : MonoBehaviour
{
    private static DavePersistence instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate Daves
        }
    }
}
