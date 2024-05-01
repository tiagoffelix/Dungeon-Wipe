using UnityEngine;

public class EditorCameraController : MonoBehaviour
{
    private float movementSpeed = 10f;
    private float rotationSpeed = 5f;

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

        // Handle movement
        float xMovement = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        float zMovement = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        float yMovement = 0;

        // Ascend or descend with E and Q keys
        if (Input.GetKey(KeyCode.E))
        {
            yMovement = movementSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            yMovement = -movementSpeed * Time.deltaTime;
        }

        Vector3 move = transform.right * xMovement + transform.up * yMovement + transform.forward * zMovement;
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
