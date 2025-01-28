using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [Header("Animation Triggers")]
    public bool shouldAttack = false;

    [Header("Character Position Variables")]
    [SerializeField] private Transform characterObject;
    private Vector3 characterObjectOffset;
    private Vector3 characterObjectScale;

    private Enemy enemyScript;
    private Animator animator;    
    [SerializeField] private float attackAnimDuration;
    [SerializeField] private bool duringAttackAnim = false;
    [SerializeField] private string currentAnimState;
    [SerializeField] private string idleAnim = "EnemyIdleDown";
    [SerializeField] private string attackDownAnim = "EnemyAttackDown";
    [SerializeField] private string attackUpAnim = "EnemyAttackUp";
    [SerializeField] private string attackAnim = "EnemyAttackLeft";
    [SerializeField] private string runDownAnim = "EnemyRunDown";
    [SerializeField] private string runUpAnim = "EnemyRunUp";
    [SerializeField] private string runAnim = "EnemyRun";
    private bool facingRight;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        enemyScript = GetComponent<Enemy>();
        characterObjectOffset = characterObject.localPosition;
        characterObjectScale = characterObject.localScale;
        ChangeAnimationState(idleAnim);
    }

    private void Update()
    {
        HandleAnimations();
        HandleSpriteFlip();
    }

    private void HandleSpriteFlip()
    {
        if (!enemyScript.aggroed)
        {
            return;
        }
        Vector3 diff = (enemyScript.player.transform.position - enemyScript.transform.position).normalized;
        float angleDiff = Vector3.Dot(diff, transform.right);
        Vector3 targetPosition = characterObjectOffset;
        Vector3 targetScale = characterObjectScale;

        targetPosition.x = characterObjectOffset.x * (angleDiff >= 0 ? -1 : 1);
        targetScale.x = characterObjectScale.x * (angleDiff >= 0 ? -1 : 1);
        characterObject.localPosition = targetPosition;
        characterObject.localScale = targetScale;
    }

    private void HandleAnimations()
    {
        if (animator == null)
        {
            return;
        }
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
                else if ((angle >= 45 && angle <= 135) || (angle >= 225 && angle < 315))
                {
                    targetAnim = attackAnim;
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

        if (enemyScript.meshAgent.velocity.x != 0)
        {
            ChangeAnimationState(runAnim);
        }
        if (enemyScript.meshAgent.velocity.z > 0)
        {
            ChangeAnimationState(runUpAnim);
        }
        else if (enemyScript.meshAgent.velocity.z < 0)
        {
            ChangeAnimationState(runDownAnim);
        }

        if (enemyScript.meshAgent.velocity == Vector3.zero)
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
        if (currentAnimState == newState || animator == null)
        {
            return;
        }

        animator.Play(newState);
        currentAnimState = newState;
    }
}
