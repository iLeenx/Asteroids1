using UnityEngine;
using VInspector;
using System.Collections.Generic;
using Cinemachine;

public class ShakeManager : MonoBehaviour
{
    // Singleton instance
    public static ShakeManager Instance { get; private set; }

    [Header("Shake Sources")]
    [Tooltip("Cinemachine Impulse Source for uniform shake.")]
    public CinemachineImpulseSource uniformShakeSource;

    [Tooltip("Cinemachine Impulse Source for legacy 6D shake.")]
    public CinemachineImpulseSource legacy6DShakeSource;

    // Cooldown management
    private Dictionary<string, float> cooldownTimers = new Dictionary<string, float>();

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject); // Optional: Keep it persistent between scenes
    }

    /// <summary>
    /// Triggers a uniform shake with specified intensity and duration, subject to cooldown.
    /// </summary>
    /// <param name="intensity">Intensity of the shake.</param>
    /// <param name="duration">Duration of the shake.</param>
    /// <param name="cooldown">Cooldown time for the shake.</param>
    [Button]
    public void Shake2D(
        float intensity = 1,
        float duration = 1,
        float cooldown = 0.1f
        )
    {
        if (uniformShakeSource == null)
        {
            Debug.LogWarning("Uniform shake source is not assigned!");
            return;
        }

        if (!IsCooldownComplete("Shake2D"))
        {
            Debug.Log("Shake2D is on cooldown.");
            return;
        }

        // Generate an impulse with the specified intensity
        uniformShakeSource.GenerateImpulse(intensity);
        Debug.Log($"Triggered uniform shake with intensity {intensity} for duration {duration}");

        // Start cooldown timer
        SetCooldown("Shake2D", cooldown);
    }

    /// <summary>
    /// Triggers a legacy 6D shake with specified intensity and duration, subject to cooldown.
    /// </summary>
    /// <param name="intensity">Intensity of the shake.</param>
    /// <param name="duration">Duration of the shake.</param>
    /// <param name="cooldown">Cooldown time for the shake.</param>
    [Button]
    public void Shake3D(
        float intensity = 5,
        float duration = 1,
        float cooldown = 0.1f
        )
    {
        if (legacy6DShakeSource == null)
        {
            Debug.LogWarning("Legacy 6D shake source is not assigned!");
            return;
        }

        if (!IsCooldownComplete("Shake3D"))
        {
            Debug.Log("Shake3D is on cooldown.");
            return;
        }

        // Generate an impulse with the specified intensity
        legacy6DShakeSource.GenerateImpulse(intensity);
        Debug.Log($"Triggered legacy 6D shake with intensity {intensity} for duration {duration}");

        // Start cooldown timer
        SetCooldown("Shake3D", cooldown);
    }

    /// <summary>
    /// Checks if the cooldown for a given shake type is complete.
    /// </summary>
    /// <param name="shakeType">The shake type identifier.</param>
    /// <returns>True if cooldown is complete, false otherwise.</returns>
    private bool IsCooldownComplete(string shakeType)
    {
        if (!cooldownTimers.ContainsKey(shakeType))
            return true;

        return Time.time >= cooldownTimers[shakeType];
    }

    /// <summary>
    /// Sets a cooldown timer for a given shake type.
    /// </summary>
    /// <param name="shakeType">The shake type identifier.</param>
    /// <param name="cooldown">The cooldown duration in seconds.</param>
    private void SetCooldown(string shakeType, float cooldown)
    {
        cooldownTimers[shakeType] = Time.time + cooldown;
    }
}
