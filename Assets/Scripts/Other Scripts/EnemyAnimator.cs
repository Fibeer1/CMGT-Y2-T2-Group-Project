using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [Header("Animation Triggers")]
    public bool shouldAttack = false;

    private Enemy enemyScript;
    private Animator animator;    
    [SerializeField] private float attackAnimDuration;
    private bool duringAttackAnim = false;
    private string currentAnimState;
    [SerializeField] private string idleAnim = "EnemyIdleDown";
    [SerializeField] private string attackDownAnim = "EnemyAttackDown";
    [SerializeField] private string attackUpAnim = "EnemyAttackUp";
    [SerializeField] private string attackLeftAnim = "EnemyAttackLeft";
    [SerializeField] private string attackRightAnim = "EnemyAttackRight";
    [SerializeField] private string runDownAnim = "EnemyRunDown";
    [SerializeField] private string runUpAnim = "EnemyRunUp";
    [SerializeField] private string runLeftAnim = "EnemyRunLeft";
    [SerializeField] private string runRightAnim = "EnemyRunRight";

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyScript = GetComponent<Enemy>();
        ChangeAnimationState(idleAnim);
    }

    private void Update()
    {
        HandleAnimations();
    }

    private void HandleAnimations()
    {
        if (shouldAttack)
        {
            shouldAttack = false;
            if (!duringAttackAnim)
            {
                duringAttackAnim = true;
                string targetAnim;
                float angle = enemyScript.enemyAttackRotator.localRotation.eulerAngles.y;
                if ((angle >= 0 && angle <= 45) || (angle >= 315 && angle <= 360))
                {
                    targetAnim = attackUpAnim;
                }
                else if (angle >= 45 && angle <= 135)
                {
                    targetAnim = attackRightAnim;
                }
                else if (angle >= 225 && angle < 315)
                {
                    targetAnim = attackLeftAnim;
                }
                else
                {
                    targetAnim = attackDownAnim;
                }
                ChangeAnimationState(targetAnim);
                Invoke("StopAttackAnim", attackAnimDuration);
            }
        }
        if (duringAttackAnim)
        {
            return;
        }

        if (!duringAttackAnim || Vector3.Distance(enemyScript.player.transform.position, 
            transform.position) >= enemyScript.deAggroRange)
        {
            ChangeAnimationState(idleAnim);
        }
    }

    private void StopAttackAnim()
    {
        duringAttackAnim = false;
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentAnimState == newState)
        {
            return;
        }

        animator.Play(newState);
        currentAnimState = newState;
    }
}
