using UnityEngine;

/// <summary>
/// Handles potion item behavior in the game world, including movement, despawn timing, and particle effects.
/// </summary>
public class PotionItem : MonoBehaviour
{
    /// <summary>
    /// Reference to a speed potion.
    /// </summary>
    [SerializeField] private SpeedPotion speedPotion;

    /// <summary>
    /// Reference to a damage potion.
    /// </summary>
    [SerializeField] private DamagePotion damagePotion;

    /// <summary>
    /// Reference to a health potion.
    /// </summary>
    [SerializeField] private HealthPotion healthPotion;

    /// <summary>
    /// Reference to the particle system associated with the potion.
    /// </summary>
    [SerializeField] private ParticleSystem particles;

    /// <summary>
    /// Gets the speed potion instance.
    /// </summary>
    public SpeedPotion SpeedPotion => speedPotion;

    /// <summary>
    /// Gets the damage potion instance.
    /// </summary>
    public DamagePotion DamagePotion => damagePotion;

    /// <summary>
    /// Gets the health potion instance.
    /// </summary>
    public HealthPotion HealthPotion => healthPotion;

    /// <summary>
    /// Gets the time (in seconds) after which the potion will despawn.
    /// </summary>
    public float TimeToDespawn => speedPotion?.TimeToDespawn ?? damagePotion?.TimeToDespawn ?? healthPotion?.TimeToDespawn ?? 0;

    private float timeSinceSpawned;

    // Variables for rotation and vertical movement
    private float rotationSpeed = 50f;
    private float verticalSpeed = 3f;
    private float baseHeight;

    /// <summary>
    /// Initializes the potion item on spawn.
    /// </summary>
    private void Start()
    {
        var mainModule = particles.main;
        mainModule.duration = TimeToDespawn;
        timeSinceSpawned = 0f;
        particles.Play();
        baseHeight = transform.position.y;
    }

    /// <summary>
    /// Updates the potion behavior every frame.
    /// </summary>
    private void Update()
    {
        timeSinceSpawned += Time.deltaTime;

        // Rotates the potion item
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Adds a floating effect
        float newY = baseHeight + 0.05f * Mathf.Sin(Time.time * verticalSpeed);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Checks if it's time to despawn the potion
        if (timeSinceSpawned >= TimeToDespawn)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Performs cleanup when the potion item is destroyed.
    /// </summary>
    public void OnDestroy()
    {
        GetComponentInParent<FloorScript>().HasCollectible = false;
    }
}
