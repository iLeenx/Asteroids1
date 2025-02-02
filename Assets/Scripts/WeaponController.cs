using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projectilePrefab;
    public int projectileCount = 10;
    public float spreadAngle = 15f;
    public float projectileSpeed = 20f;
    public string fireButton = "Fire1";
    public float fireRate = 0.5f;
    public bool isEnabled = false;

    private float nextFireTime = 0f;

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
    }
}
