#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class OB_POSTFX_BANK : MonoBehaviour
{
    public static OB_POSTFX_BANK Instance { get; private set; }

    [Header("Auto-Referenced PostFX Prefabs")]
    public List<GameObject> postFxPrefabs = new List<GameObject>();
    private Dictionary<string, GameObject> postFxDictionary = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePostFXDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        SyncPostFXPrefabs();
    }

    private void SyncPostFXPrefabs()
    {
        postFxPrefabs.Clear();
        postFxDictionary.Clear();

        string fxFolder = "Assets/OFFBOX_FX_SYSTEM/OB_Resources/POSTFX_Library";
        string[] fxPaths = AssetDatabase.FindAssets("t:GameObject", new[] { fxFolder });

        foreach (string guid in fxPaths)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab != null && !postFxDictionary.ContainsKey(prefab.name))
            {
                postFxDictionary[prefab.name] = prefab;
                postFxPrefabs.Add(prefab);
            }
        }

        Debug.Log($"Synced {postFxPrefabs.Count} PostFX prefabs from {fxFolder}.");
    }
#endif

    private void InitializePostFXDictionary()
    {
        foreach (var prefab in postFxPrefabs)
        {
            if (prefab != null && !postFxDictionary.ContainsKey(prefab.name))
            {
                postFxDictionary[prefab.name] = prefab;
            }
        }
    }

    public GameObject GetPostFXPrefab(string name)
    {
        if (postFxDictionary.TryGetValue(name, out GameObject prefab))
        {
            return prefab;
        }
        Debug.LogWarning($"PostFX '{name}' not found in the PostFX Bank.");
        return null;
    }
}
