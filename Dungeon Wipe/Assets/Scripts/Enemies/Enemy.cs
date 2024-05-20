using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Represents an enemy in the game, handling its behaviors like movement, attacking, and death.
/// </summary>
public class Enemy : MonoBehaviour
{
    private Animator animator;

    private bool isAttacking;

    /// <summary>
    /// The center of the attack range for the enemy.
    /// </summary>
    [SerializeField] private Transform attackCenter;

    /// <summary>
    /// The layer mask used to detect enemies.
    /// </summary>
    [SerializeField] private LayerMask enemyLayers;
    private float cooldownTimer;

    private float distance;

    /// <summary>
    /// The weapon used by the enemy.
    /// </summary>
    [SerializeField] private GameObject weapon;

    /// <summary>
    /// The player movement script.
    /// </summary>
    [SerializeField] private PlayerMovement player;

    /// <summary>
    /// The type of enemy, which defines its attributes.
    /// </summary>
    [SerializeField] private EnemyType enemyType;

    /// <summary>
    /// The particle system for visual effects.
    /// </summary>
    [SerializeField] private ParticleSystem particles;

    /// <summary>
    /// The particle system used for blood effects.
    /// </summary>
    [SerializeField] private ParticleSystem bloodParticles;

    private float health;

    private int points;

    private AudioSource audioSource;
    private NavMeshAgent agent;
    private bool playerSighted;

