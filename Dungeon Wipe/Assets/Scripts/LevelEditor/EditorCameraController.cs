using UnityEngine;

/// <summary>
/// Controls the movement, rotation, and zooming of the editor camera.
/// </summary>
public class EditorCameraController : MonoBehaviour
{
    private float movementSpeed = 10f; // The speed of camera movement.
    private float rotationSpeed = 5f; // The speed of camera rotation.
    private float zoomSpeed = 60f; // The speed of camera zooming.

    private float yaw = 0f; // The yaw angle for camera rotation.
    private float pitch = 0f; // The pitch angle for camera rotation.

    private Vector3 originalPosition; // The original position of the camera.
    private Quaternion originalRotation; // The original rotation of the camera.

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

        // Handle zooming with the scroll wheel if Control is pressed
        float zMovement = 0f;
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            zMovement = Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
        }

        Vector3 move = transform.right * xMovement + transform.forward * zMovement + transform.up * yMovement;
        transform.position += move;

        if (Input.GetKeyDown(KeyCode.F))
        {
            ResetCameraPosition();
        }
    }

    /// <summary>
    /// Resets the camera position and rotation to its original state.
    /// </summary>
    public void ResetCameraPosition()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        yaw = originalRotation.eulerAngles.y;
        pitch = originalRotation.eulerAngles.x;
    }
}
