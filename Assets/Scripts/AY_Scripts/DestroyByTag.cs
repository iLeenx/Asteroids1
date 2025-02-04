using UnityEngine;

public class DestroyByTag : MonoBehaviour
{
    [SerializeField] private string targetTag = "Bullet"; // Set the tag in the Inspector
    [SerializeField] private float destroyDelay = 0f; // Delay before destruction

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            Destroy(gameObject, destroyDelay);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(targetTag))
        {
            Destroy(gameObject, destroyDelay);
        }
    }
}
