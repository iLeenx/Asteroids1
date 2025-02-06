using UnityEngine;

public class DontDestroyOnLoadSingleton : MonoBehaviour
{
    private static DontDestroyOnLoadSingleton instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // Destroy duplicate instance
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
