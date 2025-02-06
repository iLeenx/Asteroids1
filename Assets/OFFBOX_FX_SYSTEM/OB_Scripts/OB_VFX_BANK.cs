// Copyright ABDULLAH ABDULBARI YOUSEF 2025. All rights reserved.

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;
using System;

//[ExecuteInEditMode]
public class OB_VFX_BANK : MonoBehaviour
{
    public static OB_VFX_BANK instance;

    [Header("All VFX Prefabs (Auto-Synced)")]
    [Tooltip("This list is automatically populated from the Assets/VFX folder.")]
    public List<GameObject> vfxPrefabs = new List<GameObject>();

    private readonly Dictionary<string, GameObject> vfxDictionary = new Dictionary<string, GameObject>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
            InitializeVFXDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Auto-sync VFX prefabs from Assets/VFX in the Editor.
    /// </summary>
    private void OnValidate()
    {
        SyncVFXPrefabs();
    }

    private void SyncVFXPrefabs()
    {
        vfxPrefabs.Clear();
        vfxDictionary.Clear();

        string vfxFolder = "Assets/OFFBOX_FX_SYSTEM/OB_Resources/VFX_Library";
        string[] vfxPaths = AssetDatabase.FindAssets("t:GameObject", new[] { vfxFolder });

        foreach (string guid in vfxPaths)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab != null && !vfxDictionary.ContainsKey(prefab.name))
            {
                vfxDictionary[prefab.name] = prefab;
                vfxPrefabs.Add(prefab);
            }
            else if (prefab == null)
            {
                Debug.LogWarning($"Failed to load VFX prefab at path: {path}");
            }
            else
            {
                Debug.LogWarning($"Duplicate VFX prefab name detected: {prefab.name}. Ignoring duplicate.");
            }
        }

        Debug.Log($"Synced {vfxPrefabs.Count} VFX prefabs from {vfxFolder}.");
    }
#endif

    private void InitializeVFXDictionary()
    {
        foreach (var prefab in vfxPrefabs)
        {
            if (prefab != null && !vfxDictionary.ContainsKey(prefab.name))
            {
                vfxDictionary[prefab.name] = prefab;
            }
        }
    }

    /// <summary>
    /// Get a VFX prefab by its name.
    /// </summary>
    public GameObject GetVFXPrefab(string name)
    {
        if (vfxDictionary.TryGetValue(name, out GameObject prefab))
        {
            return prefab;
        }
        Debug.LogWarning($"VFX '{name}' not found in VFX Bank.");
        return null;
    }
}
