using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an arrow projectile that can cause damage upon collision.
/// </summary>
public class Arrow : MonoBehaviour
{
    private Rigidbody rb; // Rigidbody component for physics handling

    /// <summary>
    /// Particle system to play effects on collision.
    /// </summary>
    [SerializeField] private ParticleSystem particles;

    /// <summary>
    /// The damage value that the arrow will inflict upon collision.
    /// </summary>
    public float Damage { get; set; }

    private bool destroy; // Determines if the arrow should be destroyed

    /// <summary>
    /// Initializes the Rigidbody component reference.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Checks if the arrow should be destroyed after particle effects stop.
    /// </summary>
    private void Update()
    {
        if (destroy && !particles.isPlaying)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Handles collision events for the arrow.
    /// </summary>
    /// <param name="collision">The collision information.</param>
    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Arrow"))
        {
            // Stop physics simulation for this arrow
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;

            GetComponent<Collider>().enabled = false;

            // Parent the arrow to the object it collided with
            transform.SetParent(collision.transform);

            // Handle collision with enemy
            if (collision.gameObject.GetComponent<Enemy>() != null)
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(Damage);
                particles.Play();
                destroy = true;
            }
            // Handle collision with player
            else if (collision.gameObject.GetComponent<PlayerMovement>() != null)
            {
                collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(Damage);
                particles.Play();
            }
        }
    }
}
