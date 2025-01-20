using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    [Header("General Variables")]
    [SerializeField] private protected bool shouldMove = true;
    private protected Player player;

    [Header("Material Drop Variables")]
    [SerializeField] private GameObject bloodOrbPrefab;
    [SerializeField] private int bloodOrbsOnDeath;
    [SerializeField] private float pickupableSpawnSpeed = 5;
    [SerializeField] private GameObject[] materialPrefabs;
    [SerializeField] private int[] materialChances;

    [Header("Combat Variables")]
    [SerializeField] private GameObject enemyAttackPrefab;
    [SerializeField] private Transform enemyAttackRotator;
    [SerializeField] private protected float damage;
    [SerializeField] private float attackCDTimer;
    [SerializeField] private float attackCD;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackRangeOffset = 0.2f;
    [SerializeField] private float minAttackDistance = 0.9f;
    [SerializeField] private float attackDelay = 0.1f;
    [SerializeField] private protected float aggroRange = 10f;
    [SerializeField] private protected float deAggroRange = 20f;
    [SerializeField] private protected bool aggroed = false;
    [SerializeField] private protected bool isAttacking = false;
    private protected float distanceToPlayer;

    private NavMeshAgent meshAgent;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        meshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        HandlePlayerTargeting();
    }

    private protected void HandlePlayerTargeting()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < aggroRange && shouldMove)
        {
            aggroed = true;
        }
        if (distanceToPlayer > deAggroRange)
        {
            aggroed = false;
            meshAgent.destination = transform.position;
        }
        if (aggroed)
        {
            HandleAggroState();
        }
        if (attackCDTimer > 0)
        {
            attackCDTimer -= Time.deltaTime;
        }
    }

    public virtual void HandleAggroState()
    {
        meshAgent.SetDestination(player.transform.position);
        if (distanceToPlayer < attackRange)
        {
            shouldMove = false;
            meshAgent.destination = transform.position;
            AttackPlayer();
        }
        else if (!isAttacking)
        {
            shouldMove = true;
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
        Vector3 objectMoveDirection = Random.insideUnitSphere.normalized * pickupableSpeed;
        objectMoveDirection.y = Random.Range(minYVelocity, maxYVelocity);
        GameObject currentOrbInstance = Instantiate(pickupablePrefab, transform.position, Quaternion.identity);
        currentOrbInstance.GetComponent<Rigidbody>().velocity = objectMoveDirection;
    }
}
