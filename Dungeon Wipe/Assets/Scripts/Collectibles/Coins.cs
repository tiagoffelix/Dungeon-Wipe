using UnityEngine;

/// <summary>
/// Handles coin behavior in the game world, including vertical movement and despawn timing.
/// </summary>
public class Coins : MonoBehaviour
{
    /// <summary>
    /// Reference to the coin's scriptable object.
    /// </summary>
    [SerializeField] private CoinsSO coinsSO;
    private float timeSinceSpawned;

    /// <summary>
    /// Reference to the particle system associated with the coin.
    /// </summary>
    [SerializeField] private ParticleSystem particles;

    /// <summary>
    /// Gets the coin's scriptable object instance.
    /// </summary>
    public CoinsSO CoinsSO { get { return coinsSO; } }

    /// <summary>
    /// Gets the time (in seconds) after which the coin will despawn.
    /// </summary>
    public float TimeToDespawn => coinsSO.TimeToDespawn;

    // Variables for vertical movement
    private float verticalSpeed = 3f;
    private float baseHeight; // Base height above the initial position

    /// <summary>
    /// Initializes the coin on spawn.
    /// </summary>
    private void Start()
    {
        timeSinceSpawned = 0f;
        var mainModule = particles.main;
        mainModule.duration = coinsSO.TimeToDespawn;
        particles.Play();
        baseHeight = transform.position.y;
    }

    /// <summary>
    /// Updates the coin behavior every frame.
    /// </summary>
    private void Update()
    {
        timeSinceSpawned += Time.deltaTime;

        float newY = baseHeight + 0.05f * Mathf.Sin(Time.time * verticalSpeed);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        if (timeSinceSpawned >= coinsSO.TimeToDespawn)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Performs cleanup when the coin item is destroyed.
    /// </summary>
    public void OnDestroy()
    {
        // Search for the FloorScript component in the parent or its children
        FloorScript floorScript = transform.parent.GetComponent<FloorScript>() ?? transform.parent.GetComponentInChildren<FloorScript>();

        floorScript.HasCollectible = false;
    }
}
