using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossBow : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab; // The projectile prefab
    [SerializeField] private Transform shootingPoint; // The point from which projectiles will be shot
    [SerializeField] private Camera mainCamera; // Main camera in the scene, could be a player or an overview camera
    [SerializeField] private Camera secondaryCamera; // A secondary camera, could be used for aiming or special views
    private float projectileSpeed; // Speed at which the projectile will move

    void Start()
    {
        projectileSpeed = 60f; // Initialize the projectile speed
    }

    Camera GetActiveCamera()
    {
        // Checks which camera is currently active by comparing their enabled state
        if (mainCamera != null && mainCamera.gameObject.activeSelf && mainCamera.enabled)
        {
            return mainCamera;
        }
        else if (secondaryCamera != null && secondaryCamera.gameObject.activeSelf && secondaryCamera.enabled)
        {
            return secondaryCamera;
        }
        return null; // Return null if no camera is active
    }

    public void ShootProjectile(float damage)
    {
        Camera activeCamera = GetActiveCamera(); // Get the currently active camera

        if (projectilePrefab != null && shootingPoint != null && activeCamera != null)
        {
            // Create a ray from the center of the screen
            Ray ray = activeCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit))
            {
                // If the ray hits something in the scene, use the hit point as the target
                targetPoint = hit.point;
            }
            else
            {
                // If nothing is hit, use a point far away in the direction of the ray
                targetPoint = ray.origin + ray.direction * 1000;
            }

            Vector3 shootingDirection = (targetPoint - shootingPoint.position).normalized;

            // Instantiate the projectile at the shooting point with orientation towards the shooting direction
            GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.LookRotation(shootingDirection));

            projectile.GetComponent<Arrow>().Damage = damage;

            // Apply the velocity to the projectile's Rigidbody component to move it in the shooting direction
            projectile.GetComponent<Rigidbody>().velocity = shootingDirection * projectileSpeed;

            Destroy(projectile, 20.0f); // Optionally adjust the time after which the projectile is destroyed
        }
    }
}
