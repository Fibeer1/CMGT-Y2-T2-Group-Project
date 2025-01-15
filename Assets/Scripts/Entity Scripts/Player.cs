using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("General Variables")]
    public Vector3 spawnPoint;

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

    private void Awake()
    {
        base.InitializeEntity();
        maxHealth = health;
        spawnPoint = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }
        HandleMovementInput();
        HandleBloodDrain();
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
}
