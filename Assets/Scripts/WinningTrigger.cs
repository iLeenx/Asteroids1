using UnityEngine;

public class WinningTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the "Player" tag
        {
            GameObject winScreen = GameObject.Find("CANVAS_UI_SCREENS/WIN_SCREEN"); // Find the object by name
            
            if (winScreen != null) // Check if the object was found
            {
                winScreen.SetActive(true); // Show the win screen
                Time.timeScale = 0f; // Optional: Pause the game
            }
            else
            {
                Debug.LogWarning("WIN_SCREEN object not found in the scene!");
            }
        }
    }
}
