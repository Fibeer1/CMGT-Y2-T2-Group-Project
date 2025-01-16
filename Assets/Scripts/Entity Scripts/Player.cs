using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("General Variables")]
    public Vector3 spawnPoint;
    private Camera playerCam;

    [Header("Stat Variables")]
    public float meleeDamage = 25;
    public float rangedDamage = 50f;
    [SerializeField] private float speed = 3f;

    [Header("Movement Variables")]
    private float horizontal;
    private float vertical;
    private Rigidbody rb;

    [Header("Blood Drain Variables")]
    [SerializeField] private bool shouldDrainBlood = true;
    [SerializeField] private float bloodDrainDivider = 3f;

    [Header("Sword Swing Variables")]
    [SerializeField] private GameObject swordSwingPrefab;
    [SerializeField] private Transform swordSwingRotator;
    [SerializeField] private float swordSwingCDTimer;
    [SerializeField] private float swordSwingCooldown = 0.75f;
    [SerializeField] private float attackRangeOffset = 0.4f;
    [SerializeField] private bool autoswing = false;

    [Header("Ability Variables")]

    [Header("Ranged Attack Variables")]
    [SerializeField] private GameObject rangedProjectilePrefab;
    [SerializeField] private Transform shootRotator;
    [SerializeField] private float rangedAttackOffset;
    [SerializeField] private float rangedAttackCDTimer;
    [SerializeField] private float rangedAttackCD;
    [SerializeField] private float rangedAttackCost = 0.2f; //% of current health

    [Header("Dash Variables")]   
    public bool isDashing;
    private bool canDash = true;
    [SerializeField] private float dashingPower = 16f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;
    [SerializeField] private ParticleSystem afterimage;

    [Header("Shield Variables")]
    [SerializeField] private GameObject shieldPrefab;
    private Shield currentShieldInstance;
    public float percentageMissingHealthShield = 0.25f; //% of missing health
    public float fixedShieldAmount = 10;
    [SerializeField] private float shieldDuration;
    [SerializeField] private float shieldCDTimer;
    [SerializeField] private float shieldCD;
    [SerializeField] private float shieldMaxHPCost = 0.45f; //% of max health

    [Header("Material Variables")]
    public int tier1MaterialCount;
    public int tier2MaterialCount;
    public int tier3MaterialCount;


    private void Awake()
    {
        base.InitializeEntity();
        maxHealth = health;
        spawnPoint = transform.position;
        playerCam = FindObjectOfType<Camera>();
        rb = GetComponent<Rigidbody>();
        swordSwingCDTimer = swordSwingCooldown;
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

    private void HandleMovementInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    private void HandleBloodDrain()
    {
        if (!shouldDrainBlood)
        {
            return;
        }
        //If bloodDrainDivider is 3, health loss will be 0.3/sec
        ChangeHealth(Time.deltaTime / bloodDrainDivider, false);
    }

    public override void ChangeHealth(float healthChangeValue, bool shieldDamage = true)
    {
        if (shieldDamage)
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
                base.ChangeHealth(healthChangeValue, false);
            }
        }
        else
        {
            base.ChangeHealth(healthChangeValue, false);
        }
    }

    private void HandleShooting()
    {
        if (rangedAttackCDTimer > 0)
        {
            rangedAttackCDTimer -= Time.deltaTime;
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        //Rotate the shooter transform towards the mouse
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            shootRotator.LookAt(new Vector3(raycastHit.point.x, transform.position.y, raycastHit.point.z));
        }
        float healthCost = health * rangedAttackCost;
        if (healthCost > health)
        {
            return;
        }
        ChangeHealth(healthCost, false);
        //Spawn the projectile
        Vector3 spawnPosition = shootRotator.position + shootRotator.forward * rangedAttackOffset;
        GameObject rangedProjectileInstance = Instantiate(rangedProjectilePrefab,
            spawnPosition, shootRotator.rotation); ;
        rangedProjectileInstance.GetComponent<Projectile>().InitializeProjectile(this, rangedDamage);
        rangedAttackCDTimer = rangedAttackCD;
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
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            swordSwingRotator.LookAt(new Vector3(raycastHit.point.x, transform.position.y, raycastHit.point.z));
        }

        //Spawn the sword swing effect
        Vector3 spawnPosition = swordSwingRotator.position + swordSwingRotator.forward * attackRangeOffset;
        GameObject swordSwingInstance = Instantiate(swordSwingPrefab,
            spawnPosition, swordSwingRotator.rotation, transform);
        swordSwingInstance.GetComponent<Projectile>().InitializeProjectile(this, meleeDamage);
        swordSwingCDTimer = swordSwingCooldown;
    }

    private void HandleShieldMechanics()
    {
        if (shieldCDTimer > 0 || currentShieldInstance != null)
        {
            shieldCDTimer -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
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
        float shieldHealth = fixedShieldAmount + missingHP;
        ChangeHealth(healthCost);
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
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        //hud.dashCooldown = 0;
        afterimage.Play();
        rb.velocity = new Vector3(horizontal, 0, vertical).normalized * dashingPower;
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        rb.velocity = new Vector3(horizontal, 0, vertical).normalized * speed;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        afterimage.Stop();
    }
}
