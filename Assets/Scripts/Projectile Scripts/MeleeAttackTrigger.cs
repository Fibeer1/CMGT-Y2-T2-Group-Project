using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackTrigger : Projectile
{
    private Animator animator;
    [SerializeField] private string swingAnimationName;

    public override void InitializeProjectile(Entity pOrigin, float pDamage)
    {
        base.InitializeProjectile(pOrigin, pDamage);
        animator = GetComponentInChildren<Animator>();
        animator.Play(swingAnimationName);
    }

    private void Update()
    {
        HandleLifeTime();
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
