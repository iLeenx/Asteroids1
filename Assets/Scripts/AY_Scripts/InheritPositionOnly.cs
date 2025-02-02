using UnityEngine;

public class InheritPositionOnly : MonoBehaviour
{
    public GameObject playerGO;
    public GameObject mainCameraGO;

    // Offset variables for player
    public float playerOffsetX = 0f;
    public float playerOffsetY = 0f;
    public float playerOffsetZ = 0f;

    // Offset variables for camera
    public float cameraOffsetX = 0f;
    public float cameraOffsetY = 0f;
    public float cameraOffsetZ = 0f;

    // Toggle to choose between player and camera
    public bool useCameraPosition = false;

    // Booleans to control which vector components to inherit
    public bool inheritPositionX = true;
    public bool inheritPositionY = true;
    public bool inheritPositionZ = true;
    public bool inheritRotation = false;

    private void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        mainCameraGO = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void LateUpdate()
    {
        Vector3 newPosition = transform.position;

        if (useCameraPosition && mainCameraGO != null)
        {
            if (inheritPositionX) newPosition.x = mainCameraGO.transform.position.x + cameraOffsetX;
            if (inheritPositionY) newPosition.y = mainCameraGO.transform.position.y + cameraOffsetY;
            if (inheritPositionZ) newPosition.z = mainCameraGO.transform.position.z + cameraOffsetZ;

            transform.position = newPosition;

            if (inheritRotation)
            {
                transform.rotation = mainCameraGO.transform.rotation;
            }
        }
        else if (playerGO != null)
        {
            if (inheritPositionX) newPosition.x = playerGO.transform.position.x + playerOffsetX;
            if (inheritPositionY) newPosition.y = playerGO.transform.position.y + playerOffsetY;
            if (inheritPositionZ) newPosition.z = playerGO.transform.position.z + playerOffsetZ;

            transform.position = newPosition;

            if (inheritRotation)
            {
                transform.rotation = playerGO.transform.rotation;
            }
        }
    }
}
