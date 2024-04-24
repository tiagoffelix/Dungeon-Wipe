using Unity.VisualScripting;
using UnityEngine;

public class PotionItem : MonoBehaviour
{
    [SerializeField] private SpeedPotion speedPotion;
    [SerializeField] private DamagePotion damagePotion;
    [SerializeField] private HealthPotion healthPotion;

    [SerializeField] private ParticleSystem particles;

    public SpeedPotion SpeedPotion
    {
        get { return speedPotion; }
    }

    // Public getter for DamagePotion
    public DamagePotion DamagePotion
    {
        get { return damagePotion; }
    }

    // Public getter for HealthPotion
    public HealthPotion HealthPotion
    {
        get { return healthPotion; }
    }

    private float timeToDespawn;
    private float timeSinceSpawned;

    // Variables for rotation and vertical movement
    private float rotationSpeed = 50f;
    private float verticalSpeed = 5f;
    private float baseHeight; // Base height above the initial position

    private void Start()
    {
        var mainModule = particles.main;
        if (speedPotion != null)
        {
            timeToDespawn = speedPotion.TimeToDespawn;
            mainModule.duration = speedPotion.TimeToDespawn;
        }
        else if (damagePotion != null)
        {
            timeToDespawn = damagePotion.TimeToDespawn;
            mainModule.duration = damagePotion.TimeToDespawn;
        }
        else if (healthPotion != null)
        {
            timeToDespawn = healthPotion.TimeToDespawn;
            mainModule.duration = healthPotion.TimeToDespawn;
        }
        else
        {
            Debug.LogWarning("No potion is set, destroying the object immediately.");
            Destroy(gameObject);
        }
        timeSinceSpawned = 0f;
        particles.Play();

        baseHeight = transform.position.y;
    }

    private void Update()
    {
        timeSinceSpawned += Time.deltaTime;

        // Rotate the object around its up axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Calculate new Y position within the range of [baseHeight+0.5, baseHeight+1]
        float newY = baseHeight + 0.5f + 0.25f * Mathf.Sin(Time.time * verticalSpeed);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        if (timeSinceSpawned >= timeToDespawn)
        {
            Destroy(gameObject);
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
