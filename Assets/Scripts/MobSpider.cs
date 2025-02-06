using UnityEngine;
using VInspector;

namespace Descent
{
    public class MobSpider : MonoBehaviour
    {
        [Header("---Spider Movements---")]
        public Transform player; // Reference to the player
        public float speed = 2f; // Speed at which the mob moves toward the player
        public Vector2 verticalLimits = new Vector2(-10, 10); // Limits for vertical movement along the wall
        public float attackRange = 1f; // Range within which the mob can attack the player

        public bool isOnLeftWall; // Determines if the mob is on the left wall

        [Header("---Projectile---")]

        public GameObject projectilePrefab; // The projectile prefab
        public float projectileSpeed = 5f; // Speed of the projectile
        public float shootInterval = 2f; // Time between projectile shots
        public float lastShootTime; // Tracks the last time the mob shot a projectile



        private void Start()
        {
            // Determine if the mob starts on the left or right wall
            isOnLeftWall = transform.position.x < 0;
        }

        private void Update()
        {
            if (UIManager.Instance.IsMenuActive) return;

            // Move vertically toward the player
            float targetY = Mathf.Clamp(player.position.y, verticalLimits.x, verticalLimits.y);
            Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Check if the mob can shoot at the player
            if (Time.time - lastShootTime >= shootInterval && Vector3.Distance(transform.position, player.position) <= attackRange * 5)
            {
                ShootProjectile();
                lastShootTime = Time.time; // Reset shoot timer
            }
        }

        [Button]
        private void ShootProjectile()
        {
            if (UIManager.Instance.IsMenuActive) return;

            if (projectilePrefab == null) return;

            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // Calculate direction to the player
            Vector2 direction = (player.position - transform.position).normalized;

            // Add velocity to the projectile
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * projectileSpeed;
            }

            // Optionally rotate the projectile to face the direction of travel
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}