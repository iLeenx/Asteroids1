using UnityEngine;
using System.Collections.Generic;
using VInspector;
using System.Data.Common;
using System;

public class AddEffectsOnCollider2D : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("List of elements. Each element can specify prefabs, audios, shakes, and event types.")]
    [SerializeField]
    public List<Juice> effect = new List<Juice>();

    [Foldout("Physics")]
    public bool isVelocityBased = false;

    [Header("Velocity Threshold")]
    [Tooltip("The minimum velocity required to instantiate effects. Ignored if Rigidbody2D is not assigned.")]
    [EnableIf("isVelocityBased")]
    public float velocityThreshold = 10f;

    [Tooltip("Optional: The Rigidbody2D to use for velocity calculations.")]
    public Rigidbody2D Rigidbody;

    [System.Serializable]
    public class VFX
    {
        //public OB_VFX.PrefabType prefabType; // Type of prefab to instantiate
        public string effectName; // Name of the audio clip

        [Variants("At Self", "At Collision Point", "At Collided Object")]
        public string spawn = "At Self"; // Spawn location for the prefab
        public Vector3 offset = Vector3.zero; // Offset from the spawn position
        public Quaternion rotation = Quaternion.identity; // Rotation of the prefab
        public float scaleMultiplier = 1f; // Scale multiplier for the prefab
        public float cooldown = 0.1f; // Cooldown for instantiating the prefab
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
    public class HUDFX
    {
        // do effects
    }

    [System.Serializable]
    public class PostProcessingFX
    {
        // do effects
    }

    [System.Serializable]
    public class Juice
    {
        public enum EventType { Enter, Stay, Exit }

        public bool DisableObject = false;
        public bool DestroyObject = false;

        public bool useTrigger = false; // Whether to use trigger events instead of collision events

        //public string SpecificTag;

        public EventType eventType = EventType.Enter; // Event type: Enter, Stay, Exit

        public List<VFX> VFX = new List<VFX>(); // List of prefabs to instantiate

        public List<SFX> SFX = new List<SFX>(); // List of audios to play

        public List<CameraShake> CameraShake = new List<CameraShake>(); // List of shakes to trigger

        public List<HUDFX> HUDFX = new List<HUDFX>(); // List of shakes to trigger

        public List<PostProcessingFX> PostProcessingFX = new List<PostProcessingFX>(); // List of shakes to trigger
    }

    private void Start()
    {
        if (isVelocityBased && Rigidbody == null)
        {
            Rigidbody = GetComponent<Rigidbody2D>();

            if (Rigidbody == null)
            {
                Debug.LogWarning("Velocity-based checks are enabled, but no Rigidbody2D is assigned or found on the GameObject. Velocity checks will not work.");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollisionOrTrigger(collision, null, Juice.EventType.Enter, isTrigger: false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HandleCollisionOrTrigger(collision, null, Juice.EventType.Stay, isTrigger: false);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        HandleCollisionOrTrigger(collision, null, Juice.EventType.Exit, isTrigger: false);
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        HandleCollisionOrTrigger(null, trigger, Juice.EventType.Enter, isTrigger: true);
    }

    private void OnTriggerStay2D(Collider2D trigger)
    {
        HandleCollisionOrTrigger(null, trigger, Juice.EventType.Stay, isTrigger: true);
    }

    private void OnTriggerExit2D(Collider2D trigger)
    {
        HandleCollisionOrTrigger(null, trigger, Juice.EventType.Exit, isTrigger: true);
    }

    private void HandleCollisionOrTrigger(Collision2D collision, Collider2D trigger, Juice.EventType eventType, bool isTrigger)
    {
        foreach (var element in effect)
        {
            if (element.useTrigger == isTrigger && element.eventType == eventType)
            {
                bool velocityCheckPassed = !isVelocityBased || (Rigidbody != null && Rigidbody.linearVelocity.magnitude >= velocityThreshold);

                if (velocityCheckPassed)
                {
                    if (isTrigger && trigger != null)
                    {
                        DoPrefabs(element.VFX, trigger.transform.position, trigger.transform);
                    }
                    else if (collision != null)
                    {
                        DoPrefabs(element.VFX, collision.contacts[0].point, collision.transform);
                    }

                    PlayAudios(element.SFX);
                    DoShakes(element.CameraShake);
                }

                // Handle object disabling or destruction for this specific Juice element
                if (element.DisableObject) gameObject.SetActive(false);
                else if (element.DestroyObject) Destroy(gameObject);
            }
        }
    }

    //private void HandleCollisionOrTrigger(Collision2D collision, Collider2D trigger, Juice.EventType eventType, bool isTrigger)
    //{
    //    foreach (var element in effect)
    //    {
    //        // Check if the event type and trigger/collision match the Juice configuration
    //        if (element.useTrigger == isTrigger && element.eventType == eventType)
    //        {
    //            // Check if the collision/trigger object has a tag matching the specific tag
    //            bool tagMatch = true; // Default to true if SpecificTag is empty or null
    //            if (!string.IsNullOrEmpty(element.SpecificTag))
    //            {
    //                if (isTrigger && trigger != null)
    //                {
    //                    tagMatch = trigger.CompareTag(element.SpecificTag);
    //                }
    //                else if (collision != null)
    //                {
    //                    tagMatch = collision.collider.CompareTag(element.SpecificTag);
    //                }
    //            }

    //            // Proceed only if the tag matches
    //            if (!tagMatch) continue;

    //            // Check velocity requirements if enabled
    //            bool velocityCheckPassed = !isVelocityBased || (Rigidbody != null && Rigidbody.linearVelocity.magnitude >= velocityThreshold);

    //            if (velocityCheckPassed)
    //            {
    //                if (isTrigger && trigger != null)
    //                {
    //                    DoPrefabs(element.VFX, trigger.transform.position, trigger.transform);
    //                }
    //                else if (collision != null)
    //                {
    //                    DoPrefabs(element.VFX, collision.contacts[0].point, collision.transform);
    //                }

    //                PlayAudios(element.SFX);
    //                DoShakes(element.CameraShake);
    //            }

    //            // Handle object disabling or destruction for this specific Juice element
    //            if (element.DisableObject) gameObject.SetActive(false);
    //            else if (element.DestroyObject) Destroy(gameObject);
    //        }
    //    }
    //}


    private void DoPrefabs(List<VFX> prefabs, Vector3 collisionPoint, Transform collidedTransform)
    {
        foreach (var prefab in prefabs)
        {
            Vector3 spawnPosition = transform.position; // Default to self position

            if (prefab.spawn == "At Collision Point")
            {
                spawnPosition = collisionPoint;
            }
            else if (prefab.spawn == "At Collided Object")
            {
                spawnPosition = collidedTransform.position;
            }

            spawnPosition += prefab.offset;

            if (OB_VFX.Instance != null)
            {
                OB_VFX.Instance.Play(
                    prefab.effectName,
                    spawnPosition,
                    prefab.rotation,
                    prefab.cooldown,
                    prefab.scaleMultiplier
                );
            }
            else
            {
                Debug.LogWarning("PrefabManager instance not found.");
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
}





//// Copyright ABDULLAH ABDULBARI YOUSEF 2025. All rights reserved.

//using UnityEngine;
//using System.Collections.Generic;
//using VInspector;
//using Unity.VisualScripting;

//public class BallCollisionEffect : MonoBehaviour
//{
//    [System.Serializable]
//    public class EffectSettings
//    {
//        [Tooltip("The type of prefab to instantiate on collision.")]
//        public PrefabManager.PrefabType effectPrefab = PrefabManager.PrefabType.Impact;

//        [Tooltip("The cooldown time before the effect can be instantiated again.")]
//        public float cooldown = 0.1f;

//        [Tooltip("The size of the instantiated effect.")]
//        public float size = 1f;

//        [Variants("At Self", "At Collision Point", "At Collided Object")]
//        public string spawn;
//    }

//    [Header("Settings")]
//    [Space(15)]
//    public List<EffectSettings> effects = new List<EffectSettings>();

//    [Foldout("Camera Shake")]
//    public bool cameraShake = false;

//    [EnableIf("cameraShake")]
//    [Variants("2D", "3D")]
//    public string shakeType;
//    public float shakeIntensity = 1f;
//    public float shakeDuration = 1f;
//    public float shakeCooldown = 0.1f;
//    [EndIf]

//    [Foldout("Physics")]
//    public bool isVelocityBased = false;

//    [Header("Velocity Threshold")]
//    [Tooltip("The minimum velocity required to instantiate effects. Ignored if Rigidbody2D is not assigned.")]
//    [EnableIf("isVelocityBased")]
//    public float velocityThreshold = 10f;

//    [Tooltip("Optional: The Rigidbody2D to use for velocity calculations.")]
//    public Rigidbody2D Rigidbody;

//    private void Start()
//    {
//        if (isVelocityBased && Rigidbody == null)
//        {
//            Rigidbody = GetComponent<Rigidbody2D>();

//            if (Rigidbody == null)
//            {
//                Debug.LogWarning("Velocity-based checks are enabled, but no Rigidbody2D is assigned or found on the GameObject. Velocity checks will not work.");
//            }
//        }
//        else if (!isVelocityBased && Rigidbody != null)
//        {
//            Debug.LogWarning("Rigidbody2D is assigned but velocity-based checks are disabled. The Rigidbody will not be used.");
//        }
//    }


//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        DoEffect(collision);
//    }

//    [Button]
//    private void DoEffect(Collision2D collision)
//    {
//        //if (cameraShake == true) ShakeCamera();

//        // Calculate velocity if Rigidbody2D exists; otherwise default to true (always allow effects)
//        bool velocityCheckPassed = Rigidbody == null || Rigidbody.linearVelocity.magnitude >= velocityThreshold;

//        if (velocityCheckPassed)
//        {
//            // Loop through all effects and instantiate them
//            foreach (var effect in effects)
//            {
//                // Determine the position for the effect
//                Vector3 effectPosition = transform.position; // Default to ball's position

//                if (effect.spawn == "At Self")
//                {
//                    // Use the collision point
//                    effectPosition = transform.position;
//                }
//                else if (effect.spawn == "At Collision Point" && collision.contacts.Length > 0)
//                {
//                    // Use the collision point
//                    effectPosition = collision.contacts[0].point;
//                }
//                else if (effect.spawn == "At Collided Object")
//                {
//                    // Use the position of the collided object
//                    effectPosition = collision.transform.position;
//                }

//                // Instantiate the effect at the determined position
//                PrefabManager.Instance.InstantiatePrefab(effect.effectPrefab, effectPosition, Quaternion.identity, effect.cooldown, effect.size);
//                if (cameraShake == true) ShakeCamera();

//                Debug.Log($"Effect instantiated: {effect.effectPrefab} at " +
//                          $"{(effect.spawn == "At Collision Point" ? "collision point" : effect.spawn == "At Collided Object" ? "collided object's position" : "ball position")}.");
//            }
//        }
//        else
//        {
//            Debug.Log("Velocity threshold not met. No effects instantiated.");
//        }
//    }

//    private void ShakeCamera()
//    {
//        if (shakeType == "2D") ShakeManager.Instance.Shake2D(shakeIntensity, shakeDuration, shakeCooldown);
//        if (shakeType == "3D") ShakeManager.Instance.Shake3D(shakeIntensity, shakeDuration, shakeCooldown);
//    }
//}
