// Copyright ABDULLAH ABDULBARI YOUSEF 2025. All rights reserved.

using UnityEngine;
using System;
using System.Collections.Generic;
using VInspector;

public class OB_VFX : MonoBehaviour
{
    public static OB_VFX Instance { get; private set; }

    private Dictionary<string, float> lastInstantiationTimes = new Dictionary<string, float>();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    // Play VFX by Name
    public GameObject Play(string vfxName, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = OB_VFX_BANK.instance.GetVFXPrefab(vfxName);
        if (prefab != null)
        {
            return Instantiate(prefab, position, rotation);
        }
        else
        {
            Debug.LogError($"VFX '{vfxName}' not found in VFX Bank.");
            return null;
        }
    }

    // Play VFX with Cooldown
    public GameObject Play(string vfxName, Vector3 position, Quaternion rotation, float cooldown)
    {
        GameObject prefab = OB_VFX_BANK.instance.GetVFXPrefab(vfxName);
        if (prefab != null)
        {
            float currentTime = Time.time;
            if (!lastInstantiationTimes.ContainsKey(vfxName) || currentTime - lastInstantiationTimes[vfxName] >= cooldown)
            {
                lastInstantiationTimes[vfxName] = currentTime;
                return Instantiate(prefab, position, rotation);
            }
            else
            {
                return null; // Cooldown active
            }
        }
        else
        {
            Debug.LogError($"VFX '{vfxName}' not found in VFX Bank.");
            return null;
        }
    }

    // Play VFX with Cooldown + Scale
    public GameObject Play(string vfxName, Vector3 position, Quaternion rotation, float cooldown, float scaleMultiplier = 1.0f)
    {
        GameObject prefab = OB_VFX_BANK.instance.GetVFXPrefab(vfxName);
        if (prefab != null)
        {
            float currentTime = Time.time;
            if (!lastInstantiationTimes.ContainsKey(vfxName) || currentTime - lastInstantiationTimes[vfxName] >= cooldown)
            {
                lastInstantiationTimes[vfxName] = currentTime;

                GameObject vfxInstance = Instantiate(prefab, position, rotation);
                vfxInstance.transform.localScale *= scaleMultiplier;
                return vfxInstance;
            }
            else
            {
                return null;
            }
        }
        else
        {
            Debug.LogError($"VFX '{vfxName}' not found in VFX Bank.");
            return null;
        }
    }

    //// Play VFX with Cooldown + Scale + Color
    //[Button("Test Play()")]
    //public GameObject Play(string vfxName, Vector3 position, Quaternion rotation, float cooldown, float scaleMultiplier)
    //{
    //    GameObject prefab = OB_VFX_BANK.instance.GetVFXPrefab(vfxName);
    //    if (prefab != null)
    //    {
    //        float currentTime = Time.time;
    //        if (!lastInstantiationTimes.ContainsKey(vfxName) || currentTime - lastInstantiationTimes[vfxName] >= cooldown)
    //        {
    //            lastInstantiationTimes[vfxName] = currentTime;

    //            GameObject vfxInstance = Instantiate(prefab, position, rotation);
    //            vfxInstance.transform.localScale *= scaleMultiplier;

    //            return vfxInstance;
    //        }
    //        else
    //        {
    //            return null;
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError($"VFX '{vfxName}' not found in VFX Bank.");
    //        return null;
    //    }
    //}

    // Play VFX with Cooldown + Scale + Color + Auto-Destruction
    [Button("Test PlayVFX()")]
    public GameObject PlayVFX(string vfxName, Vector3 position, Quaternion rotation, float cooldown, float scaleMultiplier, float destroyDelay)
    {
        GameObject prefab = OB_VFX_BANK.instance.GetVFXPrefab(vfxName);
        if (prefab == null)
        {
            Debug.LogError($"VFX '{vfxName}' not found in VFX Bank.");
            return null;
        }

        float currentTime = Time.time;
        if (lastInstantiationTimes.TryGetValue(vfxName, out float lastTime) && currentTime - lastTime < cooldown)
        {
            return null; // Cooldown not met, prevent instantiation
        }

        lastInstantiationTimes[vfxName] = currentTime;

        // Instantiate VFX
        GameObject vfxInstance = Instantiate(prefab, position, rotation);
        vfxInstance.transform.localScale *= scaleMultiplier;

        // Schedule destruction after 'destroyDelay' seconds
        if (destroyDelay > 0)
        {
            Destroy(vfxInstance, destroyDelay);
        }

        return vfxInstance;
    }
}
