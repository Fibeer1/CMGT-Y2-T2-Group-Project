using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Entity origin;
    [SerializeField] private protected float damage = 1;
    public float speed = 40f;   
    private protected Rigidbody rb;
    [SerializeField] private protected float lifeTime;
    private protected List<Entity> entitiesHit = new List<Entity>();
    [SerializeField] private bool destroyProjectileOnHit = false;

    private void Update()
    {
        HandleLifeTime();
        HandleMovement();
    }

    public virtual void InitializeProjectile(Entity pOrigin, float pDamage)
    {
        rb = GetComponent<Rigidbody>();
        origin = pOrigin;
        damage = pDamage;
        Physics.IgnoreCollision(GetComponent<Collider>(), pOrigin.GetComponent<Collider>());
    }

    private protected void HandleMovement()
    {
        if (speed > 0)
        {
            rb.velocity = transform.forward * speed;
        }
    }

    private protected void HandleLifeTime()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            DestroyProjectile();
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        Entity entityScript = other.transform.GetComponent<Entity>();
        if (entityScript != null && !entitiesHit.Contains(entityScript) && entityScript.allegiance != origin.allegiance)
        {
            OnHit(entityScript);
        }
    }

    public virtual void OnHit(Entity victim)
    {
        if (entitiesHit.Contains(victim))
        {
            return;
        }
        entitiesHit.Add(victim);
        victim.ChangeHealth(damage);
        if (destroyProjectileOnHit)
        {
            DestroyProjectile();
        }
    }

    private protected void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
