using UnityEngine;

public class WaveTrigger : MonoBehaviour
// wave trigger
{
    public CameraMovement cameraMovement;
    public float nextWaveMinY = -30;
    public float nextWaveMaxY = -10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cameraMovement.TransitionToNextWave(nextWaveMinY, nextWaveMaxY);
            Destroy(gameObject); // Remove the platform after triggering
        }
    }
}
