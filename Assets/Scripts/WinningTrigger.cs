using UnityEngine;

public class WinningTrigger : MonoBehaviour
{
    private void Update()
    {
        GameObject[] mobs = GameObject.FindGameObjectsWithTag("Mob");

        // Check if all are destroyed
        if (mobs.Length == 0)
        {
            enabled = false; // Stop checking once triggered
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject[] mobs = GameObject.FindGameObjectsWithTag("Mob");

        if (other.CompareTag("Player") && mobs.Length == 0)
        {
            GameObject canvasUI = GameObject.Find("CANVAS_UI_SCREENS"); // Find the parent Canvas
            if (canvasUI != null)
            {
                Transform winScreenTransform = canvasUI.transform.Find("WIN_SCREEN");
                if (winScreenTransform != null)
                {
                    winScreenTransform.gameObject.SetActive(true); // Show WIN_SCREEN
                    Time.timeScale = 0f; // Optional: Pause the game
                }
                else
                {
                    Debug.LogWarning("WIN_SCREEN not found inside CANVAS_UI_SCREENS!");
                }
            }
            else
            {
                Debug.LogWarning("CANVAS_UI_SCREENS not found in the scene!");
            }
        }
    }
}
