using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBullet : Projectile
{
    [SerializeField] private GameObject explosionPrefab;
    public float explosionDamage;


    public override void OnTriggerEnter(Collider other)
    {
        //Spawn explosion when the bullet hits an obstacle/enemy

        Entity entityScript = other.transform.GetComponent<Entity>();
        if (entityScript != null && !entitiesHit.Contains(entityScript) && entityScript.allegiance != origin.allegiance)
        {
            SpawnExplosion();
            OnHit(entityScript);
        }

        if (other.gameObject.tag == obstacleTag)
        {
            SpawnExplosion();
            DestroyProjectile();
        }
    }

    private void SpawnExplosion()
    {
        Explosion explosionInstance = Instantiate(explosionPrefab, transform.position,
            explosionPrefab.transform.rotation).GetComponent<Explosion>();
        explosionInstance.InitializeProjectile(origin, explosionDamage);
    }
}
