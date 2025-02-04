using UnityEngine;
using TMPro; // Import TextMeshPro namespace

namespace Descent
{
    public class Status_AY : MonoBehaviour
    {
        [Header("Fuel Settings")]
        [SerializeField] private float maxFuel = 100f; // Maximum fuel capacity
        [SerializeField] private float fuelDepletionRate = 5f; // Fuel depletion per second
        private float currentFuel; // Current fuel level

        [Header("Fuel UI (Optional)")]
        [Tooltip("Reference to TMP Text for displaying fuel amount.")]
        [SerializeField] private TMP_Text fuelText; // TMP text to display fuel

        [Header("Game Over Settings")]
        [SerializeField] private GameObject deathEffect; // Effect to play on death
        [SerializeField] private string bulletTag = "Bullet"; // Tag for objects that reduce fuel


        [SerializeField] private GameObject gameOverScreen; // Assign this in the Inspector

        private bool isGameOver = false; // Track game over state


        private void Start()
        {
            currentFuel = maxFuel; // Start with full fuel
            UpdateFuelUI(); // Initialize fuel UI display
        }

        private void Update()
        {
            if (isGameOver) return;

            DepleteFuelOverTime();

            if (currentFuel <= 0)
            {
                TriggerGameOver();
            }
        }

        // ✅ Depletes fuel over time
        private void DepleteFuelOverTime()
        {
            ModifyFuel(-fuelDepletionRate * Time.deltaTime);
        }

        // ✅ Handles collision with bullets
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(bulletTag))
            {
                ModifyFuel(-10f); // Reduce fuel by 10 when hit by a bullet
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(bulletTag))
            {
                ModifyFuel(-10f); // Reduce fuel by 10 when hit by a bullet
            }
        }

        // ✅ Universal method to modify fuel (updates UI accordingly)
        private void ModifyFuel(float amount)
        {
            currentFuel = Mathf.Clamp(currentFuel + amount, 0, maxFuel);
            UpdateFuelUI();
        }

        // Updates the TMP Fuel UI (if assigned)
        private void UpdateFuelUI()
        {
            if (fuelText != null)
            {
                fuelText.text = $"Fuel: {Mathf.CeilToInt(currentFuel)}"; // Display whole number
            }
        }

        // Restores fuel (e.g., pickups)
        public void AddFuel(float amount)
        {
            ModifyFuel(amount);
        }

        // Handles game over
        private void TriggerGameOver()
        {
            if (isGameOver) return;

            isGameOver = true;
            Debug.Log("Game Over! Fuel depleted.");

            if (deathEffect != null)
            {
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            }

            // ctivate the Game Over screen (if assigned)
            if (gameOverScreen != null)
            {
                gameOverScreen.SetActive(true);
            }

            // Optionally, disable player controls here
            // gameObject.SetActive(false); // Hide the player after game over
        }
    }
}
