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

    public bool Danger {  get ; set; }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        attackRange = 1.5f;
        nextAttackTimer = 0f;
        jumpHeight = 1f;
        gravityValue = -10f;
        speed = 5f;
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
    }

    void Update()
    {
        if (playerStats.Dead)
        {
            Death();
        }

        if(Danger) 
        {
            dangerIcon.enabled = true;
        }
        else 
        {
            dangerIcon.enabled = false;
        }

        if (sword.activeSelf && !isBlocking) 
        {
            crossbowImage.gameObject.SetActive(false);
            swordImage.gameObject.SetActive(true);
        }
        else if (bow.activeSelf && !isBlocking) 
        {
            swordImage.gameObject.SetActive(false);
            crossbowImage.gameObject.SetActive(true);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn"))
        {
            isSpawning = true;
        }
        else 
        {
            isSpawning = false;
        }

        if (!isSpawning) 
        {
            if(!canvas.isActiveAndEnabled) 
            {
                canvas.enabled = true;
            }

            healthBarImage.fillAmount = playerStats.Health / 100f;

            if (Input.GetKeyDown(KeyCode.P))
            {
                playerStats.Health = 100;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && !isAttacking)
            {
                bow.SetActive(true);   // Activate the bow
                sword.SetActive(false); // Deactivate the sword
            }

            if (Input.GetKeyDown(KeyCode.Alpha1) && !isAttacking)
            {
                sword.SetActive(true);  // Activate the sword
                bow.SetActive(false);    // Deactivate the bow
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
                /*if(!walkingSound.isPlaying) 
                {
                    walkingSound.Play();
                }*/
            }
            else
            {
                animator.SetFloat("Speed", 0);
                /*if(walkingSound.isPlaying) 
                {
                    walkingSound.Stop();
                }*/
            }

            if (Input.GetKey(KeyCode.C))
            {
                crosshair.enabled = false;
            }
            else 
            {
                // Attack Logic
                if (Time.time >= nextAttackTimer && Input.GetMouseButtonDown(0) && !isBlocking)
                {
                    if (sword.activeSelf)
                    {
                        Attack();
                    }
                    else if (bow.activeSelf)
                    {
                        /*if(!bowSound.isPlaying) 
                        {
                            bowSound.Play();
                        }*/
                        shotArrow = false;
                        animator.SetTrigger("Shoot");
                    }

                    nextAttackTimer = Time.time + 1f; // Cooldown time for next attack
                }
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f &&
                !shotArrow)
            {
                shotArrow = true;
                bow.GetComponent<CrossBow>().ShootProjectile(arrowDamage);
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Death")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            {
                isAttacking = true;
            }
            else
            {
                isAttacking = false;
            }

            if (Input.GetMouseButton(1) && !isAttacking)
            {
                animator.SetBool("Block", true);
                swordImage.gameObject.SetActive(false);
                crossbowImage.gameObject.SetActive(false);
                shieldImage.gameObject.SetActive(true);
            }
            else
            {
                shieldImage.gameObject.SetActive(false);
                blockingTime = 0f;
                animator.SetBool("Block", false);
                isBlocking = false;
                blockCounter = 0;
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Block") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f)
            {
                blockingTime += Time.deltaTime;
                isBlocking = true;
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                playerStats.Dead = true;
            }
        }
        else 
        {
            canvas.enabled = false;
        }
    }

    public void TakeDamage(float damage)
    {
        if (blockingTime < 0.3 && blockingTime > 0)
        {
            //parrySound.Play();
            animator.SetTrigger("Blocked");
        }
        else
        {
            if (isBlocking)
            {
                //shieldedSound.Play();
                animator.SetBool("Block", false);
                blockCounter++;
                if(blockCounter == 4) 
                {
                    //breakShieldSound.Play();
                    isBlocking = false;
                    animator.SetTrigger("Hit");
                    playerStats.TakeDamage(damage);
                }
            }
            else
            {
                //hitSound.Play();
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
        enabled = false;
        canvas.enabled = false;
    }

    private void DeathFall()
    {
        playerStats.TakeDamage(1000);
        healthBarImage.fillAmount = playerStats.Health / 100f;
        animator.SetBool("DeathFall", true);
        controller.enabled = false;
        canvas.enabled = false;
        enabled = false;
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
        /*if(!attackingSound.isPlaying) 
            {
                attackingSound.Play();
            }*/

        Collider[] hitEnemies = Physics.OverlapSphere(attackCenter.position, attackRange, enemyLayers);

        foreach (Collider enemyCollider in hitEnemies)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();

            if (enemy != null)
            {
                particles.Play();
                animator.SetFloat("Speed", 0);
                enemy.TakeDamage(damage);
                playerStats.Score += (int)damage;
                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackCenter.position, attackRange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Spikes"))
        {
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
                else
                {
                    Debug.LogWarning("No valid potion found on the object.");
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
