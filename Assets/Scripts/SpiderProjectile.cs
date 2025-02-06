using UnityEngine;
namespace Descent
{
    public class SpiderProjectile : MonoBehaviour
    {
        public float lifetime = 5f; // Time before the projectile is destroyed

        private void Start()
        {
            Destroy(gameObject, lifetime); // Destroy the projectile after a set time
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Example: Damage the player
                Debug.Log("Projectile hit the player!");
                // Destroy the projectile
                Destroy(gameObject);
            }

            if (other.CompareTag("Environment"))
            {
                // Destroy the projectile when hitting the environment
                Debug.Log("Projectile hit the environment!");
                Destroy(gameObject);
            }
        }
    }
}