using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// Pause player logic if Main Menu screen is active
// if (UIManager.Instance != null && UIManager.Instance.IsMenuActive) return;

// Pause player logic if Pause screen is active
// if (UIManager.Instance != null && UIManager.Instance.IsPauseActive) return;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } // Singleton instance

    public bool FreezeTimeOnMainMenu = false;
    public bool FreezeTimeOnPause = false;

    [SerializeField] private List<GameObject> menuElements = new List<GameObject>(); // Menus that pause the game
    [Space(20)]
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape; // Key to toggle pause
    [SerializeField] private List<GameObject> pauseElements = new List<GameObject>(); // Pause-related UI
    [Space(20)]
    [SerializeField] private List<GameObject> quitButtons = new List<GameObject>(); // Quit buttons
    [Space(20)]
    [SerializeField] private PlayableDirector playableDirector; // Optional PlayableDirector

    public bool IsMenuActive => CheckActiveStatus(menuElements); // True if any menu is active
    public bool IsPauseActive => CheckActiveStatus(pauseElements); // True if any pause UI is active

    public float CurrentTimeScale;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

#if UNITY_WEBGL
        DisableQuitButtons(); // Disable quit buttons if running on WebGL
#endif
    }

    private void Update()
    {
        if (FreezeTimeOnPause || FreezeTimeOnMainMenu) ManageTimeScale();

        // Block pause if a menu is active
        if (Input.GetKeyDown(pauseKey) && !IsMenuActive)
        {
            TogglePause();
        }
    }

    private void FixedUpdate()
    {
        CurrentTimeScale = Time.timeScale;
    }

    /// <summary>
    /// Toggles all pause UI elements.
    /// </summary>
    public void TogglePause()
    {
        // Only allow pausing if NO menu is active
        if (IsMenuActive) return;

        bool shouldEnable = !IsPauseActive; // Toggle pause state

        foreach (var pauseUI in pauseElements)
        {
            if (pauseUI != null)
            {
                pauseUI.SetActive(shouldEnable);
            }
        }

        ManageTimeScale();
    }

    /// <summary>
    /// Checks if any GameObject in a list is active.
    /// </summary>
    private bool CheckActiveStatus(List<GameObject> elements)
    {
        foreach (var element in elements)
        {
            if (element.activeSelf) return true;
        }
        return false;
    }

    /// <summary>
    /// Manages Time.timeScale separately for the main menu and pause menu.
    /// </summary>
    private void ManageTimeScale()
    {
        if (IsMenuActive && FreezeTimeOnMainMenu)
        {
            Time.timeScale = 0; // Freeze time for main menu
        }
        else if (IsPauseActive && FreezeTimeOnPause)
        {
            Time.timeScale = 0; // Freeze time for pause menu
        }
        else
        {
            Time.timeScale = 1; // Resume time when no freeze condition is met
        }
    }


    /// <summary>
    /// Toggles a menu UI element and updates the time scale.
    /// </summary>
    public void ToggleMenu(GameObject menu)
    {
        if (menu == null) return;

        menu.SetActive(!menu.activeSelf);
        ManageTimeScale();
    }

    /// <summary>
    /// Toggles a pause UI element.
    /// </summary>
    public void TogglePauseUI(GameObject pauseUI)
    {
        if (pauseUI == null) return;

        pauseUI.SetActive(!pauseUI.activeSelf);
    }

    /// <summary>
    /// Plays a UI transition using PlayableDirector (optional).
    /// </summary>
    public void PlayTransition()
    {
        if (playableDirector != null)
        {
            playableDirector.Play();
        }
    }

    public void QuitGame()
    {
#if UNITY_WEBGL
    Debug.Log("Quit function called, but quitting is not supported in WebGL.");
#elif UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the Editor
#else
    Application.Quit(); // Quit the built game (Standalone)
#endif
    }

    /// <summary>
    /// Disables all quit buttons in WebGL builds.
    /// </summary>
    private void DisableQuitButtons()
    {
        foreach (var button in quitButtons)
        {
            if (button != null)
            {
                button.SetActive(false);
            }
        }
        Debug.Log("Quit buttons disabled for WebGL.");
    }
}
