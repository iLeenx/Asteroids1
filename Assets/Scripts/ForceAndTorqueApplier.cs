using UnityEngine;

public class ForceAndTorqueApplier2D : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Force Settings")]
    public Vector2 forceDirection = Vector2.right;
    public float forceMagnitude = 10f;

    [Header("Force Randomization")]
    public bool randomizeForceX = false;
    public bool randomizeForceY = false;

    [Header("Torque Settings")]
    public float torque = 5f;

    [Header("Torque Randomization")]
    public bool randomizeTorque = false;

    [Header("Apply on Start?")]
    public bool applyOnStart = true;

    void Start()
    {
        if (applyOnStart)
            ApplyForceAndTorque();
    }

    public void ApplyForceAndTorque()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("No Rigidbody2D found!");
                return;
            }
        }

        // Randomize force per axis if enabled
        Vector2 finalForce = new Vector2(
            randomizeForceX ? Random.Range(-1f, 1f) * forceMagnitude : forceDirection.x * forceMagnitude,
            randomizeForceY ? Random.Range(-1f, 1f) * forceMagnitude : forceDirection.y * forceMagnitude
        );

        // Randomize torque if enabled
        float finalTorque = randomizeTorque ? Random.Range(-torque, torque) : torque;

        // Apply force and torque
        rb.AddForce(finalForce, ForceMode2D.Impulse);
        rb.AddTorque(finalTorque);
    }
}
