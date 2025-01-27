using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miniboss : Enemy
{
    [SerializeField] private GameObject rockAttackPrefab;
    [SerializeField] private float rockAttackCDTimer;
    [SerializeField] private float rockAttackCD;

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
        SpawnRock();
    }

    private void SpawnRock()
    {
        rockAttackCDTimer = rockAttackCD;
        Vector3 targetPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        Instantiate(rockAttackPrefab, targetPos, Quaternion.identity);
    }
}
