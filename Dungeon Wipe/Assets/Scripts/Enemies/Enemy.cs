using UnityEngine;
using UnityEngine.AI; // Include the AI namespace

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;

    [SerializeField] private Transform attackCenter;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private GameObject weapon;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private ParticleSystem bloodParticles;

    private bool isAttacking;
    private float health;
    private int points;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        agent.speed = 0.75f;
        agent.stoppingDistance = enemyType.AttackRange;

        health = enemyType.Health;
        points = enemyType.Points;
    }

    void Update()
    {
        if (agent.pathPending || Vector3.Distance(transform.position, player.transform.position) > agent.stoppingDistance)
        {
            agent.SetDestination(player.transform.position);
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }

        CheckAttack();
    }

    private void CheckAttack()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= enemyType.AttackRange && !isAttacking)
        {
            // Perform attack
            isAttacking = true;
            agent.isStopped = true; // Stop the agent when attacking
            animator.SetTrigger("Attack");
            // Additional attack logic here
        }
        else if (isAttacking)
        {
            // Reset attack state
            isAttacking = false;
            agent.isStopped = false;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) Death();
    }

    private void Death()
    {
        // Death logic here
        player.PlayerStats.Score += points;
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        if (attackCenter) Gizmos.DrawWireSphere(attackCenter.position, enemyType.AttackRange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Spikes"))
        {
            Death();
        }
    }
}
