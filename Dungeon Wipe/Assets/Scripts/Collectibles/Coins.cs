using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    [SerializeField] private CoinsSO coinsSO;
    private float timeSinceSpawned;

    [SerializeField] private ParticleSystem particles;

    public CoinsSO CoinsSO { get { return coinsSO; } }

    // Variables for vertical movement
    private float verticalSpeed = 5f;
    private float baseHeight; // Base height above the initial position

    private void Start()
    {
        timeSinceSpawned = 0f;
        var mainModule = particles.main;
        mainModule.duration = coinsSO.TimeToDespawn;
        particles.Play();
        baseHeight = transform.position.y;
    }

    private void Update()
    {
        timeSinceSpawned += Time.deltaTime; // Increment the timer by the time between frames


        // Calculate new Y position within the range of [baseHeight+0.5, baseHeight+1]
        float newY = baseHeight + 0.5f + 0.25f * Mathf.Sin(Time.time * verticalSpeed);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Check if the time since spawned is greater or equal to the time to despawn
        if (timeSinceSpawned >= coinsSO.TimeToDespawn)
        {
            Destroy(gameObject); // Destroy the coin object
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coins") || other.gameObject.CompareTag("Potion") || other.gameObject.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Coins") || other.gameObject.CompareTag("Potion") || other.gameObject.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }
}
