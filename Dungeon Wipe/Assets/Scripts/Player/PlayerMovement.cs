using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    private CharacterController controller;

    [SerializeField] private Transform attackCenter;
    [SerializeField] float attackRange;
    [SerializeField] LayerMask enemyLayers;

    [SerializeField] private Stats playerStats;

    public Stats PlayerStats { get { return playerStats; } }

    private float nextAttackTimer;

    private float jumpHeight;
    private float gravityValue;
    private Vector3 playerVelocity;

    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject bow;

    [SerializeField] private Image healthBarImage;
    [SerializeField] private Canvas canvas;

    [SerializeField] private Image dangerIcon;

    private bool isBlocking;
    private float blockingTime;
    private bool isAttacking;
    private bool shotArrow;

    private Coroutine currentSpeedBoostCoroutine;
    private Coroutine currentDamageBoostCoroutine;

    private float speed;
    private float originalSpeed;
    private float damage;
    private float arrowDamage;

    [SerializeField] private Image crosshair;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image crossbowImage;
    [SerializeField] private Image shieldImage;
    [SerializeField] private Image strengthBoostImage;
    [SerializeField] private Image speedBoostImage;

    [SerializeField] private TextMeshProUGUI boostText;

    [SerializeField] private ParticleSystem particles;
    private bool isSpawning;

    private int blockCounter;

    private bool deathFall;

    public bool Danger { get; set; }

    public static PlayerMovement Instance { get; private set; }

    private AudioSource audioSource;

    [SerializeField] private AudioSource walkingSound;

    [SerializeField] private Image crackImage1;
    [SerializeField] private Image crackImage2;
    [SerializeField] private Image crackImage3;


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
        audioSource = GetComponent<AudioSource>();
        walkingSound.clip = playerStats.WalkingSound;
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
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

    void Update()
    {
        if (transform.position.y < -2)
        {
            if (!deathFall)
            {
                DeathFall();
            }
        }
        if (Danger)
        {
            dangerIcon.enabled = true;
        }
        else
        {
            dangerIcon.enabled = false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn"))
        {
            isSpawning = true;
        }
        else
        {
            isSpawning = false;
        }

        if (playerStats.Health <= 0)
        {
            if(!deathFall) 
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
                audioSource.PlayOneShot(playerStats.BowSound);
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

    private void ActivateWeapon(bool activateSword)
    {
        sword.SetActive(activateSword);
        bow.SetActive(!activateSword);

        crossbowImage.gameObject.SetActive(!activateSword);
        swordImage.gameObject.SetActive(activateSword);

        if (activateSword)
        {
            audioSource.PlayOneShot(playerStats.DrawSwordSound);
        }
    }

    public void TakeDamage(float damage)
    {
        if (blockingTime > 0 && blockingTime < 0.3)
        {
            audioSource.PlayOneShot(playerStats.ParrySound);
            animator.SetTrigger("Blocked");
        }
        else
        {
            if (isBlocking)
            {
                audioSource.PlayOneShot(playerStats.ShieldedSound);
                blockCounter++;
                if (blockCounter >= 3)
                {
                    audioSource.PlayOneShot(playerStats.ShieldBreakSound);
                    animator.SetBool("Block", false);
                    isBlocking = false;
                    blockCounter = 0;
                    animator.SetTrigger("Hit");
                    playerStats.TakeDamage(damage);
                }
            }
            else
            {
                audioSource.PlayOneShot(playerStats.HitSound);
                animator.SetTrigger("Hit");
                playerStats.TakeDamage(damage);
            }
        }
    }

    public void TakeDamageMage(int damage)
    {
        playerStats.TakeDamage(damage);
    }

    private void Death()
    {
        animator.SetBool("Dead", true);
        controller.enabled = false;
        canvas.enabled = false;
    }

    private void DeathFall()
    {
        audioSource.PlayOneShot(playerStats.HitSound);
        deathFall = true;
        playerStats.TakeDamage(1000);
        healthBarImage.fillAmount = playerStats.Health / 100f;
        animator.SetBool("DeathFall", true);
        controller.enabled = false;
        canvas.enabled = false;
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");

        Collider[] hitEnemies = Physics.OverlapSphere(attackCenter.position, attackRange, enemyLayers);

        foreach (Collider enemyCollider in hitEnemies)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();

            if (enemy != null)
            {
                audioSource.Stop();
                audioSource.PlayOneShot(playerStats.SwordSound);
                particles.Play();
                animator.SetFloat("Speed", 0);
                enemy.TakeDamage(damage);
                playerStats.Score += (int)damage;
                break;
            }
        }
        if(hitEnemies.Length == 0) { audioSource.PlayOneShot(playerStats.SlashSound); }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackCenter.position, attackRange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Spikes"))
        {
            audioSource.PlayOneShot(playerStats.SpikesSound);
            DeathFall();
        }
        if (other.gameObject.CompareTag("Potion"))
        {
            PotionItem potionItem = other.gameObject.GetComponent<PotionItem>();
            if (potionItem != null)
            {
                audioSource.PlayOneShot(playerStats.CoinsSound);
                // Check each potion type and apply effects accordingly
                if (potionItem.SpeedPotion != null)
                {
                    StartCoroutine(ApplySpeedBoost(potionItem.SpeedPotion.SpeedBoost, potionItem.SpeedPotion.Duration));
                    if (potionItem.SpeedPotion.Sound != null)
                    {
                        AudioSource.PlayClipAtPoint(potionItem.SpeedPotion.Sound, transform.position);
                    }
                    Destroy(other.gameObject);
                }
                else if (potionItem.DamagePotion != null)
                {
                    StartCoroutine(ApplyDamageBoost(potionItem.DamagePotion.DamageBoost, potionItem.DamagePotion.Duration));
                    if (potionItem.DamagePotion.Sound != null)
                    {
                        AudioSource.PlayClipAtPoint(potionItem.DamagePotion.Sound, transform.position);
                    }
                    Destroy(other.gameObject);
                }
                else if (potionItem.HealthPotion != null)
                {
                    playerStats.AddHealth(potionItem.HealthPotion.Health);
                    if (potionItem.HealthPotion.Sound != null)
                    {
                        AudioSource.PlayClipAtPoint(potionItem.HealthPotion.Sound, transform.position);
                    }
                    Destroy(other.gameObject);
                }
            }
        }
        if (other.gameObject.CompareTag("Coins"))
        {
            Coins coins = other.gameObject.GetComponent<Coins>();

            audioSource.PlayOneShot(playerStats.CoinsSound);

            if (coins != null)
            {
                playerStats.Score += coins.CoinsSO.Score;
                if (coins.CoinsSO.Sound != null)
                {
                    coins.CoinsSO.Sound.Play();
                }
                Destroy(other.gameObject);
            }
        }
    }

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

    IEnumerator ApplySpeedBoostCoroutine(float multiplier, float duration)
    {
        boostText.gameObject.SetActive(true);
        boostText.text = "Speed " + multiplier + "x";
        speedBoostImage.gameObject.SetActive(true);
        originalSpeed = speed;
        speed *= multiplier;  // Increase speed
        yield return new WaitForSeconds(duration);  // Wait for the duration of the boost
        boostText.gameObject.SetActive(false);
        speed = originalSpeed;  // Reset speed to normal after duration ends
        speedBoostImage.gameObject.SetActive(false);
    }

    IEnumerator ApplyDamageBoostCoroutine(float multiplier, float duration)
    {
        boostText.gameObject.SetActive(true);
        boostText.text = "Damage " + multiplier + "x";
        strengthBoostImage.gameObject.SetActive(true);
        damage *= multiplier; // Increase damage
        arrowDamage *= multiplier; // Increase damage
        yield return new WaitForSeconds(duration);  // Wait for the duration of the boost
        boostText.gameObject.SetActive(false);
        strengthBoostImage.gameObject.SetActive(false);
        damage = playerStats.Damage;
        arrowDamage = playerStats.ArrowDamage;
    }
}
