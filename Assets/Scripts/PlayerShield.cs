using UnityEngine;
using System.Collections;

public class PlayerShield : MonoBehaviour
{
    [Header("Shield Settings")]
    public GameObject shieldObject; // The actual shield GameObject
    public float shieldDuration = 3f; // Time the shield stays active
    public float cooldownTime = 5f; // Cooldown time before the shield can be used again

    [Header("Keybind")]
    public KeyCode activationKey = KeyCode.Space; // Key to activate the shield

    private bool isShieldActive = false;
    private bool isOnCooldown = false;

    void Start()
    {
        shieldObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(activationKey) && !isShieldActive && !isOnCooldown)
        {
            StartCoroutine(ActivateShield());
        }

        if (Input.GetKeyDown(activationKey) && isOnCooldown == true)
        {
            Debug.Log("wait for the cool down!");
        }
    }

    IEnumerator ActivateShield()
    {
        // Activate the shield
        isShieldActive = true;
        shieldObject.SetActive(true);

        // Wait for the shield duration
        yield return new WaitForSeconds(shieldDuration);

        // Deactivate the shield
        shieldObject.SetActive(false);
        isShieldActive = false;

        // Start cooldown
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy any projectile that collides with the shield
        if (isShieldActive && other.CompareTag("Bullet")) // Ensure bullets have the tag "Bullet"
        {
            Destroy(other.gameObject);
        }
    }
}
