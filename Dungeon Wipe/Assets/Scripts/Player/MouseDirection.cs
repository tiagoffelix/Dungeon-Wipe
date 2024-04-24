using UnityEngine;

public class MouseDirection : MonoBehaviour
{
    [SerializeField] private Stats playerStats;

    [SerializeField] private Transform player;

    private float xRotation;

    private void Start()
    {
        xRotation = 0f;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * playerStats.Sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * playerStats.Sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -15f, 45f);

        transform.localEulerAngles = Vector3.right * xRotation;
        player.Rotate(Vector3.up * mouseX);
    }
}