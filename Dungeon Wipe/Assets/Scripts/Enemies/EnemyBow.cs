using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a bow weapon used by an enemy to shoot projectiles.
/// </summary>
public class EnemyBow : MonoBehaviour
{
    /// <summary>
    /// The projectile prefab to instantiate when shooting.
    /// </summary>
    [SerializeField] private GameObject projectilePrefab;

    /// <summary>
    /// The point from which projectiles will be shot.
    /// </summary>
    [SerializeField] private Transform shootingPoint;

    /// <summary>
    /// The speed at which the projectile will move.
    /// </summary>
    private float projectileSpeed;

    /// <summary>
    /// Initializes the projectile speed.
    /// </summary>
    void Start()
    {
        projectileSpeed = 10f;
    }

    /// <summary>
    /// Shoots a projectile towards the specified target position with the given damage value.
    /// </summary>
    /// <param name="targetPosition">The target position to shoot the projectile towards.</param>
    /// <param name="damage">The amount of damage the projectile will deal.</param>
    public void ShootProjectile(Vector3 targetPosition, int damage)
    {
        // Modify target position to be 1 unit above the original target position
        Vector3 adjustedTargetPosition = new Vector3(targetPosition.x, targetPosition.y + 0.6f, targetPosition.z);

        if (projectilePrefab && shootingPoint)
        {
            // Calculate the shooting direction based on adjusted target position
            Vector3 shootingDirection = (adjustedTargetPosition - shootingPoint.position).normalized;

            // Instantiate the projectile at the shooting point with corrected orientation
            GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.LookRotation(shootingDirection) * Quaternion.Euler(0, 180, 0));

            // Set the damage of the projectile
            projectile.GetComponent<Arrow>().Damage = damage;

            // Set the projectile's velocity to move towards the target
            projectile.GetComponent<Rigidbody>().velocity = shootingDirection * projectileSpeed;

            // Destroy the projectile after 20 seconds to clean up
            Destroy(projectile, 20.0f);
        }
    }
}
