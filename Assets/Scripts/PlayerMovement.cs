using UnityEngine;
namespace Descent
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float moveSpeed { get; set; } // Public property for speed control
        [SerializeField] private float baseSpeed = 5f;

        [Header("Ground Check")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckRadius = 0.2f;

        private Rigidbody2D rb;
        private bool isGrounded;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            moveSpeed = baseSpeed; // Initialize with base speed
        }

        void Update()
        {
            // Ground check
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            // Movement logic
            float moveInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

            // Flip player sprite based on movement direction
            if (moveInput > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (moveInput < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
}