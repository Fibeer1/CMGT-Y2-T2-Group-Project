using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodOrb : Pickupable
{
    [SerializeField] private float bloodGainAmount = 5;

    [Header("Text Variables")]
    [SerializeField] private Color textColor = new Color(0.35f, 0, 0);
    [SerializeField] private float textSize = 3;
    [SerializeField] private float textFadeDuration = 0.1f;
    [SerializeField] private float textLifetime = 0.5f;

    private void Update()
    {
        HandleMoving();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Entity collisionEntity = collision.transform.GetComponent<Entity>();

        if (collisionEntity != null && collision.transform == target)
        {
            TextPopUp3D.PopUpText(collision.transform.position + Vector3.up / 2, bloodGainAmount.ToString(), 
                textSize, textColor, textFadeDuration, textLifetime);
            collisionEntity.ChangeHealth(-bloodGainAmount);
            //Negative value because if it's positive it would hurt the entity
            Destroy(gameObject);
        }
    }

    public override void OnCollision(Player player)
    {
        TextPopUp3D.PopUpText(player.transform.position + Vector3.up / 2, bloodGainAmount.ToString(),
                textSize, textColor, textFadeDuration, textLifetime);
        player.ChangeHealth(-bloodGainAmount);
        //Negative value because if it's positive it would hurt the entity
        Destroy(gameObject);
    }
}
