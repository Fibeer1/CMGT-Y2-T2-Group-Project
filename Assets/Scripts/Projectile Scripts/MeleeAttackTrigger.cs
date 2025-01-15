using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackTrigger : Projectile
{
    private Animator animator;
    [SerializeField] private string swingAnimationName;
    public Transform spawnTransform;
    public Vector3 spawnOffset;

    [Header("Text Variables")]
    [SerializeField] private Color textColor = new Color(0.35f, 0, 0);
    [SerializeField] private float textSize = 3;
    [SerializeField] private float textFadeDuration = 0.1f;
    [SerializeField] private float textLifetime = 0.5f;

    public override void InitializeProjectile(Entity pOrigin, float pDamage)
    {
        base.InitializeProjectile(pOrigin, pDamage);
        animator = GetComponentInChildren<Animator>();
        animator.Play(swingAnimationName);
    }

    private void Update()
    {
        HandleLifeTime();
        transform.position = spawnTransform.position + spawnOffset;
    }

    private void OnTriggerEnter(Collider collision)
    {
        Entity collisionEntity = collision.transform.GetComponent<Entity>();
        if (collisionEntity != null && origin.allegiance != collisionEntity.allegiance)
        {
            TextPopUp3D.PopUpText(collision.transform.position + Vector3.up / 2, damage.ToString(), textSize, textColor, textFadeDuration, textLifetime);
            collisionEntity.ChangeHealth(damage);
        }
    }
}
