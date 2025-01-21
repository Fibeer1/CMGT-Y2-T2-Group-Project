using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBullet : Projectile
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionDamage;


    public override void OnTriggerEnter(Collider other)
    {
        //Spawn explosion when the bullet hits an obstacle/enemy


        Entity entityScript = other.transform.GetComponent<Entity>();
        if (entityScript != null && !entitiesHit.Contains(entityScript) && entityScript.allegiance != origin.allegiance)
        {
            OnHit(entityScript);
        }

        if (other.gameObject.layer == obstacleLayer)
        {
            Explosion explosionInstance = Instantiate(explosionPrefab, transform.position,
            explosionPrefab.transform.rotation).GetComponent<Explosion>();
            explosionInstance.InitializeProjectile(origin, explosionDamage);
            DestroyProjectile();
        }        
        
    }

    public override void OnHit(Entity victim)
    {
        if (entitiesHit.Contains(victim))
        {
            return;
        }
        entitiesHit.Add(victim);
        victim.ChangeHealth(damage);
    }
}
