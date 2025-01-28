using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityUnlocker : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void UnlockPlayerAbility(string abilityName)
    {
        if (abilityName == "Dash")
        {
            player.canDash = true;
        }
        if (abilityName == "Bullet")
        {
            player.canShootBullet = true;
        }
        if (abilityName == "Shield")
        {
            player.canUseShield = true;
        }
    }
}
