using UnityEngine;
namespace Descent
{
    public class PlayerMovement : MonoBehaviour
    {
        Rigidbody2D myRigidbody;
        [SerializeField] float runSpeed = 2f;
        Collider2D myCollider;

        public float horizontalLimit = 7f; // Horizontal movement limit relative to the camera center
        public float maxVerticalLimit; // Upper vertical movement limit
        public float minVerticalLimit; // Lower vertical movement limit
        public float FuelReduceBySpiderBullet; // Lower vertical movement limit

        public Camera mainCamera;
        public CameraMovement cameraMovement;

        public Status status;

        void Start()
        {
            myCollider = GetComponent<Collider2D>();
            myRigidbody = GetComponent<Rigidbody2D>();
            mainCamera = Camera.main;
            cameraMovement = mainCamera.GetComponent<CameraMovement>();

            // Initialize the player's vertical limits based on the camera's starting range
            maxVerticalLimit = cameraMovement.yLimitRange.y;
            minVerticalLimit = cameraMovement.yLimitRange.x;
        }

        void Update()
        {
            Run();
            ClampPositionWithinCameraBounds();
        }

        private void Run()
        {
            float controlDirection_x = Input.GetAxis("Horizontal");
            float controlDirection_y = Input.GetAxis("Vertical");

            Vector2 playerVelocity = new Vector2(controlDirection_x * runSpeed, controlDirection_y * runSpeed);
            myRigidbody.linearVelocity = playerVelocity;

            // Flip the player sprite based on horizontal movement
            if (controlDirection_x < 0)
            {
                transform.localScale = new Vector3(-0.2f, 0.2f, 0.2f);
            }
            else if (controlDirection_x > 0)
            {
                transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
        }

        private void ClampPositionWithinCameraBounds()
        {
            Vector3 cameraCenter = mainCamera.transform.position;

            // Clamp horizontal movement within 3f of the camera's center
            float clampedX = Mathf.Clamp(transform.position.x, cameraCenter.x - horizontalLimit, cameraCenter.x + horizontalLimit);

            // Clamp vertical movement within the dynamic camera limits
            float clampedY = Mathf.Clamp(transform.position.y, minVerticalLimit, maxVerticalLimit);

            // Apply the clamped position
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }

        public void UpdateVerticalLimits(float newMinLimit, float newMaxLimit)
        {
            // Update the player's vertical limits based on the camera's updated range
            minVerticalLimit = newMinLimit;
            maxVerticalLimit = newMaxLimit;
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Bullet"))
            {
                status.ReduceFuel(FuelReduceBySpiderBullet);
                OB_SFX.instance.PlaySFX("Test",transform.position);
            }
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Bullet"))
            {
                status.ReduceFuel(FuelReduceBySpiderBullet);
                OB_SFX.instance.PlaySFX("Test", transform.position);
            }
        }
    }
}