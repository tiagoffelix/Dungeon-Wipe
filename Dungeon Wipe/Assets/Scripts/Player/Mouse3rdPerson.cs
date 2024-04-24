using UnityEngine;

public class Mouse3rdPerson : MonoBehaviour
{
    [SerializeField] private Stats playerStats;

    [SerializeField] private Transform player;


    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * playerStats.Sensitivity * Time.deltaTime;

        player.Rotate(Vector3.up * mouseX);
    }
}