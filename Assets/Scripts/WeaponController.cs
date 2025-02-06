using UnityEngine;
using TMPro; // Required for TextMeshPro

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Settings")]
    [Tooltip("The point from which projectiles are fired.")]
    public Transform firePoint;

    [Tooltip("The projectile prefab to instantiate when firing.")]
    public GameObject projectilePrefab;

    [Header("Firing Settings")]
    [Tooltip("The number of projectiles fired per shot.")]
    public int projectileCount = 1;

    [Tooltip("The spread angle variation for projectiles.")]
    public float spreadAngle = 0f;

    [Tooltip("The speed of the projectiles.")]
    public float projectileSpeed = 20f;

    [Tooltip("The input button used for firing.")]
    public string fireButton = "Fire1";

    [Tooltip("The time delay between each shot.")]
    public float fireRate = 0.5f;

    [Tooltip("Determines if the weapon is currently enabled.")]
    public bool isEnabled = false;

    [Header("Ammo System")]
    [Tooltip("The maximum amount of ammo the weapon can hold.")]
    public int maxAmmo = 10;

    [Tooltip("The current amount of ammo available.")]
    public int currentAmmo = 10;

    [Tooltip("If enabled, the weapon will have unlimited ammo.")]
    public bool infiniteAmmo = false;

    [Header("UI References (Optional)")]
    [Tooltip("Reference to the TextMeshPro Text for displaying ammo count (Optional).")]
    public TMP_Text ammoText;

    private float nextFireTime = 0f;

    private void Start()
    {
        UpdateAmmoUI();
    }

    private void Update()
    {
        if (!isEnabled) return;

        AimAtMouse();

        if (Input.GetButton(fireButton) && Time.time >= nextFireTime)
        {
            FireWeapon();
            nextFireTime = Time.time + fireRate;
        }
    }

    void AimAtMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void FireWeapon()
    {
        if (UIManager.Instance.IsMenuActive) return;

        if (!infiniteAmmo && currentAmmo <= 0)
        {
            Debug.Log("Out of Ammo!");
            return; // Prevent firing if no ammo
        }

        for (int i = 0; i < projectileCount; i++)
        {
            float randomSpread = Random.Range(-spreadAngle, spreadAngle);
            float fireAngle = transform.rotation.eulerAngles.z + randomSpread;
            Vector3 projectileDirection = new Vector3(
                Mathf.Cos(fireAngle * Mathf.Deg2Rad),
                Mathf.Sin(fireAngle * Mathf.Deg2Rad),
                0
            );

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = projectileDirection * projectileSpeed;
            }
        }

        if (!infiniteAmmo)
        {
            currentAmmo--; // Reduce ammo only if not infinite
            UpdateAmmoUI();
        }
    }

    // Upgrade Methods

    public void UpgradeProjectileCount(float value)
    {
        projectileCount += Mathf.RoundToInt(value);
        projectileCount = Mathf.Max(1, projectileCount);
    }

    public void UpgradeSpreadAngle(float value)
    {
        spreadAngle += value;
        spreadAngle = Mathf.Clamp(spreadAngle, 0f, 180f);
    }

    public void UpgradeProjectileSpeed(float value)
    {
        projectileSpeed += value;
        projectileSpeed = Mathf.Max(1f, projectileSpeed);
    }

    public void UpgradeFireRate(float value)
    {
        fireRate -= value;
        fireRate = Mathf.Max(0.1f, fireRate);
    }

    // Ammo Methods

    /// <summary>
    /// Adds ammo to the weapon.
    /// </summary>
    /// <param name="amount">Amount of ammo to add.</param>
    public void AddAmmo(int amount)
    {
        if (infiniteAmmo) return; // Skip if infinite ammo is enabled

        currentAmmo += amount;
        currentAmmo = Mathf.Min(currentAmmo, maxAmmo); // Ensure ammo does not exceed max capacity
        UpdateAmmoUI();
    }

    /// <summary>
    /// Toggles infinite ammo mode.
    /// </summary>
    /// <param name="enabled">True to enable infinite ammo, false to disable.</param>
    public void SetInfiniteAmmo(bool enabled)
    {
        infiniteAmmo = enabled;
        UpdateAmmoUI();
    }

    /// <summary>
    /// Updates the ammo UI text if a reference is assigned.
    /// </summary>
    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            if (infiniteAmmo)
                ammoText.text = "";
            else
                ammoText.text = $"{currentAmmo}/{maxAmmo}";
        }
    }
}
