// Copyright ABDULLAH ABDULBARI YOUSEF 2025. All rights reserved.

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;
using System;

//[ExecuteInEditMode]
public class OB_SFX_BANK : MonoBehaviour
{
    public static OB_SFX_BANK instance;

    [Header("All Audio Clips (Auto-Synced)")]
    [Tooltip("This list is automatically populated from the Assets/Audio folder.")]
    public List<AudioClip> audioClips = new List<AudioClip>();

    private readonly Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();

    public enum AudioClipName
    {
        None // Placeholder for no sound
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
            InitializeSoundDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Auto-sync audio files from Assets/Audio and regenerate the AudioClipName enum.
    /// </summary>
    private void OnValidate()
    {
        SyncAudioFiles();
        GenerateAudioEnum();
    }

    private void SyncAudioFiles()
    {
        audioClips.Clear();
        soundDictionary.Clear();

        string audioFolder = "Assets/OFFBOX_FX_SYSTEM/OB_Resources/SFX_Library";
        string[] audioPaths = AssetDatabase.FindAssets("t:AudioClip", new[] { audioFolder });

        foreach (string guid in audioPaths)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);

            if (clip != null && !soundDictionary.ContainsKey(clip.name))
            {
                soundDictionary[clip.name] = clip;
                audioClips.Add(clip);
            }
            else if (clip == null)
            {
                Debug.LogWarning($"Failed to load audio file at path: {path}");
            }
            else
            {
                Debug.LogWarning($"Duplicate audio clip name detected: {clip.name}. Ignoring duplicate.");
            }
        }

        Debug.Log($"Synced {audioClips.Count} audio clips from {audioFolder}.");
    }

    private void GenerateAudioEnum()
    {
        string enumPath = "Assets/Scripts/Audio/AudioClipName.cs";
        List<string> enumEntries = new List<string> { "None" };

        foreach (var clip in audioClips)
        {
            if (!string.IsNullOrEmpty(clip.name))
            {
                enumEntries.Add(clip.name.Replace(" ", "_")); // Ensure valid enum names
            }
        }

        //System.IO.File.WriteAllText(enumPath, GenerateEnumCode(enumEntries));
        AssetDatabase.Refresh();
        Debug.Log($"Audio enum generated at {enumPath}");
    }

    //private string GenerateEnumCode(List<string> entries)
    //{
    //    string enumTemplate = @"// Auto-Generated Audio Clip Enum
    //namespace Game.Audio
    //{
    //    public enum AudioClipName
    //    {
    //        {0}
    //    }
    //}";
    //    return string.Format(enumTemplate, string.Join(",\n        ", entries));
    //}

    private string GenerateEnumCode(List<string> entries)
    {
        string enumTemplate = @"// Auto-Generated Audio Clip Enum
    namespace Game.Audio
    {
        public enum AudioClipName
        {
            {0}
        }
    }";

        try
        {
            // Attempt to format the string
            return string.Format(enumTemplate, string.Join(",\n        ", entries));
        }
        catch (FormatException)
        {
            // Log a simple error message and provide fallback content
            Debug.LogWarning("Failed to generate enum code due to invalid input format.");
            return @"// Auto-Generated Audio Clip Enum
namespace Game.Audio
{
    public enum AudioClipName
    {
        // Error: Could not generate enum
    }
}";
        }
    }

#endif

    private void InitializeSoundDictionary()
    {
        foreach (var clip in audioClips)
        {
            if (clip != null && !soundDictionary.ContainsKey(clip.name))
            {
                soundDictionary[clip.name] = clip;
            }
        }
    }

    public AudioClip GetAudioClip(AudioClipName clipName)
    {
        return GetAudioClip(clipName.ToString());
    }

    public AudioClip GetAudioClip(string name)
    {
        if (soundDictionary.TryGetValue(name, out AudioClip clip))
        {
            return clip;
        }
        Debug.LogWarning($"Sound '{name}' not found in SoundBank.");
        return null;
    }
}
