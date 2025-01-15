using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [Header("General Variables")]
    [SerializeField] private GameObject bloodOrbPrefab;
    [SerializeField] private int bloodOrbsOnDeath;
    [SerializeField] private float bloodOrbSpeed = 5;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool shouldMove = true;
    private Player player;
    private Rigidbody rb;

    [Header("Combat Variables")]
    [SerializeField] private GameObject enemyAttackPrefab;
    [SerializeField] private Transform enemyAttackRotator;
    [SerializeField] private float attackCDTimer;
    [SerializeField] private float attackCD;
    [SerializeField] private float damage;
    [SerializeField] private float aggroRange;
    [SerializeField] private float attackRange;
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
            Vector3 diff = player.transform.position - transform.position;
            rb.velocity = new Vector3(diff.x, 0, diff.z).normalized * moveSpeed;
        }
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
        StartCoroutine(HandleAttackCheck());
        enemyAttackRotator.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

        //Spawn the attack effect
        GameObject swordSwingInstance = Instantiate(enemyAttackPrefab,
            enemyAttackRotator.position + enemyAttackRotator.forward / 2.5f, enemyAttackRotator.rotation, transform);
        swordSwingInstance.GetComponent<Projectile>().InitializeProjectile(this, damage);
        swordSwingInstance.GetComponent<MeleeAttackTrigger>().spawnTransform = enemyAttackRotator;
        swordSwingInstance.GetComponent<MeleeAttackTrigger>().spawnOffset = enemyAttackRotator.forward / 2.5f;
        attackCDTimer = attackCD;
    }

    private IEnumerator HandleAttackCheck()
    {        
        isAttacking = true;
        //Attack duration is half the attack cooldown
        yield return new WaitForSeconds(attackCD / 2);
        isAttacking = false;
    }

    public override IEnumerator DeathSequence()
    {
        DropBloodOrbs();
        StartCoroutine(base.DeathSequence());
        yield return null;
    }

    private void DropBloodOrbs()
    {
        for (int i = 0; i < bloodOrbsOnDeath; i++)
        {
            Vector3 bloodOrbMoveDirection = Random.insideUnitCircle.normalized * bloodOrbSpeed;
            bloodOrbMoveDirection.y = Random.Range(10, 15);
            GameObject currentOrbInstance = Instantiate(bloodOrbPrefab, transform.position, Quaternion.identity);
            currentOrbInstance.GetComponent<Rigidbody>().velocity = bloodOrbMoveDirection;
        }
    }
}
