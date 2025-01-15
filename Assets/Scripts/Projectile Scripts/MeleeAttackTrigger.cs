using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackTrigger : Projectile
{
    private void OnTriggerEnter(Collider collision)
    {
        Entity collisionEntity = collision.transform.GetComponent<Entity>();
        if (collisionEntity != null && origin.allegiance != collisionEntity.allegiance)
        {
            collisionEntity.ChangeHealth(damage);
        }
    }
}
