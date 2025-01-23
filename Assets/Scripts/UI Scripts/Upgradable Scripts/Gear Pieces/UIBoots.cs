using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBoots : UIGearPiece
{
    [SerializeField] private float[] healthGrowthValues;
    [SerializeField] private float[] armorGrowthValues;
    [SerializeField] private float[] moveSpeedGrowthValues;

    public float currentHealthGrowth;
    public float currentArmorGrowth;
    public float currentSpeedGrowth;

    public override void UpgradePlayerStats()
    {
        currentHealthGrowth += healthGrowthValues[currentLevelIndex];
        currentArmorGrowth -= armorGrowthValues[currentLevelIndex];
        currentSpeedGrowth += moveSpeedGrowthValues[currentLevelIndex];
        player.health += currentHealthGrowth;
        if (player.health > player.maxHealth)
        {
            player.health = player.maxHealth;
        }
        base.UpgradePlayerStats();
    }

    public override string GetStats()
    {
        if (currentLevelIndex == 0 || currentLevelIndex == healthGrowthValues.Length)
        {
            float healthGrowth = currentLevelIndex == 0 ? healthGrowthValues[currentLevelIndex] : currentHealthGrowth;
            float armorGrowth = Mathf.Abs(currentLevelIndex == 0 ? armorGrowthValues[currentLevelIndex] : currentArmorGrowth);
            float speedGrowth = currentLevelIndex == 0 ? moveSpeedGrowthValues[currentLevelIndex] : currentSpeedGrowth;
            string healthGrowthText = $"Health Increase: +{healthGrowth}\n";
            string armorGrowthText = $"Armor: +{(100 * armorGrowth).ToString("0.0")}%\n";
            string speedGrowthText = $"Speed Increase: +{speedGrowth}\n";
            return healthGrowthText + armorGrowthText + speedGrowthText;
        }
        else
        {
            float previousHealthGrowth = currentHealthGrowth;
            float previousArmorGrowth = Mathf.Abs(100 * currentArmorGrowth);
            float previousSpeedGrowth = currentSpeedGrowth;

            float nextHealthGrowth = currentHealthGrowth + healthGrowthValues[currentLevelIndex];
            float nextArmorGrowth = Mathf.Abs(100 * (currentArmorGrowth - armorGrowthValues[currentLevelIndex]));
            float nextSpeedGrowth = currentSpeedGrowth + moveSpeedGrowthValues[currentLevelIndex];

            string healthGrowthText = $"Health Increase: +{previousHealthGrowth} -> +{nextHealthGrowth}\n";
            string armorGrowthText = $"Armor Increase: +{previousArmorGrowth.ToString("0.0")}% -> +{nextArmorGrowth.ToString("0.0")}%\n";
            string speedGrowthText = $"Speed Increase: +{previousSpeedGrowth} -> +{nextSpeedGrowth}\n";
            return healthGrowthText + armorGrowthText + speedGrowthText;
        }
    }
}
