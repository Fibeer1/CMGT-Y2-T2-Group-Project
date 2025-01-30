using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
public class Miniboss : Enemy
{
    [SerializeField] private GameObject rockAttackIndicator;
    [SerializeField] private GameObject rockAttackPrefab;
    [SerializeField] private float rockAttackCDTimer;
    [SerializeField] private float rockAttackCD;
    [SerializeField] private float rockAttackSpawnDelay;
    private PlayerAbilityUnlocker abilityUnlocker;
    [SerializeField] private int abilityIndexToUnlock;
    [SerializeField] private GameObject bossHealthBarPrefab;
    private GameObject healthBarInstance;
    private Slider healthBarSlider;

    [SerializeField] private EventReference bossRockSummonSound;
    [SerializeField] private EventReference bossRockAttackSound;

    private void Start()
    {
        abilityUnlocker = GetComponent<PlayerAbilityUnlocker>();
        healthBarInstance = Instantiate(bossHealthBarPrefab, FindObjectOfType<Canvas>().transform.Find("HUD"));
        healthBarSlider = healthBarInstance.GetComponent<Slider>();
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = health;
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
        HandleHealthBar();
    }

    private void HandleHealthBar()
    {
        if (aggroed && !healthBarInstance.activeSelf)
        {
            healthBarInstance.SetActive(true);
        }
        if (!aggroed && healthBarInstance.activeSelf)
        {
            healthBarInstance.SetActive(false);
        }
        healthBarSlider.value = health;
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
        AudioManager.instance.PlayOneShot(bossRockAttackSound, this.transform.position);
    }

    public override IEnumerator DeathSequence()
    {
        abilityUnlocker.UnlockPlayerAbility(abilityIndexToUnlock);
        Destroy(healthBarInstance);
        return base.DeathSequence();
    }

    private void SpawnRock()
    {
        AudioManager.instance.PlayOneShot(bossRockSummonSound, this.transform.position);
        rockAttackCDTimer = rockAttackCD;
        EnemySpecialAttackIndicator attackIndicator = Instantiate(rockAttackIndicator, 
            player.transform.position, Quaternion.identity).GetComponent<EnemySpecialAttackIndicator>();
        attackIndicator.StartCoroutine(attackIndicator.SpawnProjectileAfterDelay(rockAttackPrefab, rockAttackSpawnDelay));
    }
}
