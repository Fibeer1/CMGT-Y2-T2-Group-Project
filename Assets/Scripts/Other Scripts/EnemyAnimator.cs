using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [Header("Character Position Variables")]
    [SerializeField] private Transform characterObject;
    [SerializeField] private Sprite frontSprite;
    [SerializeField] private Sprite backSprite;
    private SpriteRenderer spriteRenderer;
    private Vector3 characterObjectOffset;
    private Vector3 characterObjectScale;

    private Enemy enemyScript;
    private Animator animator;    
    public float attackAnimDuration;
    private bool duringSpecialAnim = false;
    [SerializeField] private string currentAnimState;
    [SerializeField] private string idleAnim = "EnemyIdle";
    public string attackAnim = "EnemyAttack";
    [SerializeField] private string runAnim = "EnemyRun";

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        enemyScript = GetComponent<Enemy>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
        float horizontalAngleDiff = Vector3.Dot(diff, transform.right);
        float verticalAngleDiff = Vector3.Dot(diff, transform.forward);
        Vector3 targetPosition = characterObjectOffset;
        Vector3 targetScale = characterObjectScale;

        targetPosition.x = characterObjectOffset.x * (horizontalAngleDiff >= 0 ? -1 : 1);
        targetScale.x = characterObjectScale.x * (horizontalAngleDiff >= 0 ? -1 : 1);
        if (backSprite != null && frontSprite != null)
        {
            spriteRenderer.sprite = verticalAngleDiff >= 0 ? backSprite : frontSprite;
        }
        characterObject.localPosition = targetPosition;
        characterObject.localScale = targetScale;
    }

    private void HandleAnimations()
    {
        if (animator == null || duringSpecialAnim)
        {
            return;
        }

        if (enemyScript.meshAgent.velocity.x != 0)
        {
            ChangeAnimationState(runAnim);
        }

        if (enemyScript.meshAgent.velocity == Vector3.zero)
        {
            ChangeAnimationState(idleAnim);
        }
    }

    public IEnumerator SpecialAnimation(string animName, float animDuration)
    {
        if (duringSpecialAnim)
        {
            yield break;
        }
        duringSpecialAnim = true;
        ChangeAnimationState(animName);
        yield return new WaitForSeconds(animDuration);
        duringSpecialAnim = false;
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
