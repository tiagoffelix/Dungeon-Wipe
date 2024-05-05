using UnityEngine;

/// <summary>
/// Represents a type of enemy with specific attributes.
/// </summary>
[CreateAssetMenu(fileName = "NewEnemyType", menuName = "Enemy Type")]
public class EnemyType : ScriptableObject
{
    [SerializeField] private string typeName; // The name of the enemy type.
    [SerializeField] private float attackCooldown; // Cooldown between attacks.
    [SerializeField] private float attackRange; // Range of attack.
    [SerializeField] private int baseDamage; // Base damage inflicted by the enemy.
    [SerializeField] private int health; // Health points of the enemy.
    [SerializeField] private int points; // Points awarded for defeating the enemy.
    [SerializeField] private AudioClip attackSound; // Sound played when the enemy attacks.
    [SerializeField] private AudioClip deathSound; // Sound played when the enemy dies.

    /// <summary>
    /// Gets the name of the enemy type.
    /// </summary>
    public string TypeName
    {
        get { return typeName; }
    }

    /// <summary>
    /// Gets the cooldown between attacks for the enemy type.
    /// </summary>
    public float AttackCooldown
    {
        get { return attackCooldown; }
    }

    /// <summary>
    /// Gets the attack range of the enemy type.
    /// </summary>
    public float AttackRange
    {
        get { return attackRange; }
    }

    /// <summary>
    /// Gets the base damage inflicted by the enemy type.
    /// </summary>
    public int BaseDamage
    {
        get { return baseDamage; }
    }

    /// <summary>
    /// Gets the health points of the enemy type.
    /// </summary>
    public int Health
    {
        get { return health; }
    }

    /// <summary>
    /// Gets the points awarded for defeating the enemy type.
    /// </summary>
    public int Points
    {
        get { return points; }
    }

    /// <summary>
    /// Gets the sound played when the enemy type attacks.
    /// </summary>
    public AudioClip AttackSound
    {
        get { return attackSound; }
    }

    /// <summary>
    /// Gets the sound played when the enemy type dies.
    /// </summary>
    public AudioClip DeathSound
    {
        get { return deathSound; }
    }
}
