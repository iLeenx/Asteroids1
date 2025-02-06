using UnityEngine;
using System.Collections.Generic;
using VInspector;

[System.Serializable]
public class PostFXSequence
{
    public string effectName;
    public float duration = 1.0f;
    public AnimationCurve weightCurve = AnimationCurve.Linear(0, 0, 1, 1); // Default linear curve
}

public static class OB_POSTFX_Helper
{
    public static void PlayFX(this GameObject obj)
    {
        obj.GetComponent<OB_POSTFX_SEQUENCE>()?.PlaySequence();
    }
}


public class OB_POSTFX_SEQUENCE : MonoBehaviour
{
    [Header("PostFX Sequence")]
    [Tooltip("Define the sequence of effects with their animation curves.")]
    public List<PostFXSequence> effectSequence = new List<PostFXSequence>();

    [Button]
    public void PlaySequence()
    {
        if (effectSequence.Count == 0)
        {
            Debug.LogWarning($"[{gameObject.name}] No PostFX sequence defined.");
            return;
        }

        OB_POSTFX.Instance.TriggerEffectSequence(effectSequence);
    }
}
