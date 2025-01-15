using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodOrb : Pickupable
{
    [SerializeField] private float bloodGainAmount = 5;

    private void Update()
    {
        HandleMoving();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Entity collisionEntity = collision.transform.GetComponent<Entity>();

        if (collisionEntity != null && collision.transform == target)
        {
            collisionEntity.ChangeHealth(-bloodGainAmount);
            //Negative value because if it's positive it would hurt the entity
            Destroy(gameObject);
        }
    }
}
