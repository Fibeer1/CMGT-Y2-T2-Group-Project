using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBullet : Projectile
{
    [SerializeField] private GameObject explosionPrefab;

    public override void OnHit(Entity victim, bool shouldDestroyProjectile = true)
    {
        Explosion explosionInstance = Instantiate(explosionPrefab, transform.position, 
            explosionPrefab.transform.rotation).GetComponent<Explosion>();
        explosionInstance.InitializeProjectile(origin, damage);
        base.OnHit(victim);
    }
}
