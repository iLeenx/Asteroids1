using UnityEngine;
namespace Descent
{
    public class CameraMovement : MonoBehaviour
    {
        public Transform player; // Reference to the player's transform
        public float smoothSpeed = 2f; // Speed of the camera smoothing
        public Vector2 yLimitRange = new Vector2(0, 0); // Initial camera y-axis limits
        public float transitionSpeed = 3f; // Speed of camera transitioning to a new wave
        public float additionalViewRange = 5f; // Additional range for viewing adjacent waves

        private bool isTransitioning = false; // Whether the camera is currently transitioning
        private float targetYLimitMin; // Target minimum y limit for the camera
        private float targetYLimitMax; // Target maximum y limit for the camera

        private void Start()
        {
            // Initialize the target limits to the current limits
            targetYLimitMin = yLimitRange.x;
            targetYLimitMax = yLimitRange.y;
        }

        private void LateUpdate()
        {
            if (isTransitioning)
            {
                // Smoothly move the camera to focus on the new wave
                Vector3 targetPosition = new Vector3(transform.position.x, (targetYLimitMin + targetYLimitMax) / 2, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPosition, transitionSpeed * Time.deltaTime);

                // Check if the transition is complete
                if (Mathf.Abs(transform.position.y - targetPosition.y) < 0.1f)
                {
                    isTransitioning = false;
                }
            }
            else
            {
                // Follow the player, allowing the camera to show part of the adjacent wave
                float clampedY = Mathf.Clamp(player.position.y, targetYLimitMin - additionalViewRange, targetYLimitMax + additionalViewRange);
                Vector3 desiredPosition = new Vector3(transform.position.x, clampedY, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            }
        }

        public void TransitionToNextWave(float newMinLimit, float newMaxLimit)
        {
            // Update the target limits for the next wave
            targetYLimitMin = newMinLimit;
            targetYLimitMax = newMaxLimit;
            isTransitioning = true;

            // Update the camera's current y-axis limits
            yLimitRange = new Vector2(newMinLimit, newMaxLimit);

            // Notify the player to update its vertical limits
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.UpdateVerticalLimits(newMinLimit, newMaxLimit);
                }
            }
        }
    }
}