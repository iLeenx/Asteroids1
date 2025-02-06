// Copyright ABDULLAH ABDULBARI YOUSEF 2025. All rights reserved.

using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class OB_SFX : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the AudioManager.
    /// </summary>
    public static OB_SFX instance; // Singleton instance of the AudioManager.

    /// <summary>
    /// Array of audio clips that can be played randomly.
    /// </summary>
    private AudioClip[] audioClips;

    /// <summary>
    /// Tracks the last played times for each array of audio clips to manage cooldowns or prevent overlapping.
    /// </summary>
    private Dictionary<AudioClip[], float> lastPlayedTimes = new Dictionary<AudioClip[], float>();

    /// <summary>
    /// Stores the cooldown times for individual audio clips to prevent them from being replayed too frequently.
    /// </summary>
    private Dictionary<AudioClip, float> cooldowns = new Dictionary<AudioClip, float>();

    /// <summary>
    /// Maintains a mapping of looping audio clips to their respective AudioSource objects.
    /// Useful for controlling playback of looping sounds.
    /// </summary>
    private Dictionary<AudioClip, AudioSource> loopingAudioSources = new Dictionary<AudioClip, AudioSource>();

    #region Old Variables
    // Old variables
    // private AudioSource audioSource; // AudioSource component for playing audio.
    // private float audioCooldown = 1.0f; // Cooldown duration in seconds.
    // private float lastAudioTime = -1.0f; // Timestamp of the last played audio.
    #endregion 

    /// <summary>
    /// Singleton instantiation
    /// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject); // Make sure the audio manager persists between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// OVERLOAD 0 (Legacy): Plays a sound at a specified position.
    /// </summary>
    /// <param name="clip">The AudioClip to play.</param>
    /// <param name="position">The position to play the sound at.</param>
    public void PlaySound(AudioClip clip, Vector3 position)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position);
        }
    }
    /// <example>
    /// PlaySound(soundClip, new Vector3(0, 0, 0));
    /// </example>


    /// <summary>
    /// OVERLOAD 1 - Vol (Legacy): Plays a sound at a specified position with a specified volume.
    /// </summary>
    /// <param name="clip">The AudioClip to play.</param>
    /// <param name="position">The position to play the sound at.</param>
    /// <param name="volume">The volume to play the sound at.</param>
    public void PlaySound(AudioClip clip, Vector3 position, float volume)
    {
        if (clip != null)
        {
            // Create a temporary game object to hold the AudioSource
            GameObject tempGO = new GameObject("TempAudio");
            tempGO.transform.position = position;

            // Add an AudioSource to the game object
            AudioSource audioSource = tempGO.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = volume;

            // Play the clip
            audioSource.Play();

            // Destroy the temporary game object after the clip has finished playing
            Object.Destroy(tempGO, clip.length);
        }
    }
    /// <example>
    /// PlaySound(soundClip, new Vector3(0, 0, 0), 0.5f);
    /// </example>


    /// <summary>
    /// OVERLOAD 2 - Vol & Pitch (Legacy): Plays a sound at a specified position with a specified volume and pitch.
    /// </summary>
    /// <param name="clip">The AudioClip to play.</param>
    /// <param name="position">The position to play the sound at.</param>
    /// <param name="volume">The volume to play the sound at.</param>
    /// <param name="pitch">The pitch to play the sound at.</param>
    public void PlaySound(AudioClip clip, Vector3 position, float volume, float pitch)
    {
        if (clip != null)
        {
            // Create a temporary game object to hold the AudioSource
            GameObject tempGO = new GameObject("TempAudio");
            tempGO.transform.position = position;

            // Add an AudioSource to the game object
            AudioSource audioSource = tempGO.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.pitch = pitch;

            // Play the clip
            audioSource.Play();

            // Destroy the temporary game object after the clip has finished playing
            Object.Destroy(tempGO, clip.length / audioSource.pitch);
        }
    }
    /// <example>
    /// PlaySound(soundClip, new Vector3(0, 0, 0), 0.5f, 1.2f);
    /// </example>


    /// <summary>
    /// OVERLOAD 3 - MIN.MAX PITCH (Legacy): Plays a sound at a specified position with a specified volume and a randomized pitch within a range.
    /// </summary>
    /// <param name="clip">The AudioClip to play.</param>
    /// <param name="position">The position to play the sound at.</param>
    /// <param name="volume">The volume to play the sound at.</param>
    /// <param name="minPitch">The minimum pitch to play the sound at.</param>
    /// <param name="maxPitch">The maximum pitch to play the sound at.</param>
    public void PlaySound(AudioClip clip, Vector3 position, float volume, float minPitch, float maxPitch)
    {
        if (clip != null)
        {
            // Create a temporary game object to hold the AudioSource
            GameObject tempGO = new GameObject("TempAudio");
            tempGO.transform.position = position;

            // Add an AudioSource to the game object
            AudioSource audioSource = tempGO.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);

            // Play the clip
            audioSource.Play();

            // Destroy the temporary game object after the clip has finished playing
            Object.Destroy(tempGO, clip.length / audioSource.pitch);
        }
    }
    /// <example>
    /// ...
    /// </example>


    /// <summary>
    /// MIN.MAX PITCH (Legacy): Plays a sound at a specified position with a randomized pitch offset, volume, and cooldown.
    /// </summary>
    /// <param name="clip">The AudioClip to play.</param>
    /// <param name="position">The position to play the sound at.</param>
    /// <param name="volume">The volume to play the sound at.</param>
    /// <param name="pitchOffset">The maximum pitch offset for randomization.</param>
    /// <param name="cooldown">The cooldown time in seconds before the sound can be played again.</param>
    public void PlaySoundWithCooldown(AudioClip clip, Vector3 position, float volume, float pitchOffset, float cooldown)
    {
        if (clip != null)
        {
            float lastPlayed;
            if (cooldowns.TryGetValue(clip, out lastPlayed))
            {
                if (Time.time - lastPlayed < cooldown)
                {
                    // If the cooldown has not yet passed, do not play the sound
                    return;
                }
            }

            // Create a temporary game object to hold the AudioSource
            GameObject tempGO = new GameObject("TempAudio");
            tempGO.transform.position = position;

            // Add an AudioSource to the game object
            AudioSource audioSource = tempGO.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = volume;

            // Randomize pitch offset between (1 - pitchOffset) and (1 + pitchOffset)
            float randomPitch = Random.Range(1f - pitchOffset, 1f + pitchOffset);
            audioSource.pitch = randomPitch;

            // Play the clip
            audioSource.Play();

            // Destroy the temporary game object after the clip has finished playing
            Object.Destroy(tempGO, clip.length / audioSource.pitch);

            // Update the last played time
            cooldowns[clip] = Time.time;
        }
    }
    /// <example>
    /// PlaySoundWithCooldown(soundClip, new Vector3(0, 0, 0), 0.5f, 0.2f, 2f);
    /// </example>


    /// <summary>
    /// Play Random Audio 
    /// </summary>
    /// <param name="clips"></param>
    /// <param name="position"></param>
    /// <param name="cooldown"></param>
    /// <param name="volume"></param>
    public void PlayRandomAudio(AudioClip[] clips, Vector3 position, float cooldown, float volume = 1.0f)
    {
        if (clips != null && clips.Length > 0)
        {
            // Check cooldown
            if (!lastPlayedTimes.ContainsKey(clips) || Time.time - lastPlayedTimes[clips] >= cooldown)
            {
                int index = Random.Range(0, clips.Length);
                AudioClip clip = clips[index];

                // Play the clip at the specified volume
                AudioSource.PlayClipAtPoint(clip, position, volume);

                // Update the last played time
                lastPlayedTimes[clips] = Time.time;
            }
        }
    }
    /// <example>
    /// ...
    /// </example>


    /// <summary>
    /// Play Random Audio with pitch
    /// </summary>
    /// <param name="clips"></param>
    /// <param name="position"></param>
    /// <param name="cooldown"></param>
    /// <param name="volume"></param>
    public void PlayRandomAudioWithPitch(AudioClip[] clips, Vector3 position, float cooldown, float volume = 1.0f, float minPitch = 0.95f, float maxPitch = 1.05f)
    {
        if (clips != null && clips.Length > 0)
        {
            // Check cooldown
            if (!lastPlayedTimes.ContainsKey(clips) || Time.time - lastPlayedTimes[clips] >= cooldown)
            {
                int index = Random.Range(0, clips.Length);
                AudioClip clip = clips[index];

                GameObject audioObject = new GameObject("TempAudio");
                audioObject.transform.position = position;
                AudioSource audioSource = audioObject.AddComponent<AudioSource>();
                audioSource.clip = clip;
                audioSource.volume = volume;
                audioSource.pitch = Random.Range(minPitch, maxPitch);
                audioSource.Play();

                Destroy(audioObject, clip.length / audioSource.pitch); // Destroy the object after the clip has finished playing

                // Update the last played time
                lastPlayedTimes[clips] = Time.time;
            }
        }
    }
    /// <example>
    /// AudioManager.instance.PlayRandomAudioWithPitch(Sound, transform.position, 0.5f, 0.1f);
    /// </example>


    /// <summary>
    /// Start looping the audio
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    /// <param name="loop"></param>
    /// </summary>
    public void ToggleLoop(AudioClip clip, Vector3 position, float volume, bool loop)
    {
        if (clip != null)
        {
            if (loop)
            {
                // Start looping the audio clip if it's not already playing
                if (!loopingAudioSources.ContainsKey(clip))
                {
                    // Create a temporary game object to hold the AudioSource
                    GameObject tempGO = new GameObject("LoopingAudio_" + clip.name);
                    tempGO.transform.position = position;

                    // Add an AudioSource to the game object
                    AudioSource audioSource = tempGO.AddComponent<AudioSource>();
                    audioSource.clip = clip;
                    audioSource.volume = volume;
                    audioSource.loop = true;

                    // Play the clip
                    audioSource.Play();

                    // Store the audio source in the dictionary
                    loopingAudioSources[clip] = audioSource;

                    Debug.Log($"Started looping audio: {clip.name} at {position}");
                }
            }
            else
            {
                // Stop looping the audio clip if it's currently playing
                if (loopingAudioSources.TryGetValue(clip, out AudioSource audioSource))
                {
                    audioSource.Stop();
                    Object.Destroy(audioSource.gameObject);
                    loopingAudioSources.Remove(clip);

                    Debug.Log($"Stopped looping audio: {clip.name}");
                }
            }
        }
        else
        {
            Debug.LogWarning("Clip is null in ToggleLoopingAudio");
        }
    }
    /// <example>
    /// AudioManager.instance.ToggleLoopingAudio(yourAudioClip, transform.position, 1.0f, true);
    /// AudioManager.instance.ToggleLoopingAudio(yourAudioClip, transform.position, 1.0f, false);
    /// </example>


    /// <summary>
    /// Plays a sound at a specified position with a randomized pitch offset, volume, and cooldown.
    /// Can play directly from an AudioClip or by name from the SoundBank.
    /// </summary>
    /// <param name="clipOrName">The AudioClip to play or the name of the sound in the SoundBank.</param>
    /// <param name="position">The position to play the sound at.</param>
    /// <param name="volume">The volume to play the sound at.</param>
    /// <param name="pitchOffset">The maximum pitch offset for randomization.</param>
    /// <param name="cooldown">The cooldown time in seconds before the sound can be played again.</param>
    [Button("Test Play()")]
    public void PlaySFX(object clipOrName, Vector3 position, float volume = 0.1f, float pitchOffset = 0.1f, float cooldown = 0.01f)
    {
        AudioClip clip = null;

        // Determine if the input is an AudioClip or a string (sound name)
        if (clipOrName is AudioClip)
        {
            clip = (AudioClip)clipOrName;
        }
        else if (clipOrName is string soundName)
        {
            clip = OB_SFX_BANK.instance.GetAudioClip(soundName);
        }

        if (clip == null)
        {
            Debug.LogWarning("Sound not found or invalid input for PlaySoundWithCooldown.");
            return;
        }

        // Check the cooldown timer
        if (cooldowns.TryGetValue(clip, out float lastPlayed))
        {
            if (Time.time - lastPlayed < cooldown)
            {
                // Cooldown has not elapsed
                return;
            }
        }

        // Create a temporary game object to hold the AudioSource
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = position;

        // Add an AudioSource to the game object
        AudioSource audioSource = tempGO.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;

        // Randomize pitch offset between (1 - pitchOffset) and (1 + pitchOffset)
        float randomPitch = Random.Range(1f - pitchOffset, 1f + pitchOffset);
        audioSource.pitch = randomPitch;

        // Play the clip
        audioSource.Play();

        // Destroy the temporary game object after the clip has finished playing
        Object.Destroy(tempGO, clip.length / audioSource.pitch);

        // Update the last played time
        cooldowns[clip] = Time.time;
    }
    /// <example>
    /// PlaySoundWithCooldown("Explosion", new Vector3(0, 0, 0), 0.5f, 0.2f, 2f);
    /// PlaySoundWithCooldown(myAudioClip, new Vector3(0, 0, 0), 0.8f, 0.1f, 1f);
    /// </example>


    /// <summary>
    /// Plays Random passed audio
    /// </summary>
    /// <param name="clipsOrNames"></param>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    /// <param name="pitchOffset"></param>
    /// <param name="cooldown"></param>
    [Button("Test PlayRandom()")]
    public void PlayRandom(object[] clipsOrNames, Vector3 position, float volume = 0.1f, float pitchOffset = 0.1f, float cooldown = 0.01f)
    {
        if (clipsOrNames == null || clipsOrNames.Length == 0)
        {
            Debug.LogWarning("No clips provided to PlaySFXRandom.");
            return;
        }

        // Randomly select a clip or name from the array
        object selectedClipOrName = clipsOrNames[Random.Range(0, clipsOrNames.Length)];

        // Delegate to PlaySFX for processing
        PlaySFX(selectedClipOrName, position, volume, pitchOffset, cooldown);
    }
    /// <example>
    ///   AudioManager.instance.PlaySFXRandom(
    ///      new object[] { "audioClip1", audioClip2, audioClip3 },
    ///      transform.position,
    ///      0.5f
    ///   );
    /// </example>
}