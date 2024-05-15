using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the player's movement, attacks, and various interactions.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    private Animator animator; // Reference to the Animator component
    private CharacterController controller; // Reference to the CharacterController component
    private AudioSource audioSource; // Reference to the AudioSource component

    [SerializeField] private Transform attackCenter; // Center position for attack range
    [SerializeField] float attackRange; // Range of the attack
    [SerializeField] LayerMask enemyLayers; // Layer mask for enemies

    [SerializeField] private Stats playerStats; // Reference to the player's stats

    public Stats PlayerStats { get { return playerStats; } }

    private float nextAttackTimer; // Timer for attack cooldown
    private float jumpHeight; // Height of the player's jump
    private float gravityValue; // Gravity applied to the player
    private Vector3 playerVelocity; // Velocity of the player

    [SerializeField] private GameObject sword; // Sword GameObject for melee attacks
    [SerializeField] private GameObject bow; // Bow GameObject for ranged attacks

    [SerializeField] private Image healthBarImage; // UI Image for the health bar
    [SerializeField] private Canvas canvas; // UI Canvas for game HUD

    [SerializeField] private Image dangerIcon; // UI Image for danger indication

    private bool isBlocking; // Flag to indicate if the player is blocking
    private float blockingTime; // Timer for the duration of blocking
    private bool isAttacking; // Flag to indicate if the player is attacking
    private bool shotArrow; // Flag to indicate if an arrow has been shot

    private Coroutine currentSpeedBoostCoroutine; // Coroutine for speed boost
    private Coroutine currentDamageBoostCoroutine; // Coroutine for damage boost

    private float speed; // Movement speed of the player
    private float originalSpeed; // Original movement speed for resetting
    private float damage; // Damage amount for melee attacks
    private float arrowDamage; // Damage amount for ranged attacks

    [SerializeField] private Image crosshair; // UI Image for crosshair
    [SerializeField] private Image swordImage; // UI Image for sword indicator
    [SerializeField] private Image crossbowImage; // UI Image for crossbow indicator
    [SerializeField] private Image shieldImage; // UI Image for shield indicator
    [SerializeField] private Image strengthBoostImage; // UI Image for strength boost indicator
    [SerializeField] private Image speedBoostImage; // UI Image for speed boost indicator

    [SerializeField] private TextMeshProUGUI boostText; // UI Text for displaying boost effects

    [SerializeField] private ParticleSystem particles; // Particle system for effects
    private bool isSpawning; // Flag to indicate if the player is spawning

    private int blockCounter; // Counter for blocking attempts
    private bool deathFall; // Flag to indicate if the player has fallen to death

    public bool Danger { get; set; }

    public static PlayerMovement Instance { get; private set; }

    [SerializeField] private Image crackImage1; // UI Image for the first crack indicator
    [SerializeField] private Image crackImage2; // UI Image for the second crack indicator
    [SerializeField] private Image crackImage3; // UI Image for the third crack indicator

    [SerializeField] private AudioSource walkingSound;
    [SerializeField] private AudioSource swordSound;
    [SerializeField] private AudioSource slashSound;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource bowSound;
    [SerializeField] private AudioSource spikesSound;
    [SerializeField] private AudioSource parrySound;
    [SerializeField] private AudioSource shieldedSound;
    [SerializeField] private AudioSource shieldBreakSound;
    [SerializeField] private AudioSource drawSwordSound;

    /// <summary>
    /// Initializes the singleton instance and other components.
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Initializes player stats, movement properties, and equipment.
    /// </summary>
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();  
        attackRange = 0.375f;
        nextAttackTimer = 0f;
        jumpHeight = 0.2f;
        gravityValue = -2.5f;
        speed = 1.5f;
        playerStats.Health = 100;
        playerStats.Dead = false;
        isBlocking = false;
        blockingTime = 0f;
        isAttacking = false;
        shotArrow = false;
        isSpawning = true;
        damage = playerStats.Damage;
        arrowDamage = playerStats.ArrowDamage;
        blockCounter = 0;
        deathFall = false;
    }

    /// <summary>
    /// Handles player movement, attacks, and interactions based on input and conditions.
    /// </summary>
    void Update()
    {
        if (transform.position.y < -2)
        {
            if (!deathFall)
            {
                DeathFall();
            }
        }
        dangerIcon.enabled = Danger;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn") && isSpawning)
        {
            isSpawning = true;
        }
        else
        {
            isSpawning = false;
        }

        if (playerStats.Health <= 0)
        {
            if (!deathFall)
            {
                Death();
            }

            if ((animator.GetCurrentAnimatorStateInfo(0).IsName("DeathFall")
                || animator.GetCurrentAnimatorStateInfo(0).IsName("Death")) &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                playerStats.Dead = true;
            }
        }
        else if (!isSpawning)
        {
            if (!canvas.isActiveAndEnabled)
            {
                canvas.enabled = true;
            }

            healthBarImage.fillAmount = playerStats.Health / 100f;

            //TIRAR NA BUILD FINAL
            if (Input.GetKeyDown(KeyCode.P))
            {
                playerStats.Health = 100;
            }

            bool isGrounded = controller.isGrounded;
            playerVelocity.y += gravityValue * Time.deltaTime; // Apply gravity by default

            if (isGrounded && playerVelocity.y < 0)
            {
                playerVelocity.y = -0.4f; // A small negative value to stick the player to the ground
            }

            if (isGrounded)
            {
                animator.SetBool("Grounded", true);
            }
            else
            {
                animator.SetBool("Grounded", false);
                if (walkingSound.isPlaying)
                {
                    walkingSound.Stop();
                }
            }

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 movementDirection = transform.right * horizontalInput + transform.forward * verticalInput;

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                animator.SetTrigger("Jump");
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue); // Use -2f if using default Physics.gravity
            }

            // Movement
            controller.Move(movementDirection.normalized * speed * Time.deltaTime + playerVelocity * Time.deltaTime);

            if (movementDirection.magnitude > 0)
            {
                animator.SetFloat("Speed", 1.0f); // Normalize Speed to 1.0 when moving
                if (!walkingSound.isPlaying && isGrounded)
                {
                    walkingSound.Play();
                }
            }
            else
            {
                animator.SetFloat("Speed", 0);
                if (walkingSound.isPlaying)
                {
                    walkingSound.Stop();
                }
            }

            if (Input.GetKey(KeyCode.C))
            {
                crosshair.enabled = false;
            }
            else
            {
                crosshair.enabled = true;
                if (Input.GetMouseButton(1) && !isAttacking)
                {
                    isBlocking = true;
                    blockingTime += Time.deltaTime;
                    animator.SetBool("Block", true);
                    swordImage.gameObject.SetActive(false);
                    crossbowImage.gameObject.SetActive(false);
                    shieldImage.gameObject.SetActive(true);

                    // Reset attack-related states
                    isAttacking = false;
                    crosshair.enabled = false;
                }
                else
                {
                    blockingTime = 0f;
                    isBlocking = false;
                    animator.SetBool("Block", false);
                    shieldImage.gameObject.SetActive(false);
                    crosshair.enabled = true;

                    // Reset block counter and attack handling
                    blockCounter = 0;

                    if (Input.GetKeyDown(KeyCode.Alpha2) && !isAttacking && !isBlocking)
                    {
                        ActivateWeapon(false);
                    }

                    if (Input.GetKeyDown(KeyCode.Alpha1) && !isAttacking && !isBlocking)
                    {
                        ActivateWeapon(true);
                    }

                    if (Input.GetAxis("Mouse ScrollWheel") > 0f && !isBlocking && !sword.activeSelf) // Scroll up
                    {
                        ActivateWeapon(true); // true for sword
                    }
                    else if (Input.GetAxis("Mouse ScrollWheel") < 0f && !isBlocking && !bow.activeSelf) // Scroll down
                    {
                        ActivateWeapon(false); // false for bow
                    }

                    if (Input.GetMouseButtonDown(0) && !isBlocking && Time.time >= nextAttackTimer)
                    {
                        if (sword.activeSelf)
                        {
                            Attack();
                        }
                        else if (bow.activeSelf)
                        {
                            shotArrow = false;
                            animator.SetTrigger("Shoot");
                        }

                        nextAttackTimer = Time.time + playerStats.AttackCooldown;
                    }
                }
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f &&
                !shotArrow)
            {
                shotArrow = true;
                bowSound.Play();
                bow.GetComponent<CrossBow>().ShootProjectile(arrowDamage);
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Death")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("DeathFall")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            {
                isAttacking = true;
            }
            else
            {
                isAttacking = false;
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Block") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f)
            {
                blockingTime += Time.deltaTime;
                isBlocking = true;
            }
            else
            {
                isBlocking = false;
                blockCounter = 0;
            }

            switch (blockCounter)
            {
                case 0:
                    crackImage1.gameObject.SetActive(false);
                    crackImage2.gameObject.SetActive(false);
                    crackImage3.gameObject.SetActive(false);
                    break;
                case 1:
                    crackImage1.gameObject.SetActive(true);
                    crackImage2.gameObject.SetActive(false);
                    crackImage3.gameObject.SetActive(false);
                    break;
                case 2:
                    crackImage1.gameObject.SetActive(true);
                    crackImage2.gameObject.SetActive(true);
                    crackImage3.gameObject.SetActive(false);
                    break;
                case 3:
                    crackImage1.gameObject.SetActive(true);
                    crackImage2.gameObject.SetActive(true);
                    crackImage3.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
        else
        {
            canvas.enabled = false;
        }
    }

    /// <summary>
    /// Switches between sword and bow weapons.
    /// </summary>
    /// <param name="activateSword">If true, the sword will be activated, otherwise the bow will be activated.</param>
    private void ActivateWeapon(bool activateSword)
    {
        sword.SetActive(activateSword);
        bow.SetActive(!activateSword);

        crossbowImage.gameObject.SetActive(!activateSword);
        swordImage.gameObject.SetActive(activateSword);

        if (activateSword)
        {
            drawSwordSound.Play();
        }
    }

    /// <summary>
    /// Reduces the player's health based on the given damage, handling blocking and other effects.
    /// </summary>
    /// <param name="damage">Amount of damage to take.</param>
    public void TakeDamage(float damage)
    {
        if (blockingTime > 0 && blockingTime < 0.3)
        {
            parrySound.Play();
            animator.SetTrigger("Blocked");
        }
        else
        {
            if (isBlocking)
            {
                shieldedSound.Play();
                blockCounter++;
                if (blockCounter >= 3)
                {
                    shieldBreakSound.Play();
                    animator.SetBool("Block", false);
                    isBlocking = false;
                    blockCounter = 0;
                    animator.SetTrigger("Hit");
                    playerStats.TakeDamage(damage);
                }
            }
            else
            {
                hitSound.Play();
                animator.SetTrigger("Hit");
                playerStats.TakeDamage(damage);
            }
        }
    }

    /// <summary>
    /// Reduces the player's health by a specific amount, regardless of blocking.
    /// </summary>
    /// <param name="damage">Amount of damage to take.</param>
    public void TakeDamageMage(int damage)
    {
        playerStats.TakeDamage(damage);
    }

    /// <summary>
    /// Handles the player's death.
    /// </summary>
    private void Death()
    {
        animator.SetBool("Dead", true);
        controller.enabled = false;
        canvas.enabled = false;
        if (walkingSound.isPlaying)
        {
            walkingSound.Stop();
        }
    }

    /// <summary>
    /// Handles the player's death due to falling, or falling into spikes.
    /// </summary>
    private void DeathFall()
    {
        hitSound.Play();
        deathFall = true;
        playerStats.TakeDamage(1000);
        healthBarImage.fillAmount = playerStats.Health / 100f;
        animator.SetBool("DeathFall", true);
        controller.enabled = false;
        canvas.enabled = false;
        if (walkingSound.isPlaying)
        {
            walkingSound.Stop();
        }
    }

    /// <summary>
    /// Handles the player's melee attack.
    /// </summary>
    private void Attack()
    {
        animator.SetTrigger("Attack");

        Collider[] hitEnemies = Physics.OverlapSphere(attackCenter.position, attackRange, enemyLayers);

        foreach (Collider enemyCollider in hitEnemies)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();

            if (enemy != null)
            {
                swordSound.Play();
                particles.Play();
                animator.SetFloat("Speed", 0);
                enemy.TakeDamage(damage);
                playerStats.Score += (int)damage;
                break;
            }
        }
        if (hitEnemies.Length == 0) { slashSound.Play(); }
    }

    /// <summary>
    /// Draws the attack range gizmo in the editor for visualization.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackCenter.position, attackRange);
    }

    /// <summary>
    /// Handles collisions with various in-game objects, such as spikes, potions, and coins.
    /// </summary>
    /// <param name="other">The collider of the object collided with.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Spikes"))
        {
            spikesSound.Play();
            DeathFall();
        }
        if (other.gameObject.CompareTag("Potion"))
        {
            PotionItem potionItem = other.gameObject.GetComponent<PotionItem>();
            if (potionItem != null)
            {
                // Check each potion type and apply effects accordingly
                if (potionItem.SpeedPotion != null)
                {
                    StartCoroutine(ApplySpeedBoost(potionItem.SpeedPotion.SpeedBoost, potionItem.SpeedPotion.Duration));
                    if (potionItem.SpeedPotion.Sound != null)
                    {
                        audioSource.PlayOneShot(potionItem.SpeedPotion.Sound);
                    }
                    Destroy(other.gameObject);
                }
                else if (potionItem.DamagePotion != null)
                {
                    StartCoroutine(ApplyDamageBoost(potionItem.DamagePotion.DamageBoost, potionItem.DamagePotion.Duration));
                    if (potionItem.DamagePotion.Sound != null)
                    {
                        audioSource.PlayOneShot(potionItem.DamagePotion.Sound);
                    }
                    Destroy(other.gameObject);
                }
                else if (potionItem.HealthPotion != null)
                {
                    playerStats.AddHealth(potionItem.HealthPotion.Health);
                    if (potionItem.HealthPotion.Sound != null)
                    {
                        audioSource.PlayOneShot(potionItem.HealthPotion.Sound);
                    }
                    Destroy(other.gameObject);
                }
            }
        }
        if (other.gameObject.CompareTag("Coins"))
        {
            Coins coins = other.gameObject.GetComponent<Coins>();

            if (coins != null)
            {
                playerStats.Score += coins.CoinsSO.Score;
                if (coins.CoinsSO.Sound != null)
                {
                    AudioSource.PlayClipAtPoint(coins.CoinsSO.Sound, transform.position);
                }
                Destroy(other.gameObject);
            }
        }
    }

    /// <summary>
    /// Applies a damage boost to the player for a specified duration.
    /// </summary>
    /// <param name="multiplier">The multiplier to apply to the player's damage.</param>
    /// <param name="duration">The duration of the boost in seconds.</param>
    /// <returns>An IEnumerator for coroutine management.</returns>
    IEnumerator ApplyDamageBoost(float multiplier, float duration)
    {
        if (currentDamageBoostCoroutine != null)
        {
            StopCoroutine(currentDamageBoostCoroutine);
            boostText.gameObject.SetActive(false);
            strengthBoostImage.gameObject.SetActive(false);
            damage = playerStats.Damage;
            arrowDamage = playerStats.ArrowDamage;
        }

        if (currentSpeedBoostCoroutine != null)
        {
            StopCoroutine(currentSpeedBoostCoroutine);
            boostText.gameObject.SetActive(false);
            speedBoostImage.gameObject.SetActive(false);
            speed = originalSpeed;
        }

        currentDamageBoostCoroutine = StartCoroutine(ApplyDamageBoostCoroutine(multiplier, duration));
        yield return currentDamageBoostCoroutine;
    }

    /// <summary>
    /// Applies a speed boost to the player for a specified duration.
    /// </summary>
    /// <param name="multiplier">The multiplier to apply to the player's speed.</param>
    /// <param name="duration">The duration of the boost in seconds.</param>
    /// <returns>An IEnumerator for coroutine management.</returns>
    IEnumerator ApplySpeedBoost(float multiplier, float duration)
    {
        if (currentSpeedBoostCoroutine != null)
        {
            StopCoroutine(currentSpeedBoostCoroutine);
            boostText.gameObject.SetActive(false);
            speedBoostImage.gameObject.SetActive(false);
            speed = originalSpeed;
        }

        if (currentDamageBoostCoroutine != null)
        {
            StopCoroutine(currentDamageBoostCoroutine);
            boostText.gameObject.SetActive(false);
            strengthBoostImage.gameObject.SetActive(false);
            damage = playerStats.Damage;
            arrowDamage = playerStats.ArrowDamage;
        }

        currentSpeedBoostCoroutine = StartCoroutine(ApplySpeedBoostCoroutine(multiplier, duration));
        yield return currentSpeedBoostCoroutine;
    }

    /// <summary>
    /// Coroutine to handle applying a speed boost.
    /// </summary>
    /// <param name="multiplier">The multiplier to apply to the player's speed.</param>
    /// <param name="duration">The duration of the boost in seconds.</param>
    /// <returns>An IEnumerator for coroutine management.</returns>
    IEnumerator ApplySpeedBoostCoroutine(float multiplier, float duration)
    {
        boostText.gameObject.SetActive(true);
        boostText.text = "Speed " + multiplier + "x";
        speedBoostImage.gameObject.SetActive(true);
        originalSpeed = speed;
        speed *= multiplier; // Increase speed
        yield return new WaitForSeconds(duration); // Wait for the duration of the boost
        boostText.gameObject.SetActive(false);
        speed = originalSpeed; // Reset speed to normal after duration ends
        speedBoostImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// Coroutine to handle applying a damage boost.
    /// </summary>
    /// <param name="multiplier">The multiplier to apply to the player's damage.</param>
    /// <param name="duration">The duration of the boost in seconds.</param>
    /// <returns>An IEnumerator for coroutine management.</returns>
    IEnumerator ApplyDamageBoostCoroutine(float multiplier, float duration)
    {
        boostText.gameObject.SetActive(true);
        boostText.text = "Damage " + multiplier + "x";
        strengthBoostImage.gameObject.SetActive(true);
        damage *= multiplier; // Increase damage
        arrowDamage *= multiplier; // Increase damage
        yield return new WaitForSeconds(duration); // Wait for the duration of the boost
        boostText.gameObject.SetActive(false);
        strengthBoostImage.gameObject.SetActive(false);
        damage = playerStats.Damage;
        arrowDamage = playerStats.ArrowDamage;
    }
}
