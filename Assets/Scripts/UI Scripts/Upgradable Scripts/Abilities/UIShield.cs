using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShield : UIAbility
{
    [SerializeField] private float[] shieldCDValues;
    [SerializeField] private float[] shieldCostValues;
    [SerializeField] private float[] shieldFlatValues;

    public override void UpgradePlayerStats()
    {
        player.shieldCD -= shieldCDValues[currentLevelIndex];
        player.shieldMaxHPCost -= shieldCostValues[currentLevelIndex];
        player.fixedShieldAmount += shieldFlatValues[currentLevelIndex];
        base.UpgradePlayerStats();
    }

    public override string GetStats()
    {
        if (currentLevelIndex == 0 || currentLevelIndex == shieldCDValues.Length)
        {
            int currentIndex = currentLevelIndex == 0 ? 0 : currentLevelIndex - 1;
            float nextShieldCD = shieldCDValues[currentIndex];
            float nextShieldCost = 100 * shieldCostValues[currentIndex];
            float nextShieldFlat = shieldFlatValues[currentIndex];
            string shieldCDText = $"Cooldown: -{nextShieldCD}s\n";
            string shieldCostText = $"Health Cost: -{nextShieldCost.ToString("0.0")}%\n";
            string shieldFlatText = $"Flat Shield HP: +{nextShieldFlat}\n";
            return shieldCDText + shieldCostText + shieldFlatText;
        }
        else
        {
            float previousShieldCD = shieldCDValues[currentLevelIndex - 1];
            float previousShieldCost = 100 * shieldCostValues[currentLevelIndex - 1];
            float previousShieldFlat = shieldFlatValues[currentLevelIndex - 1];
            float nextShieldCD = shieldCDValues[currentLevelIndex];
            float nextShieldCost = 100 * shieldCostValues[currentLevelIndex];
            float nextShieldFlat = shieldFlatValues[currentLevelIndex];
            string shieldCDText = $"Cooldown: -{previousShieldCD}s -> {nextShieldCD}s\n";
            string shieldCostText = $"Health Cost: -{previousShieldCost.ToString("0.0")}% -> -{nextShieldCost.ToString("0.0")}%\n";
            string shieldFlatText = $"Flat Shield HP: +{previousShieldFlat} -> +{nextShieldFlat}\n";
            return shieldCDText + shieldCostText + shieldFlatText;
        }
    }
}
