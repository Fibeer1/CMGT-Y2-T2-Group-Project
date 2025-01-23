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

    public override void OnHit(Entity victim)
    {
        base.OnHit(victim);
        Player playerOrigin = origin.GetComponent<Player>();
        if (playerOrigin != null)
        {
            float healthChange = playerOrigin.lifeSteal;
            playerOrigin.ChangeHealth(-healthChange, false, false, true, false);
        }
    }
}