    /// <summary>
    /// Initializes the enemy properties.
    /// </summary>
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        player = PlayerMovement.Instance;
        animator = GetComponent<Animator>();
        agent.speed = 0.75f;
        cooldownTimer = enemyType.AttackCooldown;
        isAttacking = false;
        distance = 0;
        health = enemyType.Health;
        points = enemyType.Points;
    }

    /// <summary>
    /// Updates the enemy behavior every frame.
    /// </summary>
    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")
        || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")
        || animator.GetCurrentAnimatorStateInfo(0).IsName("Death")
        || animator.GetCurrentAnimatorStateInfo(0).IsName("DeathFall")
        || animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn"))
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }

        if (isAttacking)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Death")
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("DeathFall")
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn"))
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;

            distance = directionToPlayer.magnitude;

            int obstacleLayerMask = LayerMask.GetMask("Obstacle");
            int enemyLayerMask = LayerMask.GetMask("Enemy");

            // Combine masks for 'Obstacle' and 'Enemy'
            int combinedMask = obstacleLayerMask | enemyLayerMask;

            // Bitwise invert to ignore both 'Obstacle' and 'Enemy' layers
            combinedMask = ~combinedMask;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, Mathf.Infinity, combinedMask))
            {
                if (hit.collider.gameObject == player.gameObject)
                {
                    playerSighted = true;
                }
                else
                {
                    playerSighted = false;
                }
            }
            print("Distance: " + distance + " Attack Range: " + enemyType.AttackRange + " " +playerSighted);
            if (distance <= enemyType.AttackRange && playerSighted)
            {
                directionToPlayer.y = 0;
                transform.rotation = Quaternion.LookRotation(directionToPlayer);

                agent.isStopped = true;
                animator.SetFloat("Speed", 0);

                if (cooldownTimer > 0) { cooldownTimer -= Time.deltaTime; }

                if (cooldownTimer <= 0 && !isAttacking && player.PlayerStats.Health > 0)
                {
                    player.Danger = true;
                    ResetCooldown();

                    if (enemyType.TypeName == "Warrior")
                    {
                        Attack();
                    }
                    else if (enemyType.TypeName == "Archer")
                    {
                        animator.SetTrigger("Attack");
                        particles.Play();
                        audioSource.PlayOneShot(enemyType.AttackSound);
                        Vector3 shootingPosition = player.transform.position;
                        shootingPosition.y -= 0.5f;
                        weapon.GetComponent<EnemyBow>().ShootProjectile(shootingPosition, enemyType.BaseDamage);
                    }
                    else if (enemyType.TypeName == "Mage")
                    {
                        particles.Play();
                        audioSource.PlayOneShot(enemyType.AttackSound);
                        animator.SetTrigger("Attack");
                    }
                }
            }
            else
            {
                MoveToAttackPosition();
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                if (enemyType.TypeName == "Warrior"
                    && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f
                    && distance <= enemyType.AttackRange && player.PlayerStats.Health > 0)
                {
                    audioSource.PlayOneShot(enemyType.AttackSound);
                    player.TakeDamage(enemyType.BaseDamage);
                    bloodParticles.Play();
                    animator.Play("Idle");
                }
                else if (enemyType.TypeName == "Mage" && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f && player.PlayerStats.Health > 0)
                {
                    player.TakeDamageMage(enemyType.BaseDamage);
                    animator.Play("Idle");
                }
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
                {
                    player.Danger = false;
                }
                ResetCooldown();
            }
        }

    }

    /// <summary>
    /// Finds the nearest point on the NavMesh to a given position within the specified attack range.
    /// </summary>
    /// <param name="targetPosition">The target position to find the nearest NavMesh point.</param>
    /// <param name="attackRange">The attack range within which to find the NavMesh point.</param>
    /// <returns>The closest position on the NavMesh within the specified attack range from the target position. Returns null if no valid point is found.</returns>
    public Vector3? FindNearestPointOnNavMesh(Vector3 targetPosition)
    {
        NavMeshHit hit;
        float searchRadius = 10.0f; // A larger search radius to find possible NavMesh points
        float stepSize = 1.0f; // Step size for sampling positions

        for (float radius = 0; radius <= searchRadius; radius += stepSize)
        {
            // Check if there is a valid NavMesh position within the current radius of the target position
            if (NavMesh.SamplePosition(targetPosition, out hit, radius, NavMesh.AllAreas))
            {
                // Check if the point is within the attack range from the target position
                if (Vector3.Distance(hit.position, targetPosition) <= enemyType.AttackRange)
                {
                    return hit.position;
                }
            }
        }

        // No valid NavMesh point was found within the search radius and attack range
        return null;
    }

    /// <summary>
    /// Moves the agent to the closest position where it can attack, or stops if no valid point is found.
    /// </summary>
    public void MoveToAttackPosition()
    {
        Vector3? nearestPoint = FindNearestPointOnNavMesh(player.transform.position);

        if (nearestPoint.HasValue)
        {
            agent.isStopped = false;
            animator.SetFloat("Speed", agent.speed);
            agent.SetDestination(nearestPoint.Value);
        }
        else
        {
            animator.SetFloat("Speed", 0);
            agent.isStopped = true; // Stop the agent if no valid point is found within the attack range
        }
    }

    /// <summary>
    /// Reduces the enemy's health by the specified damage amount.
    /// </summary>
    /// <param name="damage">The amount of damage to deal to the enemy.</param>
    public void TakeDamage(float damage)
    {
        animator.SetTrigger("Hit");
        audioSource.PlayOneShot(enemyType.DeathSound);
        health -= damage;
        if (health <= 0)
        {
            Death();
        }
    }

    /// <summary>
    /// Handles the death of the enemy, triggering animations and disabling components.
    /// </summary>
    private void Death()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.PlayOneShot(enemyType.DeathSound);
        agent.isStopped = true;
        agent.enabled = false;
        player.Danger = false;
        weapon.SetActive(false);
        animator.SetBool("Death", true);
        enabled = false;
        isAttacking = true;
        foreach (Collider col in GetComponents<Collider>())
        {
            col.enabled = false;
        }
        player.PlayerStats.Score += points;
        Destroy(gameObject, 15);
    }

    /// <summary>
    /// Resets the cooldown timer for the enemy's attack.
    /// </summary>
    private void ResetCooldown()
    {
        cooldownTimer = enemyType.AttackCooldown;
    }

    /// <summary>
    /// Performs a melee attack, detecting players within the attack range.
    /// </summary>
    private void Attack()
    {
        Collider[] hitPlayers = Physics.OverlapSphere(attackCenter.position, enemyType.AttackRange, enemyLayers);

        foreach (Collider playerCollider in hitPlayers)
        {
            PlayerMovement player = playerCollider.GetComponent<PlayerMovement>();

            if (player != null)
            {
                isAttacking = true;
                animator.SetTrigger("Attack");
                animator.SetFloat("Speed", 0);
                particles.Play();
                break;
            }
        }
    }

    /// <summary>
    /// Draws the attack range in the Unity editor for debugging.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (attackCenter) { Gizmos.DrawWireSphere(attackCenter.position, enemyType.AttackRange); }
    }
}
