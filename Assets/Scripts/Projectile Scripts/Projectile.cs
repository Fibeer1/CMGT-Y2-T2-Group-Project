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

    [Header("Text Variables")]
    [SerializeField] private protected Color textColor = new Color(0.35f, 0, 0);
    [SerializeField] private protected float textSize = 3;
    [SerializeField] private protected float textFadeDuration = 0.1f;
    [SerializeField] private protected float textLifetime = 0.5f;

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

    public virtual void OnCollisionEnter(Collision collision)
    {
        Entity entityScript = collision.transform.GetComponent<Entity>();
        if (entityScript != null && !entitiesHit.Contains(entityScript) && entityScript.allegiance != origin.allegiance)
        {
            OnHit(entityScript);
        }
        OnCollision(collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity entityScript = other.transform.GetComponent<Entity>();
        if (entityScript != null && !entitiesHit.Contains(entityScript) && entityScript.allegiance != origin.allegiance)
        {
            OnHit(entityScript, false);
        }
    }

    public virtual void OnCollision(Collision collision)
    {
        DestroyProjectile();
    }

    public virtual void OnHit(Entity victim, bool shouldDestroyProjectile = true)
    {
        entitiesHit.Add(victim);
        TextPopUp3D.PopUpText(victim.transform.position + Vector3.up / 2, damage.ToString(), textSize, textColor, textFadeDuration, textLifetime);
        victim.ChangeHealth(damage);
        if (shouldDestroyProjectile)
        {
            DestroyProjectile();
        }
    }

    public virtual void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
