using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDash : UIAbility
{
    [SerializeField] private float[] dashCDValues;
    [SerializeField] private float[] dashCostValues;
    [SerializeField] private float[] dashDamageValues;
    public float currentDashCD;
    public float currentDashCost;
    public float currentDashDamage;

    public override void UpgradePlayerStats()
    {
        currentDashCD -= dashCDValues[currentLevelIndex];
        currentDashCost -= dashCostValues[currentLevelIndex];
        currentDashDamage += dashDamageValues[currentLevelIndex];
        base.UpgradePlayerStats();
    }

    public override string GetStats()
    {
        if (currentLevelIndex == 0 || currentLevelIndex == dashCDValues.Length)
        {
            int currentIndex = currentLevelIndex == 0 ? 0 : currentLevelIndex - 1;
            float nextDashCD = Mathf.Abs(currentDashCD - dashCDValues[currentIndex]);
            float nextDashCost = Mathf.Abs(100 * (currentDashCost - dashCostValues[currentIndex]));
            float nextDashDamage = currentDashDamage + dashDamageValues[currentIndex];
            string dashCDText = $"Cooldown: -{nextDashCD}s\n";
            string dashCostText = $"Health Cost: -{nextDashCost.ToString("0.0")}%\n";
            string dashDamageText = $"Bullet Damage: +{nextDashDamage}\n";
            return dashCDText + dashCostText + dashDamageText;
        }
        else
        {
            float previousDashCD = Mathf.Abs(dashCDValues[currentLevelIndex - 1]);
            float previousDashCost = Mathf.Abs(100 * dashCostValues[currentLevelIndex - 1]);
            float previousDashDamage = dashDamageValues[currentLevelIndex - 1];

            float nextDashCD = Mathf.Abs(currentDashCD - dashCDValues[currentLevelIndex]);
            float nextDashCost = Mathf.Abs(100 * (currentDashCost - dashCostValues[currentLevelIndex]));
            float nextDashDamage = currentDashDamage + dashDamageValues[currentLevelIndex];

            string dashCDText = $"Cooldown: -{previousDashCD}s -> {nextDashCD}s\n";
            string dashCostText = $"Health Cost: -{previousDashCost.ToString("0.0")}% -> -{nextDashCost.ToString("0.0")}%\n";
            string dashDamageText = $"Bullet Damage: +{previousDashDamage} -> +{nextDashDamage}\n";
            return dashCDText + dashCostText + dashDamageText;
        }
    }
}
