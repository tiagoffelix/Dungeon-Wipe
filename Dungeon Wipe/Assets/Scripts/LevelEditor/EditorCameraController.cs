using UnityEngine;

public class EditorCameraController : MonoBehaviour
{
    private float movementSpeed = 10f;
    private float rotationSpeed = 5f;
    private float zoomSpeed = 60f;

    private float yaw = 0f;
    private float pitch = 0f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        // Store the original position and rotation
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        // Handle rotation
        if (Input.GetMouseButton(1)) // Right mouse button
        {
            yaw += rotationSpeed * Input.GetAxis("Mouse X");
            pitch -= rotationSpeed * Input.GetAxis("Mouse Y");
            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }

        // Handle movement along X and Z planes
        float xMovement = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime; // A/D or Left/Right
        float yMovement = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime; // W/S or Up/Down

        // Handle zooming with the scroll wheel
        float zMovement = Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;

        Vector3 move = transform.right * xMovement + transform.forward * zMovement + transform.up * yMovement;
        transform.position += move;

        if (Input.GetKeyDown(KeyCode.F))
        {
            ResetCameraPosition();
        }
    }

    public void ResetCameraPosition()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        yaw = originalRotation.eulerAngles.y;
        pitch = originalRotation.eulerAngles.x;
    }
}
