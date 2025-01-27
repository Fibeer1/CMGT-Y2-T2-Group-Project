using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossRock : Entity
{
    [Header("General Variables")]
    [SerializeField] private float lifeTime = 60;
    [SerializeField] private string spawnAnim;
    [SerializeField] private string despawnAnim;
    [SerializeField] private float despawnAnimDuration;

    [Header("Material Drop Variables")]
    [SerializeField] private GameObject bloodOrbPrefab;
    [SerializeField] private int bloodOrbsOnDeath;
    [SerializeField] private float pickupableSpawnSpeed = 5;

    [Header("Aftershock Variables")]
    [SerializeField] private GameObject aftershockPrefab;
    [SerializeField] private float damage;
    [SerializeField] private float aftershockSpawnDelay = 0.25f;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(spawnAnim);
        Invoke("SpawnAftershock", aftershockSpawnDelay);
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }
        HandleLifetime();
    }

    private void HandleLifetime()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            StartCoroutine(DeathSequence());
        }
    }

    private void SpawnAftershock()
    {
        GameObject aftershockInstance = Instantiate(aftershockPrefab,
            transform.position, Quaternion.identity, transform);
        aftershockInstance.GetComponent<Projectile>().InitializeProjectile(this, damage);
        //AudioManager.instance.PlayOneShot(rockSpawnSound, transform.position);
    }

    public override IEnumerator DeathSequence()
    {
        if (isDead)
        {
            yield break;
        }
        animator.Play(despawnAnim);
        DropPickupables();
        yield return new WaitForSeconds(despawnAnimDuration);
        StartCoroutine(base.DeathSequence());
    }

    private void DropPickupables()
    {
        for (int i = 0; i < bloodOrbsOnDeath; i++)
        {
            LaunchBloodOrbs(pickupableSpawnSpeed);
        }
    }

    private void LaunchBloodOrbs(float pickupableSpeed)
    {
        float angle = Random.Range(-360, 360);
        Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
        GameObject currentOrbInstance = Instantiate(bloodOrbPrefab, transform.position, Quaternion.identity);
        currentOrbInstance.GetComponent<Rigidbody>().velocity = direction * pickupableSpeed;
    }
}
