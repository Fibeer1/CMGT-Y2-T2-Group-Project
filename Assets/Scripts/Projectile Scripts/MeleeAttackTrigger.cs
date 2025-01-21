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
}
