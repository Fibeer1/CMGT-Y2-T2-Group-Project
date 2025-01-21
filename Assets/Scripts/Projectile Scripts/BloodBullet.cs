using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBullet : Projectile
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionDamage;

    public override void OnCollision(Collision collision)
    {
        Explosion explosionInstance = Instantiate(explosionPrefab, transform.position, 
            explosionPrefab.transform.rotation).GetComponent<Explosion>();
        explosionInstance.InitializeProjectile(origin, explosionDamage);
        base.OnCollision(collision);
    }
}
