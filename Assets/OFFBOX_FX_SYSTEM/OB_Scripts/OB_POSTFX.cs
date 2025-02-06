using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;
using VInspector;

public class OB_POSTFX : MonoBehaviour
{
    public static OB_POSTFX Instance { get; private set; }

    [Header("Pooled PostFX Instances")]
    private Dictionary<string, Volume> effectPool = new Dictionary<string, Volume>();

    private Dictionary<Volume, float> activeEffects = new Dictionary<Volume, float>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeEffectPool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        List<Volume> toDisable = new List<Volume>();

        foreach (var effect in new List<KeyValuePair<Volume, float>>(activeEffects))
        {
            activeEffects[effect.Key] -= Time.deltaTime;
            if (activeEffects[effect.Key] <= 0f)
            {
                toDisable.Add(effect.Key);
            }
        }

        foreach (var volume in toDisable)
        {
            DeactivateEffect(volume);
            activeEffects.Remove(volume);
        }
    }

    private void InitializeEffectPool()
    {
        foreach (var prefab in OB_POSTFX_BANK.Instance.postFxPrefabs)
        {
            if (prefab != null && !effectPool.ContainsKey(prefab.name))
            {
                GameObject instance = Instantiate(prefab, transform);
                Volume volume = instance.GetComponent<Volume>();

                if (volume != null)
                {
                    volume.enabled = false; // Keep disabled until needed
                    effectPool[prefab.name] = volume;
                }
                else
                {
                    Debug.LogWarning($"PostFX '{prefab.name}' is missing a Volume component.");
                }
            }
        }
    }

    /// <summary>
    /// Triggers a post-processing effect from the pool.
    /// </summary>
    public void TriggerEffect(string effectName, float duration, float weight)
    {
        if (!effectPool.ContainsKey(effectName))
        {
            Debug.LogError($"PostFX '{effectName}' not found in the pool.");
            return;
        }

        Volume volume = effectPool[effectName];

        volume.enabled = true;
        volume.weight = weight;

        activeEffects[volume] = duration;
    }

    private void DeactivateEffect(Volume volume)
    {
        if (volume == null) return;

        volume.weight = 0f;
        volume.enabled = false;
    }

    /// <summary>
    /// Triggers a sequence of post-processing effects.
    /// </summary>
    public void TriggerEffectSequence(List<PostFXSequence> sequence)
    {
        StartCoroutine(SequenceEffects(sequence));
    }

    private IEnumerator SequenceEffects(List<PostFXSequence> sequence)
    {
        foreach (var effect in sequence)
        {
            yield return StartCoroutine(AnimateEffect(effect));
        }
    }

    private IEnumerator AnimateEffect(PostFXSequence effect)
    {
        if (!effectPool.ContainsKey(effect.effectName))
        {
            Debug.LogError($"PostFX '{effect.effectName}' not found in the pool.");
            yield break;
        }

        Volume volume = effectPool[effect.effectName];
        volume.enabled = true;

        float elapsedTime = 0f;

        while (elapsedTime < effect.duration)
        {
            float normalizedTime = elapsedTime / effect.duration;
            volume.weight = effect.weightCurve.Evaluate(normalizedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        volume.weight = 0f;
        volume.enabled = false;
    }
}
