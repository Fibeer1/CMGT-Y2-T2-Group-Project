using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float shieldHealth;
    private float shieldDuration;
    private Player player;

    public void InitializeShield(Player pPlayer, float pShieldHealth, float pDuration)
    {
        player = pPlayer;
        shieldHealth = pShieldHealth;
        shieldDuration = pDuration;
    }

    private void Update()
    {
        HandleLifeTime();
    }

    private void HandleLifeTime()
    {
        shieldDuration -= Time.deltaTime;
        if (shieldDuration <= 0)
        {
            player.DestroyShield();
        }
    }
}
