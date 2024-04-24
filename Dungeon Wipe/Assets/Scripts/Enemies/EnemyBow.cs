using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBow : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab; // The projectile prefab
    [SerializeField] private Transform shootingPoint; // The point from which projectiles will be shot
    private float projectileSpeed; // Speed at which the projectile will move, initialized directly in Start

    void Start()
    {
        projectileSpeed = 30f; // Initialize the projectile speed
    }

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

            // Assuming there is a component named 'Arrow' attached to the projectilePrefab that manages its damage
            projectile.GetComponent<Arrow>().Damage = damage;

            // Apply the velocity to the projectile's Rigidbody component to move it in the shooting direction
            projectile.GetComponent<Rigidbody>().velocity = shootingDirection * projectileSpeed;

            // Destroy the projectile after 20 seconds to clean up
            Destroy(projectile, 20.0f);
        }
    }
}
