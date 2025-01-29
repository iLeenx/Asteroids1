using UnityEngine;
namespace Descent{
    public class WaveTrigger : MonoBehaviour
    {
        public CameraMovement cameraMovement;
        public float nextWaveMinY = -30;
        public float nextWaveMaxY = -10;

        private WaveCardSwapper waveCardSwapper;

        private void Start()
        {
            waveCardSwapper = FindFirstObjectByType<WaveCardSwapper>();

            if (waveCardSwapper == null)
            {
                Debug.LogError("[WaveTrigger] WaveCardSwapper not found! Make sure it's in the scene.");
            }
            else
            {
                Debug.Log("[WaveTrigger] Successfully found WaveCardSwapper.");
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("[WaveTrigger] Player triggered a new wave!");

                cameraMovement.TransitionToNextWave(nextWaveMinY, nextWaveMaxY);

                if (waveCardSwapper != null)
                {
                    Debug.Log("[WaveTrigger] Calling SwapWave()");
                    waveCardSwapper.SwapWave(); // ✅ FIXED: Call the correct function
                }
                else
                {
                    Debug.LogError("[WaveTrigger] waveCardSwapper is NULL. Wave indicator won't update.");
                }

                Destroy(gameObject);
            }
        }
    }
}