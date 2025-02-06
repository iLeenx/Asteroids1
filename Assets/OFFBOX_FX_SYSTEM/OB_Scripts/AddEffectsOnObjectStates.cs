using UnityEngine;
using System.Collections.Generic;
using VInspector;

public class AddEffectsOnObjectStates : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("List of effect elements. Each element can have prefabs, audios, and shakes.")]
    [SerializeField]
    public List<Element> elements = new List<Element>();

    [System.Serializable]
    public class VFX
    {
        //public OB_VFX.PrefabType prefabType; // Type of prefab to instantiate
        public string effectName; // Name of the audio clip
        public Vector3 offset = Vector3.zero; // Optional offset from the object's position
        public Quaternion rotation = Quaternion.identity; // Optional rotation for the effect
        public float scaleMultiplier = 1f; // Scale multiplier for the effect
    }

    [System.Serializable]
    public class SFX
    {
        public string audioName; // Name of the audio clip
        public float volume = 1f; // Volume of the audio
        public float pitchOffset = 0f; // Pitch offset for variation
        public float cooldown = 0.1f; // Cooldown between plays
    }

    [System.Serializable]
    public class CameraShake
    {
        [Variants("2D", "3D")]
        public string shakeType = "2D"; // Shake type: 2D or 3D
        public float shakeIntensity = 1f; // Intensity of the shake
        public float shakeDuration = 1f; // Duration of the shake
        public float shakeCooldown = 0.1f; // Cooldown for the shake
    }

    [System.Serializable]
    public class Element
    {
        public bool doOnEnable = true; // Trigger this element on enable
        public bool doOnDisable = false; // Trigger this element on disable

        [Header("Prefab Settings")]
        public List<VFX> vfx = new List<VFX>(); // List of prefabs to instantiate

        [Header("Audio Settings")]
        public List<SFX> sfx = new List<SFX>(); // List of audios to play

        [Header("Shake Settings")]
        public List<CameraShake> cameraShake = new List<CameraShake>(); // List of shakes to trigger
    }

    private void Start()
    {

    }

    private void OnEnable()
    {
        foreach (var element in elements)
        {
            if (element.doOnEnable)
            {
                DoPrefabs(element.vfx);
                DoShakes(element.cameraShake);
                PlayAudios(element.sfx);
            }
        }
    }

    private void OnDisable()
    {
        foreach (var element in elements)
        {
            if (element.doOnDisable)
            {
                DoPrefabs(element.vfx);
                DoShakes(element.cameraShake);
                PlayAudios(element.sfx);
            }
        }
    }

    private void OnDestroy()
    {

    }

    private void DoPrefabs(List<VFX> prefabs)
    {
        foreach (var prefab in prefabs)
        {
            if (OB_VFX.Instance != null)
            {
                OB_VFX.Instance.Play(
                    prefab.effectName,
                    transform.position + prefab.offset,
                    prefab.rotation,
                    0f, // Cooldown not used here
                    prefab.scaleMultiplier
                );
            }
            else
            {
                Debug.LogWarning("PrefabManager instance not found.");
            }
        }
    }

    private void DoShakes(List<CameraShake> shakes)
    {
        foreach (var shake in shakes)
        {
            if (ShakeManager.Instance != null)
            {
                if (shake.shakeType == "2D")
                {
                    ShakeManager.Instance.Shake2D(shake.shakeIntensity, shake.shakeDuration, shake.shakeCooldown);
                }
                else if (shake.shakeType == "3D")
                {
                    ShakeManager.Instance.Shake3D(shake.shakeIntensity, shake.shakeDuration, shake.shakeCooldown);
                }
            }
            else
            {
                Debug.LogWarning("ShakeManager instance not found.");
            }
        }
    }

    private void PlayAudios(List<SFX> audios)
    {
        foreach (var audio in audios)
        {
            if (OB_SFX.instance != null)
            {
                OB_SFX.instance.PlaySFX(
                    audio.audioName,
                    transform.position,
                    audio.volume,
                    audio.pitchOffset,
                    audio.cooldown
                );
            }
            else
            {
                Debug.LogWarning("AudioManager instance not found.");
            }
        }
    }
}
