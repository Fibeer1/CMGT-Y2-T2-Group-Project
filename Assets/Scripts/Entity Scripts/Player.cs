using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{    
    [Header("General Variables")]
    public Vector3 spawnPoint;
    private Camera playerCam;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private LayerMask groundLayer;

    [Header("Stat Variables")]
    public float meleeDamage = 25;
    public float rangedDamage = 50f;
    public float dashProjectileDamage = 50f;
    public float speed = 3f;

    [Header("Keybindings")]
    [SerializeField] private KeyCode dashKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode rangedAttackKey = KeyCode.Alpha1;
    [SerializeField] private KeyCode shieldKey = KeyCode.Alpha2;

    [Header("Movement Variables")]
    private float horizontal;
    private float vertical;
    private Rigidbody rb;

    [Header("Blood Drain Variables")]
    [SerializeField] private bool shouldDrainBlood = true;
    [SerializeField] private float healthDrainPercent = 0.01f; //1% of max health per tick
    [SerializeField] private float healthDrainTickRate = 0.1f;
    private float healthDrainTimer;

    [Header("Sword Swing Variables")]
    [SerializeField] private GameObject swordSwingPrefab;
    [SerializeField] private Transform swordSwingRotator;
    public float swordSwingCDTimer;
    public float swordSwingCD = 0.75f;
    [SerializeField] private float attackRangeOffset = 0.4f;
    [SerializeField] private bool autoswing = false;

    [Header("Ability Variables")]

    [Header("Ranged Attack Variables")]
    [SerializeField] private GameObject rangedProjectilePrefab;
    [SerializeField] private Transform shootRotator;
    [SerializeField] private float rangedAttackOffset;
    public float rangedAttackCDTimer;
    public float rangedAttackCD;
    public float rangedAttackCost = 0.1f; //% of current health

    [Header("Dash Variables")]   
    public bool isDashing;
    private bool canDash = true;
    [SerializeField] private float dashingPower = 16f;
    public float dashingTime = 0.2f;
    public float dashCDTimer = 1f;
    public float dashCD = 1f;
    public float dashHealthCost = 0.06f;
    public Enemy closestEnemy;
    [SerializeField] private GameObject dashProjectile;

    [Header("Shield Variables")]
    [SerializeField] private GameObject shieldPrefab;
    private Shield currentShieldInstance;
    public float percentageMissingHealthShield = 0.25f; //% of missing health
    public float fixedShieldAmount = 10;
    [SerializeField] private float shieldDuration;
    public float shieldCDTimer;
    public float shieldCD;
    public float shieldMaxHPCost = 0.45f; //% of max health

    [Header("Material Variables")]
    public int[] materialCounts;

    private void Awake()
    {
        base.InitializeEntity();
        maxHealth = health;
        spawnPoint = transform.position;
        playerCam = FindObjectOfType<Camera>();
        rb = GetComponent<Rigidbody>();
        swordSwingCDTimer = swordSwingCD;
        healthDrainTimer = healthDrainTickRate;
    }

    private void Update()
    {
        if (isDashing || isDead)
        {
            return;
        }
        HandleMovementInput();        
        HandleBloodDrain();
        HandleShooting();
        HandleSwordSwing();        
        HandleShieldMechanics();
        HandleDashing();
    }

    private void FixedUpdate()
    {
        if (isDashing || isDead)
        {
            return;
        }
        rb.velocity = new Vector3(horizontal, 0, vertical).normalized * speed;
    }

    public void UpdatePlayerStats(float maxHealthGrowth, float armorGrowth, float moveSpeedGrowth, float damageGrowth)
    {
        maxHealth += maxHealthGrowth;
        armor -= armorGrowth;
        speed += moveSpeedGrowth;
        meleeDamage += damageGrowth;

        health += maxHealthGrowth;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    private void HandleMovementInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    private void HandleBloodDrain()
    {
        if (!shouldDrainBlood || healthDrainTimer > 0)
        {
            healthDrainTimer -= Time.deltaTime;
            return;
        }
        healthDrainTimer = healthDrainTickRate;
        float healthChange = maxHealth * healthDrainPercent;
        ChangeHealth(healthChange, false, false, false, false);
    }

    public override void ChangeHealth(float healthChangeValue, bool shieldDamage = true, 
        bool shouldAccountForArmor = true, bool shouldDisplayDamageText = true, bool shouldPlaySound = true)
    {
        //If healthChange > 0 the entity loses health and vice versa
        if (healthChangeValue > 0 && shieldDamage)
        {
            if (currentShieldInstance != null)
            {
                float remainingShieldHealth = currentShieldInstance.shieldHealth - healthChangeValue;
                if (remainingShieldHealth <= 0)
                {
                    DestroyShield();
                    base.ChangeHealth(-remainingShieldHealth);
                    return;
                }
                currentShieldInstance.shieldHealth = remainingShieldHealth;
            }
            else
            {
                base.ChangeHealth(healthChangeValue, false, shouldAccountForArmor, 
                    shouldDisplayDamageText, shouldPlaySound);
            }
        }
        else
        {
            base.ChangeHealth(healthChangeValue, false, shouldAccountForArmor, 
                shouldDisplayDamageText, shouldPlaySound);
        }
    }

    public override IEnumerator DeathSequence()
    {
        if (isDead)
        {
            yield break;
        }
        isDead = true;
        Instantiate(deathEffect, transform.position, Quaternion.identity);        
        playerSprite.enabled = false;
        health = 0;
        rb.velocity = Vector2.zero;
        GetComponent<Collider>().enabled = false;
        if (currentShieldInstance != null)
        {
            currentShieldInstance.gameObject.SetActive(false);
        }
        yield return new WaitForSecondsRealtime(1);
        GameOverScreen.DeathAnimation();
        enabled = false;
    }

    private void HandleShooting()
    {
        if (rangedAttackCDTimer > 0)
        {
            rangedAttackCDTimer -= Time.deltaTime;
            return;
        }
        if (Input.GetKeyDown(rangedAttackKey))
        {
            Shoot(rangedProjectilePrefab, rangedDamage);
        }
    }

    private void Shoot(GameObject projectilePrefab, float projectileDamage, bool shouldDrainHealth = true, Transform target = null)
    {
        if (shouldDrainHealth)
        {
            float healthCost = health * rangedAttackCost;
            if (healthCost > health)
            {
                return;
            }
            rangedAttackCDTimer = rangedAttackCD;
            ChangeHealth(healthCost, false, false, false);
        }
        
        if (target != null)
        {
            //Rotate the shooter transform towards the target
            shootRotator.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        }
        else
        {
            Vector3 targetDirection = Vector3.zero;
            //Rotate the shooter transform towards the mouse
            Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000, groundLayer))
            {
                targetDirection = new Vector3(raycastHit.point.x, shootRotator.position.y, raycastHit.point.z);
            }
            shootRotator.LookAt(targetDirection);
        }
        

        //Spawn the projectile
        Vector3 spawnPosition = shootRotator.position + shootRotator.forward * rangedAttackOffset;
        GameObject projectileInstance = Instantiate(projectilePrefab,
            spawnPosition, shootRotator.rotation);
        projectileInstance.GetComponent<Projectile>().InitializeProjectile(this, projectileDamage);
    }

    private void HandleSwordSwing()
    {
        if (swordSwingCDTimer > 0)
        {
            swordSwingCDTimer -= Time.deltaTime;
            return;
        }
        if (autoswing && Input.GetKey(KeyCode.Mouse0))
        {
            SwingSword();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SwingSword();
        }
    }

    private void SwingSword()
    {
        //Rotate the sword swing transform towards the mouse
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000, groundLayer))
        {
            swordSwingRotator.LookAt(new Vector3(raycastHit.point.x, transform.position.y, raycastHit.point.z));
        }

        //Spawn the sword swing effect
        Vector3 spawnPosition = swordSwingRotator.position + swordSwingRotator.forward * attackRangeOffset;
        GameObject swordSwingInstance = Instantiate(swordSwingPrefab,
            spawnPosition, swordSwingRotator.rotation, transform);
        swordSwingInstance.GetComponent<Projectile>().InitializeProjectile(this, meleeDamage);
        swordSwingCDTimer = swordSwingCD;
    }

    private void HandleShieldMechanics()
    {
        if (shieldCDTimer > 0 || currentShieldInstance != null)
        {
            shieldCDTimer -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(shieldKey))
        {
            GenerateShield();
        }
    }

    private void GenerateShield()
    {        
        float missingHP = (maxHealth - health) * percentageMissingHealthShield;
        float healthCost = maxHealth * shieldMaxHPCost;
        if (healthCost > health)
        {
            return;
        }
        shieldCDTimer = shieldCD;
        float shieldHealth = fixedShieldAmount + missingHP;
        ChangeHealth(healthCost, false, false, false);
        currentShieldInstance = Instantiate(shieldPrefab, transform.position, 
            Quaternion.identity, transform).GetComponent<Shield>();        
        currentShieldInstance.InitializeShield(this, shieldHealth, shieldDuration);
    }

    public void DestroyShield()
    {
        Destroy(currentShieldInstance.gameObject);
        currentShieldInstance = null;
    }

    private void HandleDashing()
    {        
        if (Input.GetKeyDown(dashKey) && canDash)
        {
            float healthCost = health * dashHealthCost;
            float remainingHealth = health - healthCost;
            if (remainingHealth <= 0)
            {
                Debug.Log("Not enough health to dash.");
                return;
            }
            ChangeHealth(healthCost, false, false, false);
            StartCoroutine(Dash());
        }
        //Purely used for the HUD
        if (dashCDTimer > 0)
        {
            dashCDTimer -= Time.deltaTime;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector3(horizontal, 0, vertical).normalized * dashingPower;
        yield return new WaitForSeconds(dashingTime);
        if (closestEnemy != null)
        {
            Shoot(dashProjectile, dashProjectileDamage, false, closestEnemy.transform);
        }
        dashCDTimer = dashCD;
        isDashing = false;
        rb.velocity = new Vector3(horizontal, 0, vertical).normalized * speed;
        yield return new WaitForSeconds(dashCD);
        canDash = true;
    }
}
