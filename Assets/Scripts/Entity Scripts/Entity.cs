using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("General Variables")]
    [SerializeField] private protected GameObject deathEffect;
    public string allegiance;
    public bool isDead = false;
    public float health = 100;
    public float maxHealth = 100;
    public float armor = 0;

    public virtual void InitializeEntity()
    {

    }

    public virtual void ChangeHealth(float healthChangeValue, bool shieldDamage = true)
    {
        healthChangeValue -= armor;
        health -= healthChangeValue;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
        if (health <= 0)
        {
            StartCoroutine(DeathSequence());
        }
    }

    public virtual IEnumerator DeathSequence()
    {
        if (isDead)
        {
            yield break;
        }
        isDead = true;
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        yield return null;
        Destroy(gameObject);
    }
}
