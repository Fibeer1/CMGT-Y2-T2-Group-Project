using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miniboss : Enemy
{
    [SerializeField] private GameObject rockAttackIndicator;
    [SerializeField] private GameObject rockAttackPrefab;
    [SerializeField] private float rockAttackCDTimer;
    [SerializeField] private float rockAttackCD;
    [SerializeField] private float rockAttackSpawnDelay;
    private PlayerAbilityUnlocker abilityUnlocker;
    [SerializeField] private string abilityToUnlock;

    private void Start()
    {
        abilityUnlocker = GetComponent<PlayerAbilityUnlocker>();
        InitializeEntity();
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }
        HandlePlayerTargeting();
        HandleRockAttack();
    }

    private void HandleRockAttack()
    {
        if (rockAttackCDTimer > 0)
        {
            rockAttackCDTimer -= Time.deltaTime;
            return;
        }
        if (!aggroed)
        {
            return;
        }
        SpawnRock();
    }

    public override IEnumerator DeathSequence()
    {
        abilityUnlocker.UnlockPlayerAbility(abilityToUnlock);
        return base.DeathSequence();
    }

    private void SpawnRock()
    {
        rockAttackCDTimer = rockAttackCD;
        EnemySpecialAttackIndicator attackIndicator = Instantiate(rockAttackIndicator, 
            player.transform.position, Quaternion.identity).GetComponent<EnemySpecialAttackIndicator>();
        attackIndicator.StartCoroutine(attackIndicator.SpawnProjectileAfterDelay(rockAttackPrefab, rockAttackSpawnDelay));
    }
}
