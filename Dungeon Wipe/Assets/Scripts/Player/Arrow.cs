using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private ParticleSystem particles;
    public float Damage { get; set; }

    private bool destroy;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(destroy && !particles.isPlaying) 
        {
            Destroy(gameObject);
        }   
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the arrow should stop (ignore collisions with Player and other Arrows)
        if (!collision.gameObject.CompareTag("Arrow"))
        {
            // Immediately stop all physics movement
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;

            GetComponent<Collider>().enabled = false;

            // Make this projectile a child of the object it hit
            transform.SetParent(collision.transform);

            if (collision.gameObject.GetComponent<Enemy>() != null)
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(Damage);
                particles.Play();
                destroy = true;
            }
            else if (collision.gameObject.GetComponent<PlayerMovement>() != null)
            {
                collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(Damage);
                particles.Play();
            }
        }
    }
}
