using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Projectile
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxSize;
    [SerializeField] private float sizeIncreaseMultiplier = 3;
    private float size;

    private void Update()
    {
        HandleLifeTime();
        HandleSizeIncrease();
    }

    private void HandleSizeIncrease()
    {
        if (size <= maxSize)
        {
            size += Time.deltaTime * sizeIncreaseMultiplier;
        }        
        transform.localScale = new Vector3(size, size, 1);
        transform.Rotate(new Vector3(0, rotationSpeed, 0), Space.World);
    }

    public override void OnHit(Entity victim, bool shouldDestroyProjectile = false)
    {
        base.OnHit(victim, shouldDestroyProjectile);
    }
}
