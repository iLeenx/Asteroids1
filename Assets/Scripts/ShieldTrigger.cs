using UnityEngine;

public class ShieldTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
