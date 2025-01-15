using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("General Variables")]
    public Vector3 spawnPoint;
    private Camera playerCam;

    [Header("Stat Variables")]
    public float damage = 3;
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
        if (isDead)
        {
            return;
        }
        HandleMovementInput();
        HandleBloodDrain();
        HandleSwordSwing();
    }

    private void FixedUpdate()
    {
        if (isDead)
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
        ChangeHealth(Time.deltaTime / bloodDrainDivider);
    }

    private void HandleSwordSwing()
    {
        if (swordSwingCDTimer > 0)
        {
            swordSwingCDTimer -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Rotate the sword swing transform towards the mouse
            Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                swordSwingRotator.LookAt(new Vector3(raycastHit.point.x, transform.position.y, raycastHit.point.z));
            }

            //Spawn the sword swing effect
            Vector3 spawnPosition = swordSwingRotator.position + swordSwingRotator.forward / 3;
            GameObject swordSwingInstance = Instantiate(swordSwingPrefab,
                spawnPosition, swordSwingRotator.rotation); ;
            swordSwingInstance.GetComponent<Projectile>().InitializeProjectile(this, damage);
            swordSwingInstance.GetComponent<MeleeAttackTrigger>().spawnTransform = swordSwingRotator;
            swordSwingInstance.GetComponent<MeleeAttackTrigger>().spawnOffset = swordSwingRotator.forward / 3;
            swordSwingCDTimer = swordSwingCooldown;
        }
    }
}
