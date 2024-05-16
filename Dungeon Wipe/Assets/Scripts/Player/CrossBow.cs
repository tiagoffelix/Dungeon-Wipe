using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the shooting mechanics of the crossbow.
/// </summary>
public class CrossBow : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab; // The projectile prefab
    [SerializeField] private Transform shootingPoint; // The point from which projectiles will be shot
    [SerializeField] private Camera mainCamera; // Main camera in the scene, could be a player or an overview camera
    [SerializeField] private Camera secondaryCamera; // A secondary camera, could be used for aiming or special views
    private float projectileSpeed; // Speed at which the projectile will move

    /// <summary>
    /// Checks if the arrow was shot by the Player.
    /// </summary>
    public bool ShotByPlayer { get; set; }

    /// <summary>
    /// Initializes the projectile speed.
    /// </summary>
    void Start()
    {
        ShotByPlayer = false;
        projectileSpeed = 10f;
    }

    /// <summary>
    /// Gets the active camera, either main or secondary.
    /// </summary>
    /// <returns>The active camera or null if none are active.</returns>
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

    /// <summary>
    /// Shoots a projectile forward from the shooting point.
    /// </summary>
    /// <param name="damage">The damage value the projectile will inflict.</param>
    public void ShootProjectile(float damage)
    {
        if (projectilePrefab != null && shootingPoint != null)
        {
            Camera activeCamera = GetActiveCamera(); // Get the currently active camera
            if (activeCamera != null)
            {
                // Rotate the shooting point to align with the camera's forward direction
                shootingPoint.rotation = Quaternion.LookRotation(activeCamera.transform.forward);
            }

            Vector3 shootingDirection = shootingPoint.forward;

            // Instantiate the projectile at the shooting point with orientation towards the shooting direction
            GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.LookRotation(shootingDirection));

            projectile.GetComponent<Arrow>().Damage = damage;
            projectile.GetComponent<Arrow>().SetShotByPlayer();

            // Apply velocity to the projectile's Rigidbody component to move it in the shooting direction
            projectile.GetComponent<Rigidbody>().velocity = shootingDirection * projectileSpeed;

            // Optionally adjust the time after which the projectile is destroyed
            Destroy(projectile, 20.0f);
        }
    }
}
