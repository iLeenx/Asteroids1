using UnityEngine;

namespace Descent
{
    public class DestroyByTag : MonoBehaviour
    {
        [Header("Tag Settings")]
        [SerializeField] private string targetTag = "Bullet"; // Set the tag in the Inspector
        [SerializeField] private float destroyDelay = 0f; // Delay before destruction

        [Header("Health Settings")]
        [SerializeField] private float maxHealth = 100f; // Maximum health of the object
        [SerializeField] private float currentHealth; // Current health of the object
        [SerializeField] private bool destroyOnZeroHealth = true; // Should destroy when health reaches zero

        private void Awake()
        {
            currentHealth = maxHealth; // Initialize health
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(targetTag))
            {
                TakeDamage(10f); // Example damage value, can be modified as needed
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(targetTag))
            {
                TakeDamage(10f); // Example damage value, can be modified as needed
            }
        }

        /// <summary>
        /// Applies damage to the object and checks for destruction.
        /// </summary>
        /// <param name="damageAmount">The amount of damage to apply.</param>
        public void TakeDamage(float damageAmount)
        {
            currentHealth -= damageAmount;

            if (currentHealth <= 0 && destroyOnZeroHealth)
            {
                Destroy(gameObject, destroyDelay);
            }
        }

        /// <summary>
        /// Heals the object by a specified amount.
        /// </summary>
        /// <param name="healAmount">The amount of health to restore.</param>
        public void Heal(float healAmount)
        {
            currentHealth += healAmount;
            currentHealth = Mathf.Min(currentHealth, maxHealth); // Clamp to max health
        }
    }
}
