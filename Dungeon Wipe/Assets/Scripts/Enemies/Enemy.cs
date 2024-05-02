using UnityEngine;

public class Enemy : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;

    private bool isAttacking;

    [SerializeField] private Transform attackCenter;
    [SerializeField] private LayerMask enemyLayers;

    private float speed;
    private float gravity;
    private float cooldownTimer;

    private bool isGrounded;

    private Vector3 moveVelocity;

    private float distance;

    [SerializeField] private GameObject weapon;

    [SerializeField] private PlayerMovement player;

    [SerializeField] private EnemyType enemyType;

    [SerializeField] private ParticleSystem particles;
    [SerializeField] private ParticleSystem bloodParticles;

    private float health;

    private int points;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = PlayerMovement.Instance;
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        speed = 0.75f; 
        gravity = 1f;
        cooldownTimer = enemyType.AttackCooldown;
        isAttacking = false;
        moveVelocity = Vector3.zero;
        distance = 0;
        health = enemyType.Health;
        points = enemyType.Points;
    }

    private void Update()
    {
        isGrounded = characterController.isGrounded;

        if(!isGrounded) 
        {
            moveVelocity.y -= gravity * Time.deltaTime;
            characterController.Move(moveVelocity * Time.deltaTime);
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Death")
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("DeathFall")
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) 
        {
            distance = Vector3.Distance(player.transform.position, transform.position);

            Vector3 directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.y = 0; // Set the y-component to zero to keep the rotation horizontal
            transform.rotation = Quaternion.LookRotation(directionToPlayer);

            if (distance <= enemyType.AttackRange)
            {
                animator.SetFloat("Speed", 0);

                if (cooldownTimer > 0) { cooldownTimer -= Time.deltaTime; }

                if (cooldownTimer <= 0 && !isAttacking)
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
                if (!isAttacking)
                {
                    characterController.Move(directionToPlayer.normalized * speed * Time.deltaTime);
                    animator.SetFloat("Speed", speed);
                }
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                if (enemyType.TypeName == "Warrior"
                    && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f
                    && distance <= enemyType.AttackRange)
                {
                    audioSource.PlayOneShot(enemyType.AttackSound);
                    player.TakeDamage(enemyType.BaseDamage);
                    bloodParticles.Play();
                    animator.Play("Idle");
                }
                else if (enemyType.TypeName == "Mage" && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
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
        }
    }

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

    private void Death()
    {
        player.Danger = false;
        weapon.SetActive(false);
        animator.SetBool("Death", true);
        characterController.enabled = false;
        enabled = false;
        isAttacking = true;
        foreach (Collider col in GetComponents<Collider>())
        {
            col.enabled = false;
        }
        player.PlayerStats.Score += points;
        Destroy(gameObject, 15);
    }

    private void DeathFall()
    {
        player.Danger = false;
        weapon.SetActive(false);
        animator.SetBool("DeathFall", true);
        characterController.enabled = false;
        enabled = false;
        isAttacking = true;
        foreach (Collider col in GetComponents<Collider>())
        {
            col.enabled = false;
        }
        player.PlayerStats.Score += points;
        Destroy(gameObject, 15);
    }

    private void ResetCooldown() 
    {
        cooldownTimer = enemyType.AttackCooldown;
    }

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

    private void OnDrawGizmosSelected()
    {
        if (attackCenter) { Gizmos.DrawWireSphere(attackCenter.position, enemyType.AttackRange); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Spikes"))
        {
            DeathFall();
        }
    }
}

