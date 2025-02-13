using UnityEngine;

public class ShieldTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy any projectile that collides with the shield
        if (other.CompareTag("Bullet")) // Ensure bullets have the tag "Bullet"
        {
            Debug.Log("shielding");
            Destroy(other.gameObject);
        }
    }
}