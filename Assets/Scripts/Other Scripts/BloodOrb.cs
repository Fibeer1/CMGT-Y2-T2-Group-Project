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

    public override void OnCollision(Player player)
    {
        player.ChangeHealth(-bloodGainAmount);
        //Negative value because if it's positive it would hurt the entity
        Destroy(gameObject);
    }
}
