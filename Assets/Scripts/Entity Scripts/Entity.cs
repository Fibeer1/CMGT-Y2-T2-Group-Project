using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("General Variables")]
    [SerializeField] private protected GameObject deathEffect;
    public string allegiance;
    public bool isDead = false;
    public float health = 5;
    public float maxHealth = 5;

    public virtual void InitializeEntity()
    {

    }

    public virtual void ChangeHealth(float healthChangeValue)
    {
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
