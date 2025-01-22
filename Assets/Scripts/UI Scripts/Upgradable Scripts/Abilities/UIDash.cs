using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDash : UIAbility
{
    [SerializeField] private float[] dashCDValues;
    [SerializeField] private float[] dashCostValues;
    [SerializeField] private float[] dashDamageValues;

    public override void UpgradePlayerStats()
    {
        player.dashCD -= dashCDValues[currentLevelIndex];
        player.dashHealthCost -= dashCostValues[currentLevelIndex];
        player.dashProjectileDamage += dashDamageValues[currentLevelIndex];
        base.UpgradePlayerStats();
    }

    public override string GetStats()
    {
        if (currentLevelIndex == 0 || currentLevelIndex == dashCDValues.Length)
        {
            int currentIndex = currentLevelIndex == 0 ? 0 : currentLevelIndex - 1;
            float nextDashCD = dashCDValues[currentIndex];
            float nextDashCost = 100 * dashCostValues[currentIndex];
            float nextDashDamage = dashDamageValues[currentIndex];
            string dashCDText = $"Cooldown: -{nextDashCD}s\n";
            string dashCostText = $"Health Cost: -{nextDashCost.ToString("0.0")}%\n";
            string dashDamageText = $"Bullet Damage: +{nextDashDamage}\n";
            return dashCDText + dashCostText + dashDamageText;
        }
        else
        {
            float previousDashCD = dashCDValues[currentLevelIndex - 1];
            float previousDashCost = 100 * dashCostValues[currentLevelIndex - 1];
            float previousDashDamage = dashDamageValues[currentLevelIndex - 1];
            float nextDashCD = dashCDValues[currentLevelIndex];
            float nextDashCost = 100 * dashCostValues[currentLevelIndex];
            float nextDashDamage = dashDamageValues[currentLevelIndex];
            string dashCDText = $"Cooldown: -{previousDashCD}s -> {nextDashCD}s\n";
            string dashCostText = $"Health Cost: -{previousDashCost.ToString("0.0")}% -> -{nextDashCost.ToString("0.0")}%\n";
            string dashDamageText = $"Bullet Damage: +{previousDashDamage} -> +{nextDashDamage}\n";
            return dashCDText + dashCostText + dashDamageText;
        }
    }
}
