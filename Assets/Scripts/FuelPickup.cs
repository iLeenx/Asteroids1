using UnityEngine;
namespace Descent
{

    public class FuelPickup : MonoBehaviour
    {
        [Header("Fuel Settings")]
        [SerializeField] private float fuelAmount = 25f; // Amount of fuel to add
        [SerializeField] private GameObject pickupEffect; // Visual effect when collected
        [SerializeField] private AudioClip pickupSound; // Sound effect when collected

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Get the player's Status component
                Status playerStatus = other.GetComponent<Status>();
                if (playerStatus != null)
                {
                    // Add fuel to the player
                    playerStatus.AddFuel(fuelAmount);

                    // Play pickup effect
                    if (pickupEffect != null)
                    {
                        Instantiate(pickupEffect, transform.position, Quaternion.identity);
                    }

                    // Play pickup sound
                    if (pickupSound != null)
                    {
                        AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                    }

                    // Destroy the pickup object
                    Destroy(gameObject);
                }
            }
        }
    }
}