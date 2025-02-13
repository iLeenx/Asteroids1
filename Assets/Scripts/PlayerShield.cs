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
            shieldObject.SetActive(false); // Ensure the shield is off initially
        }

        void Update()
        {
            // Activate the shield while holding mouse button 1
            if (Input.GetKey(activationKey) && !isShieldActive && !isOnCooldown)
            {
                StartCoroutine(ActivateShield());
            }

            // Check cooldown status if trying to press mouse 1 again
            if (Input.GetKeyDown(activationKey) && isOnCooldown)
            {
                Debug.Log("Shield is on cooldown!");
            }
        }

        IEnumerator ActivateShield()
        {
            // Activate the shield
            isShieldActive = true;
            shieldObject.SetActive(true);

            // Wait for the shield duration or until the button is released
            float timer = 0f;
            while (Input.GetKey(activationKey) && timer < shieldDuration)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            // Deactivate the shield when the duration ends or the button is released
            shieldObject.SetActive(false);
            isShieldActive = false;

            // Start cooldown
            isOnCooldown = true;
            yield return new WaitForSeconds(cooldownTime);
            isOnCooldown = false;
        }    
    }
}