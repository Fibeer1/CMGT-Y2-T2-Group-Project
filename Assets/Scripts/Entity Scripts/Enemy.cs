using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMODUnity;

public class Enemy : Entity
{
    [Header("General Variables")]
    [SerializeField] private protected bool shouldMove = true;
    public Player player;
    public EnemySpawner originSpawner;
    private protected EnemyAnimator animator;

    [Header("Material Drop Variables")]
    [SerializeField] private GameObject bloodOrbPrefab;
    [SerializeField] private int bloodOrbsOnDeath;
    [SerializeField] private float pickupableSpawnSpeed = 5;
    [SerializeField] private GameObject[] materialPrefabs;
    [SerializeField] private int[] materialChances;
    private bool hasSpawnedPickupables = false;

    [Header("Combat Variables")]
    [SerializeField] private GameObject enemyAttackPrefab;
    public Transform enemyAttackRotator;
    [SerializeField] private protected float damage;
    [SerializeField] private Transform attackParent;
    [SerializeField] private float attackCDTimer;
    [SerializeField] private float attackCD;
    [SerializeField] private float attackDuration;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackOffset = 0.2f;
    [SerializeField] private float closeAttackOffset = 0.5f;
    [SerializeField] private float normalAttackOffset = 0.5f;
    [SerializeField] private float minFleeDistance = 0.9f;
    [SerializeField] private float fleeMoveSpeed;
    [SerializeField] private float normalMoveSpeed;
    [SerializeField] private float attackDelay = 0.1f;
    [SerializeField] private protected float aggroRange = 10f;
    public float deAggroRange = 20f;
    public bool aggroed = false;
    [SerializeField] private bool attackTriggeredThroughAnim = false;
    [SerializeField] private protected bool isAttacking = false;
    private protected float distanceToPlayer;
    public Vector3 targetPosition;

    [Header("Sound Variables")]
    [SerializeField] private EventReference enemyAttackSound;
    [SerializeField] private EventReference enemyDyingSound;


    public NavMeshAgent meshAgent;

    private void Start()
    {
        InitializeEntity();
    }

    public override void InitializeEntity()
    {
        player = FindObjectOfType<Player>();
        meshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<EnemyAnimator>();
        attackOffset = normalAttackOffset;
        meshAgent.speed = normalMoveSpeed;
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }
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
            targetPosition = transform.position;
            meshAgent.SetDestination(targetPosition);
        }
        if (aggroed)
        {
            HandleAggroState();
        }
        if (isAttacking)
        {
            targetPosition = transform.position;
            meshAgent.SetDestination(targetPosition);
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
            if (distanceToPlayer < minFleeDistance)
            {
                //Move away from the player when he is too close
                attackOffset = closeAttackOffset;
                Vector3 diff = transform.position - player.transform.position;
                targetPosition = transform.position + diff;
                meshAgent.SetDestination(targetPosition);
                meshAgent.speed = fleeMoveSpeed;
            }
            else
            {
                targetPosition = transform.position;
                meshAgent.SetDestination(targetPosition);
                attackOffset = normalAttackOffset;
                meshAgent.speed = normalMoveSpeed;
            }
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
        if (isAttacking)
        {
            yield break;
        }
        isAttacking = true;
        attackCDTimer = attackCD;
        if (animator != null)
        {
             StartCoroutine(animator.SpecialAnimation(animator.attackAnim, animator.attackAnimDuration));
        }        
        yield return new WaitForSeconds(attackDelay);
        enemyAttackRotator.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

        if (!attackTriggeredThroughAnim)
        {
            //Spawn the attack effect
            GameObject enemyAttackInstance = Instantiate(enemyAttackPrefab,
                enemyAttackRotator.position + enemyAttackRotator.forward * attackOffset, enemyAttackRotator.rotation, attackParent);
            enemyAttackInstance.GetComponent<Projectile>().InitializeProjectile(this, damage);
            AudioManager.instance.PlayOneShot(enemyAttackSound, transform.position);
        }

        //Attack duration is half the attack cooldown
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
    }

    public void SpawnAttackEffect()
    {
        if (!attackTriggeredThroughAnim)
        {
            return;
        }
        GameObject enemyAttackInstance = Instantiate(enemyAttackPrefab,
            enemyAttackRotator.position + enemyAttackRotator.forward * attackOffset, enemyAttackRotator.rotation, attackParent);
        enemyAttackInstance.GetComponent<Projectile>().InitializeProjectile(this, damage);
        AudioManager.instance.PlayOneShot(enemyAttackSound, transform.position);
    }



    public override IEnumerator DeathSequence()
    {
        if (isDead)
        {
            yield break;
        }
        AudioManager.instance.PlayOneShot(enemyDyingSound, transform.position);

        GameManager.enemies.Remove(this);
        if (originSpawner != null)
        {
            originSpawner.enemies.Remove(this);
        }
        DropPickupables();
        StartCoroutine(base.DeathSequence());
    }

    private void DropPickupables()
    {
        if (hasSpawnedPickupables)
        {
            return;
        }
        hasSpawnedPickupables = true;
        for (int i = 0; i < bloodOrbsOnDeath; i++)
        {
            LaunchPickupable(bloodOrbPrefab, pickupableSpawnSpeed);
        }
        for (int i = 0; i < materialPrefabs.Length; i++)
        {
            DropMaterial(i);
        }
    }

    private void DropMaterial(int materialIndex)
    {
        int randomNumber = Random.Range(1, 101); //E.g. 1 to 100 -> 1% chance
        if (randomNumber >= materialChances[materialIndex])
        {
            return;
        }
        LaunchPickupable(materialPrefabs[materialIndex], pickupableSpawnSpeed);
    }

    private void LaunchPickupable(GameObject pickupablePrefab, float pickupableSpeed)
    {
        float angle = Random.Range(-360, 360);
        Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
        GameObject currentPickupableInstance = Instantiate(pickupablePrefab, transform.position, Quaternion.identity);
        currentPickupableInstance.GetComponent<Rigidbody>().velocity = direction * pickupableSpeed;
    }
}
