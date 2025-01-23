using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class Entity : MonoBehaviour
{
    [Header("General Variables")]
    [SerializeField] private protected GameObject deathEffect;
    public string allegiance;
    public bool isDead = false;
    public float health = 100;
    public float maxHealth = 100;
    public float armor = 1;

    [Header("Health Change Text Variables")]
    [SerializeField] private Color healTextColor = new Color(0, 0.35f, 0);
    [SerializeField] private Color damageTextColor = new Color(0.35f, 0, 0);
    [SerializeField] private float textSize = 3;
    [SerializeField] private float textFadeDuration = 0.1f;
    [SerializeField] private float textLifeTime = 0.5f;

    [Header("Sounds")]
    [SerializeField] private EventReference enemyTakeDamageSound;
    [SerializeField] private EventReference takeDamageSound;
    public virtual void InitializeEntity()
    {

    }

    public virtual void ChangeHealth(float healthChangeValue, bool shieldDamage = true, 
        bool shouldAccountForArmor = true, bool shouldDisplayDamageText = true, bool shouldPlaySound = true)
    {
        Color textColor = healthChangeValue >= 0 ? damageTextColor : healTextColor;
        if (healthChangeValue >= 0 && shouldAccountForArmor)
        {
            healthChangeValue *= armor;
        }
        health -= healthChangeValue;
        if (shouldPlaySound)
        {
            //Play sound
            if (GetComponent<Enemy>() != null)
            {
                //AudioManager.instance.PlayOneShot(enemyTakeDamageSound, this.transform.position);
            }
            else if (GetComponent<Player>() != null)
            {
                //AudioManager.instance.PlayOneShot(takeDamageSound, this.transform.position);
            }

        }
        if (health > maxHealth)
        {
            health = maxHealth; 
        }
        bool isValueFloat = Mathf.Abs(healthChangeValue) % 1 != 0;
        if (shouldDisplayDamageText)
        {
            string healthChangeText = (healthChangeValue < 0 ? "+" : "") + 
                Mathf.Abs(healthChangeValue).ToString(isValueFloat ? "0.0" : "0");
            TextPopUp3D.PopUpText(transform.position + Vector3.up / 2, healthChangeText,
            textSize, textColor, textFadeDuration, textLifeTime);
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
