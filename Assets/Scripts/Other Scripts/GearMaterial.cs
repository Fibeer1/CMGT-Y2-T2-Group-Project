using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearMaterial : Pickupable
{
    [SerializeField] private string materialName;
    public int materialTierIndex;
    [SerializeField] private int materialCount = 5;

    [Header("Text Variables")]
    [SerializeField] private Color textColor = new Color(0.35f, 0.35f, 0.35f);
    [SerializeField] private float textSize = 3;
    [SerializeField] private float textFadeDuration = 0.1f;
    [SerializeField] private float textLifetime = 0.5f;

    private void Update()
    {
        HandleMoving();
    }

    public override void OnCollision(Player player)
    {
        player.materialCounts[materialTierIndex] += materialCount;
        TextPopUp3D.PopUpText(player.transform.position + Vector3.up / 2, materialName + " +" + materialCount,
                textSize, textColor, textFadeDuration, textLifetime);
        Destroy(gameObject);
    }   
}
