using UnityEngine;
using System.Collections;

namespace Descent
{
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
        shieldObject.SetActive(false); // Ensure the shield is off at the start
    }

    void Update()
    {
        // Check if the mouse button is held down
        if (Input.GetKey(activationKey) && !isShieldActive && !isOnCooldown)
        {
            StartCoroutine(ActivateShield());
        }

        // Check if the shield should be deactivated after the mouse button is released
        if (Input.GetKeyUp(activationKey) && isShieldActive)
        {
            StopCoroutine(ActivateShield()); // Stop the shield activation coroutine if it's running
            shieldObject.SetActive(false);
            isShieldActive = false;
            
            // Start cooldown after release
            StartCoroutine(Cooldown());
        }

        if (Input.GetKeyDown(activationKey) && isOnCooldown)
        {
            Debug.Log("Wait for the cooldown!");
        }
    }

    IEnumerator ActivateShield()
    {
        // Activate the shield
        isShieldActive = true;
        shieldObject.SetActive(true);

        // Wait for the shield duration
        yield return new WaitForSeconds(shieldDuration);

        // Deactivate the shield if the button is not released before duration
        if (isShieldActive)
        {
            shieldObject.SetActive(false);
            isShieldActive = false;
        }
    }

    IEnumerator Cooldown()
    {
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
}