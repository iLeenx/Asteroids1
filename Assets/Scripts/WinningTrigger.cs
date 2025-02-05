using UnityEngine;

public class WinningTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
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
