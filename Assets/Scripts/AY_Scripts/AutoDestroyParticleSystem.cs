using UnityEngine;

public class AutoDestroyParticleSystem : MonoBehaviour
{
    private ParticleSystem ps;

    [SerializeField]
    private bool waitForParticlesToEnd = true; // Whether to wait for particle system to end before starting countdown

    [SerializeField]
    private float destroyDelay = 0f; // Delay before destruction, editable in Unity Editor

    [SerializeField]
    private bool enableDominantTimer = false; // Enables a dominant timer that forces destruction

    [SerializeField]
    private float dominantDuration = 5f; // Duration after which the GameObject is forcefully destroyed

    void Start()
    {
        ps = GetComponent<ParticleSystem>();

        if (enableDominantTimer)
        {
            // Invoke the forceful destruction after the dominant duration, regardless of particle status
            Invoke("ForcefulDestroy", dominantDuration);
        }
    }

    void Update()
    {
        // Check if the particle system is not null and not already invoking a delayed destroy
        if (ps != null && !IsInvoking("DelayedDestroy"))
        {
            // Check the status of particles and the delay destroy flag
            if (!ps.IsAlive())
            {
                if (waitForParticlesToEnd)
                {
                    // Invoke the destruction method with a delay if waiting for particles to end
                    Invoke("DelayedDestroy", destroyDelay);
                }
                else
                {
                    // Invoke the destruction method immediately
                    Invoke("DelayedDestroy", 0f);
                }
            }
        }
    }

    void DelayedDestroy()
    {
        // Check if the GameObject hasn't been destroyed yet by the dominant timer
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    void ForcefulDestroy()
    {
        // Forcefully destroy the GameObject, overriding other conditions
        Destroy(gameObject);
    }
}
