using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public GameObject winningTrigger; // Assign the "Winning Trigger" in the Inspector
    public bool isStarted = false;

    void Update()
    {
        if (isStarted)
        {
            CheckWinCondition();
        }
    }

    public void StartCheckingGameState()
    {
        isStarted = true;
    }


    void CheckWinCondition()
    {
        GameObject[] mobs = GameObject.FindGameObjectsWithTag("Mob");

        // Check if all are destroyed
        if (mobs.Length == 0)
        {
            winningTrigger.SetActive(true);
            enabled = false; // Stop checking once triggered
        }
    }
}
