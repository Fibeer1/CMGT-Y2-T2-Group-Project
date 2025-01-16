using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [Header("General Variables")]    
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool shouldMove = true;
    private Player player;
    private Rigidbody rb;

    [Header("Material Drop Variables")]
    [SerializeField] private GameObject bloodOrbPrefab;
    [SerializeField] private int bloodOrbsOnDeath;
    [SerializeField] private float pickupableSpawnSpeed = 5;
    [SerializeField] private GameObject[] materialPrefabs;
    [SerializeField] private int[] materialChances;

    [Header("Combat Variables")]
    [SerializeField] private GameObject enemyAttackPrefab;
    [SerializeField] private Transform enemyAttackRotator;
    [SerializeField] private float attackCDTimer;
    [SerializeField] private float attackCD;
    [SerializeField] private float damage;
    [SerializeField] private float aggroRange = 10f;
    [SerializeField] private float deAggroRange = 20f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackRangeOffset = 0.2f;
    [SerializeField] private float attackDelay = 0.1f;
    [SerializeField] private bool aggroed = false;
    [SerializeField] private bool isAttacking = false;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandlePlayerTargeting();
    }

    private void HandlePlayerTargeting()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < aggroRange && shouldMove)
        {
            aggroed = true;
            
        }
        if (distance > deAggroRange)
        {
            aggroed = false;
            rb.velocity = Vector3.zero;
        }
        if (aggroed)
        {
            Vector3 diff = player.transform.position - transform.position;
            rb.velocity = new Vector3(diff.x, 0, diff.z).normalized * moveSpeed;
            if (distance < attackRange)
            {
                shouldMove = false;
                rb.velocity = Vector3.zero;
                AttackPlayer();
            }
            else if (!isAttacking)
            {
                shouldMove = true;
            }
        }
        if (attackCDTimer > 0)
        {
            attackCDTimer -= Time.deltaTime;
        }
    }

    private void AttackPlayer()
    {
        if (attackCDTimer > 0)
        {
            return;
        }
        StartCoroutine(HandleAttack());        
    }

    private IEnumerator HandleAttack()
    {        
        isAttacking = true;
        attackCDTimer = attackCD;
        yield return new WaitForSeconds(attackDelay);
        enemyAttackRotator.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

        //Spawn the attack effect
        GameObject swordSwingInstance = Instantiate(enemyAttackPrefab,
            enemyAttackRotator.position + enemyAttackRotator.forward * attackRangeOffset, enemyAttackRotator.rotation, transform);
        swordSwingInstance.GetComponent<Projectile>().InitializeProjectile(this, damage);
        
        //Attack duration is half the attack cooldown
        yield return new WaitForSeconds(attackCD / 2);
        isAttacking = false;
    }

    public override IEnumerator DeathSequence()
    {
        if (isDead)
        {
            yield break;
        }
        DropPickupables();
        StartCoroutine(base.DeathSequence());
    }

    private void DropPickupables()
    {
        for (int i = 0; i < bloodOrbsOnDeath; i++)
        {
            LaunchPickupable(bloodOrbPrefab, pickupableSpawnSpeed, 2.5f, 7.5f);
        }
        for (int i = 0; i < materialPrefabs.Length; i++)
        {
            DropMaterial(i);
        }
    }

    private void DropMaterial(int materialIndex)
    {
        int randomNumber = Random.Range(materialChances[materialIndex], 100); //E.g. 1 to 100 -> 1% chance
        if (randomNumber != materialChances[materialIndex])
        {
            return;
        }
        LaunchPickupable(materialPrefabs[materialIndex], pickupableSpawnSpeed, 2.5f, 7.5f);
    }

    private void LaunchPickupable(GameObject pickupablePrefab, float pickupableSpeed, float minYVelocity, float maxYVelocity)
    {
        Vector3 objectMoveDirection = Random.insideUnitCircle.normalized * pickupableSpeed;
        objectMoveDirection.y = Random.Range(minYVelocity, maxYVelocity);
        GameObject currentOrbInstance = Instantiate(pickupablePrefab, transform.position, Quaternion.identity);
        currentOrbInstance.GetComponent<Rigidbody>().velocity = objectMoveDirection;
    }
}
